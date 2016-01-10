using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Logic;
using UCS.Helpers;

namespace UCS.PacketProcessing

{
    class LeagueMemberListCommand : Message
    {

        public LeagueMemberListCommand(Client client, Alliance alliance)
            : base(client)
        {
        }

       
        public override void Encode()
        {
            List<Byte> pack = new List<Byte>();
            pack.AddInt64(0);
            pack.AddString("Aidid");
            pack.AddInt32(21);
            pack.AddInt32(4000);
            pack.AddInt32(20);
            pack.AddInt32(2000);
            pack.AddInt32(32);

            SetData(pack.ToArray());
        }
    }
}