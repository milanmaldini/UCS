using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UCS.Logic;
using UCS.Helpers;
using UCS.Network;
using UCS.PacketProcessing.Messages.Server;
namespace UCS.PacketProcessing
{
    class FirstAuthentication : Message
    {
        public int Data1;
        public int Data2;
        public byte[] SomeData;
        public string MasterHash;
        public int Data4;
        public int Data5;

        public FirstAuthentication(Client client, BinaryReader br) : base(client, br)
        {
        }


        public override void Decode()
        {
          
        }


        public override void Process(Level level)
        {
            PacketManager.ProcessOutgoingPacket(new FirstAuthenticationOk(this.Client));
        }
       
    }
}
