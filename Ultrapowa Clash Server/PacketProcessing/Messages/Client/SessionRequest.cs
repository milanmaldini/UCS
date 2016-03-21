using System.IO;
using UCS.Helpers;
using UCS.Logic;
using UCS.Network;
using System.Text;
using System;

namespace UCS.PacketProcessing
{
    internal class SessionRequest : Message
    {
        //Packet 10100
        
        public int Unknown1;
        public int Unknown2;
        public int MajorVersion;
        public int Unknown4;
        public int MinorVersion;
        public string Hash;
        public int Unknown6;
        public int Unknown7;

        public SessionRequest(Client client, BinaryReader br) : base(client, br) { }
        
        public override void Decode()
        {
            using (var reader = new CoCSharpPacketReader(new MemoryStream(GetData())))
            {
               Console.WriteLine(Encoding.UTF8.GetString(GetRawData()));
                
                Unknown1 = reader.ReadInt32();
                Unknown2 = reader.ReadInt32();
                MajorVersion = reader.ReadInt32();
                Unknown4 = reader.ReadInt32();
                MinorVersion = reader.ReadInt32();
                Hash = reader.ReadString();
                Unknown6 = reader.ReadInt32();
                Unknown7 = reader.ReadInt32();
                
            }
        }

        public override void Process(Level level)
        {
            if (MajorVersion != 8)
                return;
            var authOk = new SessionSuccess(Client, this);
            PacketManager.ProcessOutgoingPacket(authOk);
        }
    }
}