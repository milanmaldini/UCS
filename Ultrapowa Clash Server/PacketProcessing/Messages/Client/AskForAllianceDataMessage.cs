using System.IO;
using UCS.Core;
using UCS.Helpers;
using UCS.Logic;
using UCS.Network;

namespace UCS.PacketProcessing
{
    //14302
    internal class AskForAllianceDataMessage : Message
    {
        private long m_vAllianceId;

        public AskForAllianceDataMessage(Client client, BinaryReader br) : base(client, br)
        {
            Decrypt8();
        }

        public override void Decode()
        {
            using (var br = new BinaryReader(new MemoryStream(GetData())))
            {
                m_vAllianceId = br.ReadInt64WithEndian();
            }
        }

        public override void Process(Level level)
        {
            PacketManager.ProcessOutgoingPacket(new OwnHomeDataMessage(level.GetClient(), level));
            var p = new GlobalChatLineMessage(level.GetClient());
            p.SetChatMessage("Not implemented");
            p.SetPlayerId(0);
            p.SetPlayerName("Ultrapowa Clash Server");
            PacketManager.ProcessOutgoingPacket(p);
            /*
            var alliance = ObjectManager.GetAlliance(m_vAllianceId);
            if (alliance != null)
            {
                PacketManager.ProcessOutgoingPacket(new AllianceDataMessage(Client, alliance));
            }
            */
        }
    }
}