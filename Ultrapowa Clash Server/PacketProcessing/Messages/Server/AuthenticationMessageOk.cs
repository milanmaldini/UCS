using System;
using System.Collections.Generic;
using UCS.Helpers;

namespace UCS.PacketProcessing.Messages.Server
{
    class AuthenticationMessageOk : Message
    {
        public AuthenticationMessageOk(Client client) : base(client)
        {
            SetMessageType(20100);
            Key8 = new byte[] { 0xD7, 0x16, 0x28, 0xA8, 0x08, 0xD2, 0x3C, 0x1D, 0xD9, 0x26, 0xA4, 0xB2, 0x1C, 0xB4, 0xB9, 0x32 };
        }
        public override void Encode()
        {
            var data = new List<Byte>();
            data.AddInt32(Key8.Length);
            data.AddRange(Key8);
            data.AddInt32(1);
            SetData(data.ToArray());
        }
        public byte[] Key8 { get; set; }
    }
}
