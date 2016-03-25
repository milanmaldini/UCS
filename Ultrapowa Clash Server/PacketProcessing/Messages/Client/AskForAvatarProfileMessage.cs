using System.IO;
using UCS.Core;
using UCS.Helpers;
using UCS.Logic;
using UCS.Network;

namespace UCS.PacketProcessing
{
    //14325
    internal class AskForAvatarProfileMessage : Message
    {
        private long m_vAvatarId;
        private long m_vCurrentHomeId;

        public AskForAvatarProfileMessage(Client client, BinaryReader br) : base(client, br)
        {
            Decrypt8();
        }

        public override void Decode()
        {
            using (var br = new BinaryReader(new MemoryStream(GetData())))
            {
                m_vAvatarId = br.ReadInt64WithEndian();
                m_vCurrentHomeId = br.ReadInt64WithEndian();
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
            var targetLevel = ResourcesManager.GetPlayer(m_vAvatarId);
            if (targetLevel != null)
            {
                targetLevel.Tick();
                var p = new AvatarProfileMessage(Client);
                p.SetLevel(targetLevel);
                PacketManager.ProcessOutgoingPacket(p);
            }
            */
        }
    }
}