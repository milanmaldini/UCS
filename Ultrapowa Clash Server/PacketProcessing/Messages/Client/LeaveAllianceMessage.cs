using System.IO;
using UCS.Core;
using UCS.Logic;
using UCS.Network;

namespace UCS.PacketProcessing
{
    //Packet 14308
    class LeaveAllianceMessage : Message
    {
        public LeaveAllianceMessage(Client client, BinaryReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
        }

        public override void Process(Level level)
        {
            var alliance = ObjectManager.GetAlliance(level.GetPlayerAvatar().GetAllianceId());
            level.GetPlayerAvatar().SetAllianceId(0);
            alliance.RemoveMember(level.GetPlayerAvatar().GetId());
            //envoyer message départ à tous les membres
            //si chef nommer un nouveau chef
            //if alliance member count = 0, supprimer alliance
            PacketManager.ProcessOutgoingPacket(new LeaveAllianceOkMessage(Client, alliance));
        }
    }
}