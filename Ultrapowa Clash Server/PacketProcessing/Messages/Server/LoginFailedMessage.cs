using Newtonsoft.Json;
using Sodium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            SetMessageVersion(10);
            //SetReason("UCS Developement Team <3");
            client.CState = 2;
            
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
            Console.WriteLine("[UCS][20103] Client Key     -> " + Encoding.UTF8.GetString(Client.CPublicKey));
            Console.WriteLine("[UCS][20103] Client Nonce   -> " + Encoding.UTF8.GetString(Client.CSNonce));
            Console.WriteLine("[UCS][20103] Client Shared  -> " + Encoding.UTF8.GetString(Client.CSharedKey));
            Console.WriteLine("[UCS][20103] Client Session -> " + Encoding.UTF8.GetString(Client.CSessionKey));
            Console.WriteLine("[UCS][20103] Client State   -> " + Client.CState);

            
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
            pack.AddString("");


            var packet = pack.ToArray();
            packet = Client.CSNonce.Concat(Client.CPublicKey).Concat(packet).ToArray();
            byte[] nonce = GenericHash.Hash(Client.CSNonce.Concat(Client.CPublicKey).Concat(Crypto8.StandardKeyPair.PublicKey).ToArray(), null, 24);
            SetData(PublicKeyBox.Create(packet, nonce, Crypto8.StandardKeyPair.PrivateKey, Client.CPublicKey));
            
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