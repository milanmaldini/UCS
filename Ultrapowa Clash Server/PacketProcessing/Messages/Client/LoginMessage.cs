using Blake2Sharp;
using Sodiumc;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UCS.Core;
using UCS.Logic;
using UCS.Network;
using UCS.PacketProcessing;

namespace UCS.PacketProcessing
{
    //Packet 10101
    internal class LoginMessage : Message
    {
        private static byte[] PlainText;
        private long m_vAccountId;
        private int m_vClientBuild;
        private int m_vClientContentVersion;
        private int m_vClientMajorVersion;
        private uint m_vClientSeed;
        private string m_vDevice;
        private string m_vGameVersion;
        private string m_vMacAddress;
        private string m_vOpenUDID;
        private string m_vPassToken;
        private string m_vPhoneId;
        private string m_vPreferredDeviceLanguage;
        private string m_vResourceSha;
        private string m_vSignature2;
        private string m_vSignature3;
        private string m_vSignature4;
        private string m_vUDID;

        public LoginMessage(Client client, BinaryReader br)
            : base(client, br)
        {
        }

        public static void ShowData()
        {
            using (var reader = new BinaryReader(new MemoryStream(PlainText)))
            {
                Console.WriteLine("\n[UCS] ----- FULL PACKET CONTENT #10100 ----- [/UCS]\n");
                Console.WriteLine(Encoding.UTF8.GetString(PlainText));
                Console.WriteLine("\n[UCS] ----- -------------------------- ----- [/UCS]\n");
                Console.WriteLine("User ID = " + reader.ReadInt64());
                //Console.WriteLine("UserToken = " + reader.ReadString());
                //Console.WriteLine("Major Version = " + reader.ReadInt32());
                //Console.WriteLine("Content Version = " + reader.ReadInt32());
                //Console.WriteLine("Minor Version = " + reader.ReadInt32());
                //Console.WriteLine("MasterHash = " + reader.ReadString());
                //Console.WriteLine("Unknown 1 = " + reader.ReadString());
                //Console.WriteLine("OpenUDID = " + reader.ReadString());
                //Console.WriteLine("MacAddress = " + reader.ReadString());
                //Console.WriteLine("Device Model = " + reader.ReadString());
                //Console.WriteLine("");
            }
        }

        public override void Decode()
        {
            /* The Packet Raw Data */
            var RawPacket = GetData();

            /* The client public key */
            byte[] CPublicKey = RawPacket.Take(32).ToArray();

            // Encryption start - Decryption 
            Console.WriteLine("[UCS]    Client Public Key = " + Encoding.UTF8.GetString(CPublicKey) + "\n[UCS]      Client Public Key Length = " + CPublicKey.Length);

            /* Generating Blake2B Nonce with Client Public Key */
            var CNonce = GenericHash.Hash(CPublicKey.Concat(Crypto8.StandardKeyPair.PublicKey).ToArray(), null, 24);
            Console.WriteLine("[UCS]    Client Nonce = " + Encoding.UTF8.GetString(CNonce) + "\n[UCS]      Client Nonce Length = " + CNonce.Length);

            /* The full packet content in raw data without the public key of the client */
            var cipherText = RawPacket.Skip(32).ToArray();

            /* Finally, we store the decrypted data, and use the function below for dencryption */
            PlainText = PublicKeyBox.Open(cipherText, CNonce, Crypto8.StandardKeyPair.PrivateKey, CPublicKey);

            /* We also store the Session Key of the client */
            var Skey = PlainText.Take(24).ToArray();

            // ---------------------------------- 

            ShowData();
        }

