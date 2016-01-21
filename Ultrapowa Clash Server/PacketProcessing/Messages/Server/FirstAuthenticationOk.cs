using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UCS.Helpers;
namespace UCS.PacketProcessing.Messages.Server
{
    class FirstAuthenticationOk : Message
    {
        public FirstAuthenticationOk(Client client)
            : base(client)
        {
            SetMessageType(20100);
        }

        byte[] packData = new byte[]
{
    0x4E, 0x84, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x18, 0x40, 0xBA, 0x72, 0x10, 0x10, 
    0xE7, 0x0E, 0x03, 0xB7, 0xF2, 0x5A, 0xD4, 0x92, 0x3A, 0xEF, 0xC3, 0x13, 0xC4, 0x1C, 0x3A, 0xA2, 
    0x55, 0xC8, 0xFF, 
};
        public override void Encode()
        {
            List<Byte> data = new List<Byte>();
            data.AddInt32(0);
            data.AddInt32(0);
            data.AddInt32(0);
            SetData(data.ToArray());
        }

    }
}
