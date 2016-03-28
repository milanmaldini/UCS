using System.IO;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Commande 0x20E
    internal class BoostBuildingCommand : Command
    {
        public BoostBuildingCommand(BinaryReader br)
        {
            /*
            BuildingId = br.ReadUInt32WithEndian(); //buildingId - 0x1DCD6500;
            Unknown1 = br.ReadUInt32WithEndian();
            */
        }

        public uint BuildingId { get; set; }
        public uint Unknown1 { get; set; }
    }
}