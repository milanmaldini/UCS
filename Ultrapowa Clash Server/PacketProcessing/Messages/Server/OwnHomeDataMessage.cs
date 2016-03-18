using System;
using System.Collections.Generic;
using System.Text;
using UCS.Helpers;
using UCS.Logic;
using Ionic.Zlib;
using System.IO;
using Sodium;

namespace UCS.PacketProcessing
{
    //Packet 24101
    internal class OwnHomeDataMessage : Message
    {
        public OwnHomeDataMessage(Client client, Level level) : base(client)
        {
            SetMessageType(24101);
            Player = level;
            m_vClientNonce = client.CNonce;
            m_vClientKey = client.CPublicKey;
        }

        public Level Player { get; set; }
        private byte[] m_vSerializedVillage { get; set; }

        private byte[] m_vClientNonce;

        private byte[] m_vClientKey;

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




            
            var packet = data.ToArray();
            Console.WriteLine(Encoding.UTF8.GetString(packet));
            Client.CNonce = Utilities.Increment(Client.CNonce);
            packet = PublicKeyBox.Create(packet, Client.CNonce, Crypto8.StandardKeyPair.PublicKey, m_vClientKey);
            SetData(packet);
        }
    }
}