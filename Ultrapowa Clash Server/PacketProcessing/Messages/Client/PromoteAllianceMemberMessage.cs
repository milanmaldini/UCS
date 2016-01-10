using System;
using System.IO;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    //Packet 14316

    internal class PromoteAllianceMemberMessage : Message
    {
        private long m_vId;
        private int m_vRole;

        public PromoteAllianceMemberMessage(Client client, BinaryReader br) : base(client, br)
        {
            //Not sure if there should be something here o.O
        }

        public override void Decode()
        {
            using (var br = new BinaryReader(new MemoryStream(GetData())))
            {
                m_vId = br.ReadInt64WithEndian();
                m_vRole = br.ReadInt32WithEndian();
            }
        }

        public override void Process(Level level)
        {
            Console.WriteLine(m_vId);
            Console.WriteLine(m_vRole);
        }
    }
}