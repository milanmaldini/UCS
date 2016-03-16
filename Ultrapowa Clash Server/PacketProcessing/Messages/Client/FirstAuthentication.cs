using System.IO;
using UCS.Helpers;
using UCS.Logic;
using UCS.Network;
using System.Text;
using System;

namespace UCS.PacketProcessing
{
    internal class FirstAuthentication : Message
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

        public FirstAuthentication(Client client, BinaryReader br) : base(client, br) { }
        
        public override void Decode()
        {
            using (var reader = new CoCSharpPacketReader(new MemoryStream(GetRawData())))
            {
                Unknown1 = reader.ReadInt32();
                Unknown2 = reader.ReadInt32();
                MajorVersion = reader.ReadInt32();
                Unknown4 = reader.ReadInt32();
                MinorVersion = reader.ReadInt32();
                Hash = reader.ReadString();
                Unknown6 = reader.ReadInt32();
                Unknown7 = reader.ReadInt32();

                Console.WriteLine("[UCS]    [10100]    Unknown1 ->    " + Unknown1);
                Console.WriteLine("[UCS]    [10100]    Unknown2 ->    " + Unknown2);
                Console.WriteLine("[UCS]    [10100]    MVersion ->    " + MajorVersion);
                Console.WriteLine("[UCS]    [10100]    MVersion ->    " + MinorVersion);
                Console.WriteLine("[UCS]    [10100]    Unknown4 ->    " + Unknown4);
                Console.WriteLine("[UCS]    [10100]    Hash     ->    " + Hash);
                Console.WriteLine("[UCS]    [10100]    Unknown6     ->    " + Unknown6);
                Console.WriteLine("[UCS]    [10100]    Unknown7     ->    " + Unknown7);
            }
        }

        public override void Process(Level level)
        {
            var authOk = new FirstAuthenticationOk(Client, this);
            PacketManager.ProcessOutgoingPacket(authOk);
        }
    }
}