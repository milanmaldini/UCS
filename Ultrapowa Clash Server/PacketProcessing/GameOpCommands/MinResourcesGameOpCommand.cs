using UCS.Core;
using UCS.GameFiles;
using UCS.Logic;
using UCS.Network;

namespace UCS.PacketProcessing
{
    internal class MinResourcesGameOpCommand : GameOpCommand
    {
        private string[] m_vArgs;

        public MinResourcesGameOpCommand(string[] args)
        {
            m_vArgs = args;
            SetRequiredAccountPrivileges(0);
        }

        public override void Execute(Level level)
        {
            if (level.GetAccountPrivileges() >= GetRequiredAccountPrivileges())
            {
                var dt = ObjectManager.DataTables.GetTable(2);
                for (var i = 0; i < dt.GetItemCount(); i++)
                {
                    var rd = (ResourceData) dt.GetItemAt(i);
                    if (!rd.PremiumCurrency)
                    {
                        var ca = level.GetPlayerAvatar();
                        ca.SetResourceCount(rd, ca.GetResourceCap(rd));
                    }
                    PacketManager.ProcessOutgoingPacket(new OwnHomeDataMessage(level.GetClient(), level));
                }
            }
            else
                SendCommandFailedMessage(level.GetClient());
        }
    }
}