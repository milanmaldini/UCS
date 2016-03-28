using System.Collections.Generic;
using System.IO;
using System.Linq;
using UCS.Core;
using UCS.Helpers;
using UCS.Logic;
using UCS.Network;

namespace UCS.PacketProcessing
{
    internal class TopGlobalAlliancesMessage : Message
    {
        public TopGlobalAlliancesMessage(Client client, BinaryReader br) : base(client, br)
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