using System.Collections.Generic;
using UCS.Core;
using UCS.Logic;
using UCS.Network;

namespace UCS
{
    internal class ProgramThread
    {
        private readonly MessageManager _mm;
        private readonly PacketManager _pm;
        private List<Level> _list;

        public bool m_vRunning = false;

        public ProgramThread()
        {
            //rm = new ResourcesManager();
            //om = new ObjectManager();
            _pm = new PacketManager();
            _mm = new MessageManager();
        }

        public ProgramThread(List<Level> list)
        {
            this._list = list;
        }

        public void Start()
        {
            _pm.Start();
            _mm.Start();
        }

        public void Stop()
        {
            _pm.Stop();
            _mm.Stop();
        }
    }
}