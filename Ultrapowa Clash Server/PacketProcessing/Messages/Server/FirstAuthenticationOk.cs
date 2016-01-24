using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Packet 20100
    class FirstAuthenticationOk : Message
    {
        public FirstAuthenticationOk(Client client, FirstAuthentication cka)
            : base(client)
        {
            SetMessageType(20100);
        }

        public static byte[] randomBytes = new byte[24];
        public override void Encode()
        {
            // Randombytes()
            Sodium.SodiumLibrary.randombytes_buff(randomBytes, 24);
            List<Byte> pack = new List<Byte>();
            pack.AddInt32(randomBytes[7]);  
            pack.AddInt32(randomBytes[14]);
            pack.AddInt32(randomBytes[21]);
            SetData(pack.ToArray());
        }
    }
}