        public override void Process(Level level)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["ServerCapacityMode"]))
            {
                if (ResourcesManager.GetOnlinePlayers().Count >=
                    Convert.ToInt32(ConfigurationManager.AppSettings["ServerCapacity"]))
                {
                    if (!(level.GetAccountPrivileges() >= 1))
                    {
                        var p = new LoginFailedMessage(Client);
                        p.SetErrorCode(12);
                        p.RemainingTime(0);
                        p.SetReason(ConfigurationManager.AppSettings["ServerFullMessage"]);
                        PacketManager.ProcessOutgoingPacket(p);
                        return;
                    }
                }
            }

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["maintenanceMode"]))
            {
                var p = new LoginFailedMessage(Client);
                p.SetErrorCode(10);
                PacketManager.ProcessOutgoingPacket(p);
                return;
            }

            var versionData = ConfigurationManager.AppSettings["clientVersion"].Split('.');
            if (versionData.Length >= 2)
            {
                if (m_vClientMajorVersion != Convert.ToInt32(versionData[0]) ||
                    m_vClientBuild != Convert.ToInt32(versionData[1]))
                {
                    var p = new LoginFailedMessage(Client);
                    p.SetErrorCode(8);
                    p.SetUpdateURL("market://details?id=com.supercell.clashofclans");
                    PacketManager.ProcessOutgoingPacket(p);
                    return;
                }
            }
            else
            {
                Debugger.WriteLine("Connection failed. UCS config key clientVersion is not properly set.");
            }

            level = ResourcesManager.GetPlayer(m_vAccountId);
            if (level != null)
            //Console.WriteLine("Debug: Retrieve Player Data for player " + auth.PlayerId.ToString());
            {
                if (level.GetAccountStatus() == 99)
                {
                    var p = new LoginFailedMessage(Client);
                    p.SetErrorCode(11);
                    PacketManager.ProcessOutgoingPacket(p);
                    return;
                }
            }
            else
            {
                //New player
                level = ObjectManager.CreateAvatar(m_vAccountId);
                var tokenSeed = new byte[20];
                new Random().NextBytes(tokenSeed);
                SHA1 sha = new SHA1CryptoServiceProvider();
                m_vPassToken = BitConverter.ToString(sha.ComputeHash(tokenSeed)).Replace("-", "");
            }

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["useCustomPatch"]))
            {
                if (m_vResourceSha != ObjectManager.FingerPrint.sha)
                {
                    var p = new LoginFailedMessage(Client);
                    p.SetErrorCode(7);
                    p.SetResourceFingerprintData(ObjectManager.FingerPrint.SaveToJson());
                    p.SetContentURL(ConfigurationManager.AppSettings["patchingServer"]);
                    p.SetUpdateURL("market://details?id=com.supercell.clashofclans");
                    PacketManager.ProcessOutgoingPacket(p);
                    return;
                }
            }

            Client.ClientSeed = m_vClientSeed;
            PacketManager.ProcessOutgoingPacket(new SessionKeyMessage(Client));

            if (level.GetAccountPrivileges() > 0)
                level.GetPlayerAvatar().SetLeagueId(21);
            if (level.GetAccountPrivileges() > 4)
                level.GetPlayerAvatar().SetLeagueId(22);
            ResourcesManager.LogPlayerIn(level, Client);
            level.Tick();

            var loginOk = new LoginOkMessage(Client);
            var avatar = level.GetPlayerAvatar();
            loginOk.SetAccountId(avatar.GetId());
            loginOk.SetPassToken(m_vPassToken);
            loginOk.SetServerMajorVersion(m_vClientMajorVersion);
            loginOk.SetServerBuild(m_vClientBuild);
            loginOk.SetContentVersion(m_vClientContentVersion);
            loginOk.SetServerEnvironment("prod");
            loginOk.SetDaysSinceStartedPlaying(10);
            loginOk.SetServerTime(
                Math.Round(level.GetTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds * 1000).ToString());
            loginOk.SetAccountCreatedDate("1414003838000");
            loginOk.SetStartupCooldownSeconds(0);
            loginOk.SetCountryCode("FR");
            PacketManager.ProcessOutgoingPacket(loginOk);

            var alliance = ObjectManager.GetAlliance(level.GetPlayerAvatar().GetAllianceId());
            if (alliance == null)
                level.GetPlayerAvatar().SetAllianceId(0);
            PacketManager.ProcessOutgoingPacket(new OwnHomeDataMessage(Client, level));
            if (alliance != null)
                PacketManager.ProcessOutgoingPacket(new AllianceStreamMessage(Client, alliance));
        }
    }
}

// Last edit: 20.01.2016 - iSwuerfel 