using System;
using UCS.Helpers;

namespace UCS.Core
{
    internal class ExitHandler
    {
        // An enumerated type for the control messages sent to the handler routine.
        public enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        public static bool Handler(CtrlType sig)
        {
            switch (sig)
            {
                case CtrlType.CTRL_C_EVENT:
                case CtrlType.CTRL_BREAK_EVENT:
                case CtrlType.CTRL_CLOSE_EVENT:
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                    Console.Clear();
                    Console.WriteLine(GlobalStrings.__ASCIIART);
                    Console.WriteLine("Exit event detected,Starting exit procedure!");
                    Program.ExitProgram();
                    break;
            }

            return true;
        }
    }
}