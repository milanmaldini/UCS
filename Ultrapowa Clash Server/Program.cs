using System;
using System.Configuration;
using UCS.Core.Threading;

namespace UCS
{
    class Program
    {
        /// <summary>
        /// Entry point of UCS
        /// </summary>
        /// <param name="args">Arguments</param>
        static void Main(string[] args)
        {
            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["guiMode"]))
                ConsoleThread.Start();
            else
                InterfaceThread.Start();
        }
    }
}
