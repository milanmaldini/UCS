﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UCS.Logic;
using UCS.Helpers;
using UCS.Network;

namespace UCS.PacketProcessing
{
    class FirstAuthentication : Message
    {
        //Packet 10100

        public FirstAuthentication(Client client, BinaryReader br)
            : base(client, br)
        {
        }

        public int Data1;
        public int Data2;
        public byte[] SomeData;
        public string MasterHash;
        public int Data4;
        public int Data5;
        /// <summary>
        /// Data probably (!) not needed for encryption
        /// </summary>
        public override void Decode()
        {
            using (var reader = new CoCSharpPacketReader(new MemoryStream(GetData())))
            {
                Data1 = reader.ReadInt32();
                Data2 = reader.ReadInt32();
                SomeData = reader.ReadByteArray();
                MasterHash = reader.ReadString();
                Data4 = reader.ReadInt32();
                Data5 = reader.ReadInt32();
            }
        }

        public override void Process(Level level)
        {
            var authOk = new FirstAuthenticationOk(this.Client, this);
            PacketManager.ProcessOutgoingPacket(authOk);
        }

    }
}