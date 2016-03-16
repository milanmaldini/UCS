using System;
using System.Collections.Generic;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    internal class Command
    {
        internal int Depth { get; set; }
        public const int MaxEmbeddedDepth = 10;
        internal void ThrowIfReaderNull(MessageReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
        }
        public virtual byte[] Encode()
        {
            return new List<byte>().ToArray();
        }

        public virtual void Execute(Level level)
        {
        }
    }
}