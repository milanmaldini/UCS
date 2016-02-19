using System.Collections.Generic;
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