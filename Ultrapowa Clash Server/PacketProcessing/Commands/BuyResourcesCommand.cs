using System.IO;
using UCS.Core;
using UCS.GameFiles;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    //Commande 0x206
    class BuyResourcesCommand : Command
    {
        private object m_vCommand;
        private byte m_vIsCommandEmbedded;
        private int m_vResourceCount;
        private int m_vResourceId;

        public BuyResourcesCommand(BinaryReader br)
        {
            m_vResourceId = br.ReadInt32WithEndian();
            m_vResourceCount = br.ReadInt32WithEndian();
            m_vIsCommandEmbedded = br.ReadByte();
            if (m_vIsCommandEmbedded >= 0x01)
            {
                m_vCommand = CommandFactory.Read(br);
            }
            br.ReadInt32WithEndian(); //Unknown1
        }

        public override void Execute(Level level)
        {
            var rd = (ResourceData) ObjectManager.DataTables.GetDataById(m_vResourceId);
            if (rd != null)
            {
                if (m_vResourceCount >= 1)
                {
                    if (!rd.PremiumCurrency)
                    {
                        var avatar = level.GetPlayerAvatar();
                        var diamondCost = GamePlayUtil.GetResourceDiamondCost(m_vResourceCount, rd);
                        var unusedResourceCap = avatar.GetUnusedResourceCap(rd);
                        if (m_vResourceCount <= unusedResourceCap)
                        {
                            if (avatar.HasEnoughDiamonds(diamondCost))
                            {
                                avatar.UseDiamonds(diamondCost);
                                avatar.CommodityCountChangeHelper(0, rd, m_vResourceCount);
                                if (m_vIsCommandEmbedded >= 1)
                                    ((Command) m_vCommand).Execute(level);
                            }
                        }
                    }
                }
            }
        }
    }
}