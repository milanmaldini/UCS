using System.IO;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Commande 0x20E
    class BoostBuildingCommand : Command
    {
        public BoostBuildingCommand(BinaryReader br)
        {
            BuildingId = br.ReadUInt32WithEndian(); //buildingId - 0x1DCD6500;
            Unknown1 = br.ReadUInt32WithEndian();
        }

        //00 00 02 0E 1D CD 65 05 00 00 8C 52

        public uint BuildingId { get; set; }
        public uint Unknown1 { get; set; }
    }
}