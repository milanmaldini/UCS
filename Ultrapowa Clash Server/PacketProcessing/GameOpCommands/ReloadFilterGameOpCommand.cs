using System;
using UCS.Core;
using UCS.Logic;

namespace UCS.PacketProcessing
{
    internal class ReloadFilterGameOpCommand : GameOpCommand
    {
        private string[] m_vArgs;

        public ReloadFilterGameOpCommand(string[] args)
        {
            m_vArgs = args;
            SetRequiredAccountPrivileges(5);
        }

        public override void Execute(Level level)
        {
            if (level.GetAccountPrivileges() >= GetRequiredAccountPrivileges())
            {
                Message.ReloadChatFilterList();
                Debugger.WriteLine("Filterlist is reloaded!", null, 0, ConsoleColor.DarkCyan);
            }
            else
            {
                SendCommandFailedMessage(level.GetClient());
            }
        }
    }
}