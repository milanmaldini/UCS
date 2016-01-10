using System.IO;
using UCS.Logic;
using UCS.Network;

namespace UCS.PacketProcessing
{
    //14302
    internal class AskForPlayerLeagueList : Message
    {
        public AskForPlayerLeagueList(Client client, BinaryReader br) : base(client, br)
        {

        }

        public override void Decode()
        {

        }

        public override void Process(Level level)
        {
            PacketManager.ProcessOutgoingPacket(new LeagueListPlayer(Client));
        }
    }
}