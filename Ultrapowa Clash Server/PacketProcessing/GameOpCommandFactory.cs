﻿using System;
using System.Collections.Generic;

namespace UCS.PacketProcessing
{
    internal static class GameOpCommandFactory
    {
        private static readonly Dictionary<string, Type> m_vCommands;

        static GameOpCommandFactory()
        {
            m_vCommands = new Dictionary<string, Type>();
            m_vCommands.Add("/attack", typeof (AttackGameOpCommand));
            m_vCommands.Add("/ban", typeof (BanGameOpCommand));
            m_vCommands.Add("/kick", typeof (KickGameOpCommand));
            m_vCommands.Add("/rename", typeof (RenameAvatarGameOpCommand));
            m_vCommands.Add("/setprivileges", typeof (SetPrivilegesGameOpCommand));
            m_vCommands.Add("/shutdown", typeof (ShutdownServerGameOpCommand));
            m_vCommands.Add("/unban", typeof (UnbanGameOpCommand));
            m_vCommands.Add("/visit", typeof (VisitGameOpCommand));
            m_vCommands.Add("/sysmsg", typeof (SystemMessageGameOpCommand));
            m_vCommands.Add("/id", typeof (GetIdGameopCommand));
        }

        public static object Parse(string command)
        {
            var commandArgs = command.Split(' ');
            object result = null;
            if (commandArgs.Length > 0)
            {
                if (m_vCommands.ContainsKey(commandArgs[0]))
                {
                    var type = m_vCommands[commandArgs[0]];
                    var ctor = type.GetConstructor(new[] {typeof (string[])});
                    result = ctor.Invoke(new object[] {commandArgs});
                }
            }
            return result;
        }
    }
}