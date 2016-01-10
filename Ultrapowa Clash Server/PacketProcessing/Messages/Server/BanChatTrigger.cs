using System.Collections.Generic;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Packet 20118
    internal class BanChatTrigger : Message
    {
        private int m_vCode;

        public BanChatTrigger(Client client)
            : base(client)
        {
            SetMessageType(20118);
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