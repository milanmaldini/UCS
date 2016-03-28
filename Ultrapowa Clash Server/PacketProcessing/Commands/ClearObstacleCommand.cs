using System.IO;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Commande 0x1FB 507
    internal class ClearObstacleCommand : Command
    {
        public ClearObstacleCommand(BinaryReader br)
        {
            /*
            ObstacleId = br.ReadUInt32WithEndian(); //ObstacleId - 0x1DFB2BC0;
            Unknown1 = br.ReadUInt32WithEndian();
            */
        }

        public uint ObstacleId { get; set; }
        public uint Unknown1 { get; set; }
    }
}