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
            
            data.AddInt32(0);
            data.AddInt32(-1);
            data.AddInt32((int) Player.GetTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            data.AddInt32(0);
            data.AddInt64(Avatar.GetId());
            data.AddInt32(Avatar.RemainingShieldTime);
            data.AddInt32(1800);
            data.AddInt32(69119);
            data.AddInt32(1200);
            data.AddInt32(60);
            data.AddString("true");

            using (var bw = new BinaryWriter(new MemoryStream()))
            {
                var Home = Player.SaveToJSON();
                var CHome = ZlibStream.CompressString(Home);
                
                bw.Write(Home.Length);
                bw.Write(CHome);

                data.AddRange(((MemoryStream)bw.BaseStream).ToArray());
            }

            data.AddInt32(0);
            
            var packet = data.ToArray();
            
            Client.CNonce = Utilities.Increment(Client.CNonce);

            packet = PublicKeyBox.Create(packet, Client.CNonce, Crypto8.StandardKeyPair.PrivateKey, m_vClientKey);
            SetData(packet);
        }
    }
}