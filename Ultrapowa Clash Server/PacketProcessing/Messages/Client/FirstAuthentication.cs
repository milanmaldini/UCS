using System.IO;
using UCS.Helpers;
using UCS.Logic;
using UCS.Network;

namespace UCS.PacketProcessing
{
    internal class FirstAuthentication : Message
    {
        //Packet 10100

        public int Data1;

        public int Data2;

        public int Data4;

        public int Data5;

        public string MasterHash;

        public byte[] SomeData;

        public FirstAuthentication(Client client, BinaryReader br)
            : base(client, br)
        {
        }

        /// <summary>
        ///     Data probably (!) not needed for encryption
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
            var authOk = new FirstAuthenticationOk(Client, this);
            PacketManager.ProcessOutgoingPacket(authOk);
        }
    }
}