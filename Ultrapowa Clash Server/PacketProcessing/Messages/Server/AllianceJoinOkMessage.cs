using System.Collections.Generic;

namespace UCS.PacketProcessing
{
    //Packet 24303
    class AllianceJoinOkMessage : Message
    {
        public AllianceJoinOkMessage(Client client) : base(client)
        {
            SetMessageType(24303);
        }

        public override void Encode()
        {
            var pack = new List<byte>();
            SetData(pack.ToArray());
        }
    }
}