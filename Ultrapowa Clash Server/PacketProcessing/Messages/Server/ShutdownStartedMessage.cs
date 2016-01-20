using System.Collections.Generic;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Packet 20161
    class ShutdownStartedMessage : Message
    {
        private int m_vCode;

        public ShutdownStartedMessage(Client client)
            : base(client)
        {
            SetMessageType(20161);
        }

        public override void Encode()
        {
            var data = new List<byte>();
            data.AddInt32(m_vCode);
            SetData(data.ToArray());
        }

        public void SetCode(int code)
        {
            m_vCode = code;
        }
    }
}