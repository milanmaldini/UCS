using System.Collections.Generic;
using System.IO;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Packet 20100
    internal class FirstAuthenticationOk : Message
    {
        public FirstAuthenticationOk(Client client, FirstAuthentication cka) : base(client)
        {
            SetMessageType(20100);
        }

        public byte[] SessionKey;

        public override void Decode()
        {
            using (var reader = new CoCSharpPacketReader(new MemoryStream(GetData())))
            {
                SessionKey = reader.ReadAllBytes();
            }
        }

        public override void Encode()
        {
            var pack = new List<byte>();
            pack.AddInt32(0);
            pack.AddInt32(0);
            pack.AddInt32(0);
            SetData(pack.ToArray());
        }
    }
}