using System;
using System.Configuration;
using UCS.Core.Threading;
using UCS.Helpers;

namespace UCS
{
    class Program
    {
        private static void Main()
        {
            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["guiMode"]))
                ConsoleThread.Start();
            else
                InterfaceThread.Start();
        }
    }
}