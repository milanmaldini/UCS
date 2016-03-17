using System.Collections.Generic;
using System.IO;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Packet 20100
    internal class FirstAuthenticationOk : Message
    {
        public byte[] SessionKey;

        public FirstAuthenticationOk(Client client, FirstAuthentication cka) : base(client)
        {
            SetMessageType(20100);
            SessionKey = Crypto8.GenerateNonce();
        }
        
        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddInt32(SessionKey.Length);
            pack.AddRange(SessionKey);
            SetData(pack.ToArray());
        }
    }
}