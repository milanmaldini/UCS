using Sodium;
using System;
using System.Collections.Generic;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Packet 20104
    internal class LoginOkMessage : Message
    {
        private string m_vAccountCreatedDate;
        private long m_vAccountId;
        private int m_vContentVersion;
        private string m_vCountryCode;
        private int m_vDaysSinceStartedPlaying;
        private string m_vFacebookId;
        private string m_vGamecenterId;
        private string m_vPassToken;
        private int m_vPlayTimeSeconds;
        private int m_vServerBuild;
        private string m_vServerEnvironment;
        private int m_vServerMajorVersion;
        private string m_vServerTime;
        private int m_vSessionCount;
        private int m_vStartupCooldownSeconds;
        private byte[] m_vClientKey;
        private byte[] m_vClientNonce;
        private int m_vFacebookAppID;
        private int m_vLastUpdate;
        private int m_vGoogleID;

        public LoginOkMessage(Client client) : base(client)
        {
            SetMessageType(20104);
            SetMessageVersion(1);
            Unknown11 = "someid2";
            m_vClientKey = client.CPublicKey;
            m_vClientNonce = client.CNonce;
        }

        public string Unknown11 { get; set; }

        public string Unknown9 { get; set; }

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddRange(m_vClientKey);
            pack.AddRange(m_vClientNonce);
            pack.AddInt64(m_vAccountId);
            pack.AddInt64(m_vAccountId);
            pack.AddString(m_vPassToken);
            pack.AddString(m_vFacebookId);
            pack.AddString(m_vGamecenterId);
            pack.AddInt32(m_vServerMajorVersion);
            pack.AddInt32(m_vServerBuild);
            pack.AddInt32(m_vContentVersion);
            pack.AddString(m_vServerEnvironment);
            //pack.AddInt32(m_vDaysSinceStartedPlaying);
            pack.AddInt32(m_vSessionCount);
            pack.AddInt32(m_vPlayTimeSeconds);
            pack.AddString("someid1");
            //pack.AddString(m_vServerTime);
            pack.AddInt32(m_vFacebookAppID);
            pack.AddInt32(m_vStartupCooldownSeconds);
            pack.AddString(m_vAccountCreatedDate);
            pack.AddString("someid2");
            pack.AddInt32(m_vGoogleID);
            pack.AddString(m_vCountryCode);
            pack.AddInt32(1);
            var packet = pack.ToArray();
            packet = PublicKeyBox.Create(packet, m_vClientNonce, Crypto8.StandardKeyPair.PublicKey, m_vClientKey);
            SetData(packet);
        }

        public void SetAccountCreatedDate(string date)
        {
            m_vAccountCreatedDate = date;
        }

        public void SetAccountId(long id)
        {
            m_vAccountId = id;
        }

        public void SetContentVersion(int version)
        {
            m_vContentVersion = version;
        }

        public void SetCountryCode(string code)
        {
            m_vCountryCode = code;
        }

        public void SetDaysSinceStartedPlaying(int days)
        {
            m_vDaysSinceStartedPlaying = days;
        }

        public void SetFacebookId(string id)
        {
            m_vFacebookId = id;
        }

        public void SetGamecenterId(string id)
        {
            m_vGamecenterId = id;
        }

        public void SetPassToken(string token)
        {
            m_vPassToken = token;
        }

        public void SetPlayTimeSeconds(int seconds)
        {
            m_vPlayTimeSeconds = seconds;
        }

        public void SetServerBuild(int build)
        {
            m_vServerBuild = build;
        }

        public void SetServerEnvironment(string env)
        {
            m_vServerEnvironment = env;
        }

        public void SetServerMajorVersion(int version)
        {
            m_vServerMajorVersion = version;
        }

        public void SetServerTime(string time)
        {
            m_vServerTime = time;
        }

        public void SetSessionCount(int count)
        {
            m_vSessionCount = count;
        }

        public void SetStartupCooldownSeconds(int seconds)
        {
            m_vStartupCooldownSeconds = seconds;
        }
    }
}