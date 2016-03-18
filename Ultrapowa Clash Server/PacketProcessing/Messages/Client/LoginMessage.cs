using Sodium;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Data.HashFunction;
using System.Security.Cryptography;
using System.Text;
using UCS.Core;
using UCS.Helpers;
using UCS.Logic;
using UCS.Network;
using UCS.PacketProcessing;

namespace UCS.PacketProcessing
{
    //Packet 10101
    internal class LoginMessage : Message
    {
        public byte[] SPublicKey;
        public byte[] SPrivateKey;
        public byte[] CPublicKey;
        public byte[] SessionKey;
        public byte[] Nonce;
        public long UserID;
        public string UserToken;
        public int MajorVersion;
        public int ContentVersion;
        public int MinorVersion;
        public string MasterHash;
        public string Unknown1;
        public string OpenUDID;
        public string MacAddress;
        public string DeviceModel;
        public int LocaleKey;
        public string Language;
        public string AdvertisingGUID;
        public string OSVersion;
        public byte Unknown2;
        public string Unknown3;
        public string AndroidDeviceID;
        public string FacebookDistributionID;
        public bool IsAdvertisingTrackingEnabled;
        public string VendorGUID;
        public int Seed;
        public byte Unknown4;
        public string Unknown5;
        public string Unknown6;
        public string ClientVersion;
        public byte[] PlainText;
        


        public LoginMessage(Client client, BinaryReader br) : base(client, br) { }

        public override void Decode()
        {
            
            /* The Packet Data */
            byte[] RawPacket = GetData();

            /* Generating a Key Pair and store the client public key */
            SPrivateKey = Crypto8.StandardKeyPair.PrivateKey;
            SPublicKey = Crypto8.StandardKeyPair.PublicKey;
            CPublicKey = RawPacket.Take(32).ToArray();

            try
            {
                /* Generating Nonce with server public key */
                var ServerNonce = GenericHash.Hash(CPublicKey.Concat(SPublicKey).ToArray(), null, 24);

                /* The full packet content in raw data without the public key of the client */
                var cipherText = RawPacket.Skip(32).ToArray();

                /* Finally, we store the decrypted data, and use the function below for dencryption */
                PlainText = PublicKeyBox.Open(cipherText, ServerNonce, SPrivateKey, CPublicKey).ToArray();
            }
            catch
            {
                Console.WriteLine("[UCS][ERROR] Someone with an old version of clash of clans tried to connected unsuccessfully.");
            }
            
            // ---------------------------------- 


            using (var reader = new CoCSharpPacketReader(new MemoryStream(PlainText)))
            {
                SessionKey = reader.ReadBytes(CoCKeyPair.NonceLength);
                Nonce = reader.ReadBytes(CoCKeyPair.NonceLength);

                UserID = reader.ReadInt64();
                UserToken = reader.ReadString();
                MajorVersion = reader.ReadInt32();
                ContentVersion = reader.ReadInt32();
                MinorVersion = reader.ReadInt32();
                MasterHash = reader.ReadString();

                Unknown1 = reader.ReadString();

                OpenUDID = reader.ReadString();
                MacAddress = reader.ReadString();
                DeviceModel = reader.ReadString();
                LocaleKey = reader.ReadInt32();
                Language = reader.ReadString();
                AdvertisingGUID = reader.ReadString();
                OSVersion = reader.ReadString();

                Unknown2 = reader.ReadByte();
                Unknown3 = reader.ReadString();

                AndroidDeviceID = reader.ReadString();
                FacebookDistributionID = reader.ReadString();
                IsAdvertisingTrackingEnabled = reader.ReadBoolean();
                VendorGUID = reader.ReadString();
                Seed = reader.ReadInt32();

                Unknown4 = reader.ReadByte();
                Unknown5 = reader.ReadString();
                Unknown6 = reader.ReadString();

                ClientVersion = reader.ReadString();
            }



            Console.WriteLine("[UCS]    [10101] Session Key  = " + BitConverter.ToString(SessionKey));
            Console.WriteLine("[UCS]    [10101] Client Nonce = " + BitConverter.ToString(Nonce));
            Console.WriteLine("[UCS]    [10101] User ID      = " + UserID);
            Console.WriteLine("[UCS]    [10101] User Token   = " + UserToken);
            Console.WriteLine("[UCS]    [10101] MajorVersion = " + MajorVersion);
            Console.WriteLine("[UCS]    [10101] ContVersion  = " + ContentVersion);
            Console.WriteLine("[UCS]    [10101] MinorVersion = " + MinorVersion);
            Console.WriteLine("[UCS]    [10101] MasterHash   = " + MasterHash);
            Console.WriteLine("[UCS]    [10101] OpenUDID     = " + OpenUDID);
            Console.WriteLine("[UCS]    [10101] OS Version   = " + OSVersion);
            Console.WriteLine("[UCS]    [10101] ClientV      = " + ClientVersion);
            Console.WriteLine("[UCS]    [10101] Language     = " + Language);
            Console.WriteLine("[UCS]    [10101] ADeviceID    = " + AndroidDeviceID);
            Console.WriteLine("[UCS]    [10101] Device Model = " + DeviceModel);
        }

        public override void Process(Level level)
        {
            /* Storing Data about Client Crypto */

            Client.CPublicKey = CPublicKey;
            Client.CSessionKey = SessionKey;
            Client.CState = 1;
            Client.CNonce = Nonce;
            
            /* END */

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
                var cv = ClientVersion.Split('.');
                if (cv[0] != versionData[0] || cv[1] != versionData[1])
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

            level = ResourcesManager.GetPlayer(UserID);
            if (level != null)
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
                level = ObjectManager.CreateAvatar(UserID);
                var tokenSeed = new byte[20];
                new Random().NextBytes(tokenSeed);
                using (SHA1 sha = new SHA1CryptoServiceProvider())
                {
                    UserToken = BitConverter.ToString(sha.ComputeHash(tokenSeed)).Replace("-", "");
                }
            }

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["useCustomPatch"]))
            {
                if (MasterHash != ObjectManager.FingerPrint.sha)
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

            Client.ClientSeed = Seed;
            //PacketManager.ProcessOutgoingPacket(new SessionKeyMessage(Client));

            if (level.GetAccountPrivileges() > 0)
                level.GetPlayerAvatar().SetLeagueId(21);
            if (level.GetAccountPrivileges() > 4)
                level.GetPlayerAvatar().SetLeagueId(22);
            ResourcesManager.LogPlayerIn(level, Client);
            level.Tick();

            var loginOk = new LoginOkMessage(Client);
            /* SETTING NONCE */
            loginOk.SetNonce(GenericHash.Hash(Nonce.Concat(CPublicKey).Concat(SPublicKey).ToArray(), null, 24));
            /* END OF CRYPTO */
            var avatar = level.GetPlayerAvatar();
            loginOk.SetAccountId(avatar.GetId());
            loginOk.SetPassToken(UserToken);
            loginOk.SetServerMajorVersion(MajorVersion);
            loginOk.SetServerBuild(MinorVersion);
            loginOk.SetContentVersion(ContentVersion);
            loginOk.SetServerEnvironment("prod");
            loginOk.SetDaysSinceStartedPlaying(10);
            loginOk.SetServerTime(Math.Round(level.GetTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds * 1000).ToString());
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