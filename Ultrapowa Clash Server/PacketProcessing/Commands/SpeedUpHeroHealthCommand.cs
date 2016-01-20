using System.IO;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Commande 0x0212
    class SpeedUpHeroHealthCommand : Command
    {
        private int m_vBuildingId;

        public SpeedUpHeroHealthCommand(BinaryReader br)
        {
            m_vBuildingId = br.ReadInt32WithEndian();
            br.ReadInt32WithEndian();
        }
    }
}