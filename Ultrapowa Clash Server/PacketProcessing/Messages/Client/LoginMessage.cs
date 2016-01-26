using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Security.Cryptography;
using UCS.Helpers;
using UCS.Core;
using UCS.Network;
using UCS.Logic;
using Blake2Sharp;
using UCS.EncryptionTesting;

namespace UCS.PacketProcessing
{
    class LoginMessageHelper
    {
        private static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            return val - (val < 58 ? 48 : 55);
        }

        /// <summary>
        /// Converts a hex string to a byte array (DEADBEEF becomes 0xDE, 0xAD, 0xBE, 0xEF)
        /// </summary>
        /// <param name="hexStr">The hex string</param>
        /// <returns></returns>
        public static byte[] StringToByteArray(string hexStr)
        {
            if (hexStr.Length % 2 == 1)
                throw new FormatException("HexStr Length is not odd (2,4,6,8,16,32,64 [...])!");

            byte[] arr = new byte[hexStr.Length >> 1];

            for (int i = 0; i < hexStr.Length >> 1; ++i)
                arr[i] = (byte)((GetHexVal(hexStr[i << 1]) << 4) + (GetHexVal(hexStr[(i << 1) + 1])));

            return arr;
        }
        
        private static byte[] Server_PK = null;
        /// <summary>
        /// Returns the generated public key in libg.so (Offset 0x
        /// </summary>
        public static byte[] ServerPublicKey
        {
            get
            {
                Server_PK = StringToByteArray("72f1a4a4c48e44da0c42310f800e96624e6dc6a641a9d41c3b5039d8dfadc27e".ToUpper());
                return Server_PK;
            }
            set
            {
                Server_PK = value;
            }
        }

        private static byte[] Server_SK = null;
        public static byte[] ServerSecretKey
        {
            get
            {
                Server_SK = StringToByteArray("1891d401fadb51d25d3a9174d472a9f691a45b974285d47729c45c6538070d85".ToUpper());
                return Server_SK;
            }
            set
            {
                Server_SK = value;
            }
        }
    }
    //Packet 10101
    class LoginMessage : Message
    {
        private long m_vAccountId;
        private string m_vPassToken;
        private int m_vClientMajorVersion;
        private int m_vClientBuild;
        private int m_vClientContentVersion;
        private string m_vResourceSha;
        private string m_vUDID;
        private string m_vOpenUDID;
        private string m_vMacAddress;
        private string m_vDevice;
        private string m_vPreferredDeviceLanguage;
        private string m_vPhoneId;
        private string m_vGameVersion;
        private string m_vSignature2;
        private string m_vSignature3;
        private string m_vSignature4;
        private uint m_vClientSeed;

        public LoginMessage(Client client, BinaryReader br)
            : base(client, br)
        {
        }



        public override void Decode()
        {
                byte[] RawPacket = GetData(); // Raw, encrypted packet;
                byte[] RawPacketWithoutPK = GetData().Skip(32).ToArray();
                byte[] PubKeyFromPacket = RawPacket.Take(32).ToArray(); // Public key from 10101
                byte[] PublicKey = LoginMessageHelper.ServerPublicKey; // Public key from the server
                byte[] PrivateKey = LoginMessageHelper.ServerSecretKey; // Private key from the server
                byte[] Nonce = null; // Blake2b nonce

                /* 11. Server generates nonce with blake2b using pk and serverkey. */
                Blake2BConfig b2conf = new Blake2BConfig(); 
                b2conf.OutputSizeInBytes = 24; 
                Hasher b2 = Blake2B.Create(b2conf);

                b2.Init();
                b2.Update(PubKeyFromPacket);
                b2.Update(PublicKey);
                Nonce = b2.Finish();

                /* 12. Server generates a shared key (s) with crypto_box_beforenm using its private key and pk. */
                byte[] sharedKey = new byte[32];
                Sodium.SodiumLibrary.crypto_box_beforenm(sharedKey, PubKeyFromPacket, PrivateKey);

                /* 13. Server decrypts packet 10101 with crypto_box_open using s and nonce. */
                byte[] decryptedPacket = Sodium.PublicKeyBox.Open(RawPacketWithoutPK, Nonce, PrivateKey, PubKeyFromPacket);

                byte[] decryptedPacketWithoutNonces = decryptedPacket.Skip(48).ToArray();
                Console.WriteLine(Encoding.UTF8.GetString(decryptedPacketWithoutNonces));
                Console.WriteLine(BitConverter.ToString(decryptedPacketWithoutNonces));

                using (var br = new BinaryReader(new MemoryStream(decryptedPacket.Skip(48).ToArray())))
                {
                   /* Process data */
                }
            
        }

        public override void Process(Level level)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["ServerCapacityMode"]))
            {
                if (ResourcesManager.GetOnlinePlayers().Count >= Convert.ToInt32(ConfigurationManager.AppSettings["ServerCapacity"]))
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

            string[] versionData = ConfigurationManager.AppSettings["clientVersion"].Split('.');
            if (versionData.Length >= 2)
            {
                if (m_vClientMajorVersion != Convert.ToInt32(versionData[0]) || m_vClientBuild != Convert.ToInt32(versionData[1]))
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
                byte[] tokenSeed = new byte[20];
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
            loginOk.SetServerTime(Math.Round((level.GetTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000).ToString());
            loginOk.SetAccountCreatedDate("1414003838000");
            loginOk.SetStartupCooldownSeconds(0);
            loginOk.SetCountryCode("FR");
            PacketManager.ProcessOutgoingPacket(loginOk);

            Alliance alliance = ObjectManager.GetAlliance(level.GetPlayerAvatar().GetAllianceId());
            if (alliance == null)
                level.GetPlayerAvatar().SetAllianceId(0);
            PacketManager.ProcessOutgoingPacket(new OwnHomeDataMessage(Client, level));
            if (alliance != null)
                PacketManager.ProcessOutgoingPacket(new AllianceStreamMessage(Client, alliance));
        }
    }
}

// Last edit: 20.01.2016 - iSwuerfel
