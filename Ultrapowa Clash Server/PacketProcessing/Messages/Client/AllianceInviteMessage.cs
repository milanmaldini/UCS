using System.IO;
using UCS.Helpers;
using UCS.Logic;
using UCS.Network;

namespace UCS.PacketProcessing
{
    internal class AllianceInviteMessage : Message
    {
        public AllianceInviteMessage(Client client, BinaryReader br) : base(client, br)
        {
            Decrypt8();
        }


        public override void Decode()
        {

        }

        public override void Process(Level level)
        {

        }
    }
}