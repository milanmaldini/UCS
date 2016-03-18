using Sodium;
using System.Collections.Generic;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Packet 20103
    internal class LoginFailedMessage : Message
    {
        private string m_vContentURL;
        private int m_vErrorCode;
        private string m_vReason;
        private string m_vRedirectDomain;
        private int m_vRemainingTime;
        private string m_vResourceFingerprintData;
        private string m_vUpdateURL;

        public LoginFailedMessage(Client client) : base(client)
        {
            SetMessageType(20103);
            SetMessageVersion(3);

            //errorcodes:
            //9: removeredirectdomain
            //8: new game version available (removeupdateurl)
            //7: removeresourcefingerprintdata
            //10: maintenance
            //11: banni temporairement
            //12: played too much
            //13: compte verrouillé
        }

        public override void Encode()
        {
            var pack = new List<byte>();

            pack.AddInt32(m_vErrorCode);
            pack.AddString(m_vResourceFingerprintData);
            pack.AddString(m_vRedirectDomain);
            pack.AddString(m_vContentURL);
            pack.AddString(m_vUpdateURL);
            pack.AddString(m_vReason);
            pack.AddInt32(m_vRemainingTime);
            pack.AddInt32(-1);
            pack.Add(0);

            
            SetData(PublicKeyBox.Create(pack.ToArray(), Client.CNonce, Crypto8.StandardKeyPair.PublicKey, Client.CPublicKey));
        }

        public void RemainingTime(int code)
        {
            m_vRemainingTime = code;
        }

        public void SetContentURL(string url)
        {
            m_vContentURL = url;
        }

        public void SetErrorCode(int code)
        {
            m_vErrorCode = code;
        }

        public void SetReason(string reason)
        {
            m_vReason = reason;
        }

        public void SetRedirectDomain(string domain)
        {
            m_vRedirectDomain = domain;
        }

        public void SetResourceFingerprintData(string data)
        {
            m_vResourceFingerprintData = data;
        }

        public void SetUpdateURL(string url)
        {
            m_vUpdateURL = url;
        }
    }
}