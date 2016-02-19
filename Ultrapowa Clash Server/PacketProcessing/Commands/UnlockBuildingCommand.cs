using System.IO;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    //Commande 0x208
    internal class UnlockBuildingCommand : Command
    {
        public UnlockBuildingCommand(BinaryReader br)
        {
            BuildingId = br.ReadUInt32WithEndian(); //buildingId - 0x1DCD6500;
            Unknown1 = br.ReadUInt32WithEndian();
        }

        public uint BuildingId { get; set; }
        public uint Unknown1 { get; set; }

        public override void Execute(Level l)
        {
        }
    }
}