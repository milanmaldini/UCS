using System.Collections.Generic;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    class Command
    {
        public virtual void Execute(Level level)
        {
        }

        public virtual byte[] Encode()
        {
            return new List<byte>().ToArray();
        }
    }
}