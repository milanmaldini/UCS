using System;
using System.IO;
using UCS.Core;
using UCS.Logic;
using UCS.Network;

namespace UCS.PacketProcessing
{
    class CheckGlobalChatGameOpCommand : GameOpCommand
    {
        private string[] m_vArgs;

        public CheckGlobalChatGameOpCommand(string[] args)
        {
            m_vArgs = args;
            SetRequiredAccountPrivileges(0);
        }

        public override void Execute(Level level)
        {
            if (level.GetAccountPrivileges() >= GetRequiredAccountPrivileges())
            {
                if (m_vArgs.Length >= 1)
                {
                    if (File.Exists(@"filter.ucs"))
                    {
                        var p = new GlobalChatLineMessage(level.GetClient());
                        p.SetPlayerId(0);
                        p.SetPlayerName("UCS Global Status");
                        p.SetChatMessage("The Global Chat is online and working! If your Message didn't appear, check the Server Status.");
                        PacketManager.ProcessOutgoingPacket(p);
                        return;
                    }
                    else
                    {
                        var p = new GlobalChatLineMessage(level.GetClient());
                        p.SetPlayerId(0);
                        p.SetPlayerName("UCS Global Status");
                        p.SetChatMessage("The Global Chat is currently disabled. Check the Server Status for further informations!");
                        PacketManager.ProcessOutgoingPacket(p);
                        return;
                    }
                }
            }
            else
            {
                SendCommandFailedMessage(level.GetClient());
            }
        }
    }
}

//Signed off by iSwuerfel - 20.01.2016