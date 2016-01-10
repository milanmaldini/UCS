using System.Collections.Generic;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    internal class Command
    {
        public virtual byte[] Encode()
        {
            return new List<byte>().ToArray();
        }

        public virtual void Execute(Level level)
        {

        }
    }
}