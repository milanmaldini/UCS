using System.IO;
using UCS.Helpers;

namespace UCS.PacketProcessing
{
    //Commande 0x20B
    internal class ClaimAchievementRewardCommand : Command
    {
        public ClaimAchievementRewardCommand(BinaryReader br)
        {
            /*
            AchievementId = br.ReadUInt32WithEndian(); //= achievementId - 0x015EF3C0;
            Unknown1 = br.ReadUInt32WithEndian();
            */
        }
        public uint AchievementId { get; set; }
        public uint Unknown1 { get; set; }
    }
}