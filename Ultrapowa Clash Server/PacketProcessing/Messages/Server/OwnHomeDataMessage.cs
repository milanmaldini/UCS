using System;
using System.Collections.Generic;
using System.Text;
using UCS.Helpers;
using UCS.Logic;
using Ionic.Zlib;
using System.IO;
using Sodium;
using System.Linq;

namespace UCS.PacketProcessing
{
    //Packet 24101
    internal class OwnHomeDataMessage : Message
    {
        public OwnHomeDataMessage(Client client, Level level) : base(client)
        {
            SetMessageType(24101);
            Player = level;
        }

        public Level Player { get; set; }

        public override void Encode()
        {
            var Avatar = Player.GetPlayerAvatar();

            var data = new List<byte>();

            var home = new ClientHome(Avatar.GetId());

            home.SetShieldDurationSeconds(Avatar.RemainingShieldTime);

            home.SetHomeJSON(Player.SaveToJSON());

            data.AddInt32(0);

            data.AddInt32(-1);

            data.AddInt32((int)Player.GetTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds);

            data.AddRange(home.Encode());

            data.AddRange(Avatar.Encode());

            Encrypt8(data.ToArray());

        }
    }
}