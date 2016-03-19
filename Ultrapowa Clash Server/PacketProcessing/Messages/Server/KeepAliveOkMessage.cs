using Sodium;
using System.Collections.Generic;
using System.Linq;

namespace UCS.PacketProcessing
{
    //Packet 20108
    internal class KeepAliveOkMessage : Message
    {
        public KeepAliveOkMessage(Client client, KeepAliveMessage cka) : base(client)
        {
            SetMessageType(20108);
        }

        public override void Encode()
        {
            var data = new List<byte>();
            var packet = data.ToArray();
            Client.CRNonce = Utilities.Increment(Utilities.Increment(Client.CRNonce));
            SetData(SecretBox.Create(packet, Client.CSNonce, Client.CSharedKey).Skip(16).ToArray());
        }
    }
}