using Ionic.Zlib;
using System.Collections.Generic;
using System.IO;
using UCS.Helpers;

namespace UCS.Logic
{
    internal class ClientHome : Base
    {
        private readonly long m_vId;
        private int m_vRemainingShieldTime;
        private byte[] m_vSerializedVillage;
        private int m_vHomeLenght;

        public ClientHome() : base(0) { }

        public ClientHome(long id) : base(0)
        {
            m_vId = id;
        }

        public override byte[] Encode()
        {
            var data = new List<byte>();

            data.AddRange(base.Encode());
            data.AddInt32(0);
            data.AddInt64(m_vId);
            data.AddInt32(m_vRemainingShieldTime);
            data.AddInt32(1800);
            data.AddInt32(69119);
            data.AddInt32(1200);
            data.AddInt32(60);
            data.Add(1);


            data.AddInt32(m_vHomeLenght + 4);
            data.AddRange(m_vSerializedVillage);

            data.AddInt32(0);

            return data.ToArray();
        }

        public byte[] GetHomeJSON()
        {
            return m_vSerializedVillage;
        }

        public void SetHomeJSON(string json)
        {
            m_vSerializedVillage = ZlibStream.CompressString(json);
            m_vHomeLenght = json.Length;
        }

        public void SetShieldDurationSeconds(int seconds)
        {
            m_vRemainingShieldTime = seconds;
        }
    }
}