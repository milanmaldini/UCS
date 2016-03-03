using System;
using System.IO;
using UCS.Helpers;
using UCS.Logic;
using UCS.Core;

namespace UCS.PacketProcessing
{
    //Packet 14316

    internal class PromoteAllianceMemberMessage : Message
    {
        private long m_vId;
        private int m_vRole;

        public PromoteAllianceMemberMessage(Client client, BinaryReader br) : base(client, br) { }

        public override void Decode()
        {
            using (var br = new BinaryReader(new MemoryStream(GetData())))
            {
                m_vId = br.ReadInt64WithEndian();
                m_vRole = br.ReadInt32WithEndian();
            }
        }

        public override void Process(Level level)
        {
            var target = ResourcesManager.GetPlayer(m_vId);
            var player = level.GetPlayerAvatar();
            if (player.GetAllianceRole() == 2 || player.GetAllianceRole() == 4)
            {
                if (player.GetAllianceId() == target.GetPlayerAvatar().GetAllianceId())
                {
                    target.GetPlayerAvatar().SetAllianceRole(m_vRole);
                    if (m_vRole == 2)
                        player.SetAllianceRole(4);
                }
            }
        }
    }
}