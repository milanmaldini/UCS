using System.IO;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Commande 0x0214
    class NewShopItemsSeenCommand : Command
    {
        public NewShopItemsSeenCommand(BinaryReader br)
        {
            var NewShopItemNumber = br.ReadUInt32WithEndian();
            var Unknown1 = br.ReadUInt32WithEndian();
            var Unknown2 = br.ReadUInt32WithEndian();
            var Unknown3 = br.ReadUInt32WithEndian();
        }

        //00 00 02 14 00 00 00 00 00 00 00 00 00 00 00 00 00 01 02 27
        //00 00 02 14 00 00 00 06 00 00 00 00 00 00 00 00 00 01 02 27

        public uint NewShopItemNumber { get; set; }
        public uint Unknown1 { get; set; }
        public uint Unknown2 { get; set; }
        public uint Unknown3 { get; set; }
    }
}