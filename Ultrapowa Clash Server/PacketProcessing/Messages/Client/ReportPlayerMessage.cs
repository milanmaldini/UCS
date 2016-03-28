using System;
using System.IO;
using System.Text;
using UCS.Helpers;
using UCS.Logic;
using UCS.Network;

namespace UCS.PacketProcessing
{
    internal class ReportPlayerMessage : Message
    {
        public ReportPlayerMessage(Client client, BinaryReader br) : base(client, br)
        {
            Decrypt8();
        }
        
        public override void Decode()
        {
            using (var br = new BinaryReader(new MemoryStream(GetData())))
            {
                Console.WriteLine(br.ReadInt32());
            }
        }

        public override void Process(Level level)
        {

        }
    }
}