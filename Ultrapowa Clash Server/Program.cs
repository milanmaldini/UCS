using System;
using System.Configuration;
using UCS.Core.Threading;

namespace UCS
{
    class Program
    {
        static void Main()
        {
            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["guiMode"]))
                ConsoleThread.Start();
            else
                InterfaceThread.Start();
        }
    }
}
