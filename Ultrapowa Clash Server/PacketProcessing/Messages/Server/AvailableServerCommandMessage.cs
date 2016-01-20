using System.Collections.Generic;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Packet 24111
    class AvailableServerCommandMessage : Message
    {
        private Command m_vCommand;
        private int m_vServerCommandId;

        public AvailableServerCommandMessage(Client client) : base(client)
        {
            SetMessageType(24111);
        }

        public void SetCommandId(int id)
        {
            m_vServerCommandId = id;
        }

        public void SetCommand(Command c)
        {
            m_vCommand = c;
        }

        //00 00 00 01 00 00 00 26 00 20 FE BA 00 00 00 07 4B 61 6E 61 62 69 73 5B 00 1A 5A 00 00 00 00 03 00 00 00 03 FF FF FF FF
        public override void Encode()
        {
            var pack = new List<byte>();

            pack.AddInt32(m_vServerCommandId);
            pack.AddRange(m_vCommand.Encode());

            SetData(pack.ToArray());
        }
    }
}