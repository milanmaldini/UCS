using System.IO;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Commande 0x20A
    internal class BuyShieldCommand : Command
    {
        public BuyShieldCommand(BinaryReader br)
        {
            /*
            ShieldId = br.ReadUInt32WithEndian(); //= shieldId - 0x01312D00;
            Unknown1 = br.ReadUInt32WithEndian();
            */
        }

        public uint ShieldId { get; set; }
        public uint Unknown1 { get; set; }
    }
}