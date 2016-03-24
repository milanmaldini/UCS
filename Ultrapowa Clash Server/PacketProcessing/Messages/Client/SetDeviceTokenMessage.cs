using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace UCS.PacketProcessing
{
    internal class SetDeviceTokenMessage : Message
    {
        private byte[] m_vPacket;

        public SetDeviceTokenMessage(Client client, BinaryReader br) : base(client, br)
        {
            Decrypt8();
            m_vPacket = GetData();
        }

        public override void Decode()
        {
            File.WriteAllBytes(Directory.GetCurrentDirectory() + "/packet10113.raw", m_vPacket);
        }
    }
}