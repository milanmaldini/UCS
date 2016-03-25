using System.IO;
using UCS.Core;
using UCS.Helpers;
using UCS.Logic;
using UCS.Network;

namespace UCS.PacketProcessing
{
    //Packet 14113
    internal class VisitHomeMessage : Message
    {
        public VisitHomeMessage(Client client, BinaryReader br)
            : base(client, br)
        {
            Decrypt8();
        }

        public long AvatarId { get; set; }

        public override void Decode()
        {
            using (var br = new BinaryReader(new MemoryStream(GetData())))
            {
                AvatarId = br.ReadInt64WithEndian();
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
            var targetLevel = ResourcesManager.GetPlayer(AvatarId);
            targetLevel.Tick();
            //Clan clan;
            PacketManager.ProcessOutgoingPacket(new VisitedHomeDataMessage(Client, targetLevel, level));
            //if (clan != null)
            //    PacketHandler.ProcessOutgoingPacket(new ServerAllianceChatHistory(this.Client, clan));
            */
        }
    }
}