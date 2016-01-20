using System;
using System.Configuration;
using System.Reflection;
using System.Runtime.Remoting.Channels;
using System.Threading;
using UCS.Helpers;
using UCS.Network;
using System.Threading;
using Timer = System.Timers.Timer;

namespace UCS.Core.Threading
{
    class ConsoleThread
    {
        public static string Name = "Console Thread";
        public static string Description = "Manages Console I/O";
        public static string Version = "1.0.0";
        public static string Author = "ExPl0itR";

        private static string Title, Tmp, Command;

        /// <summary>
        /// Variable holding the thread itself
        /// </summary>
        private static Thread T { get; set; }

        /// <summary>
        /// Starts the Thread
        /// </summary>
        public static void Start()
        {
            T = new Thread(() =>
            {
                /* Animated Console Title */
                Title = "Ultrapowa Clash Server v" + Assembly.GetExecutingAssembly().GetName().Version + " - Online Players : " + Gateway.ImPlayers;
                foreach (char title in Title)
                {
                    Tmp += title;
                    Console.Title = Tmp;
                    Thread.Sleep(35);
                }
                /* ASCII Art centered */
                Console.WriteLine(
                    @"
                    888     888  .d8888b.   .d8888b.
                    888     888 d88P  Y88b d88P  Y88b
                    888     888 888    888 Y88b.
                    888     888 888         ""Y888b.
                    888     888 888            ""Y88b.
                    888     888 888    888       ""888
                    Y88b. .d88P Y88b  d88P Y88b  d88P
                     ""Y88888P""   ""Y8888P""   ""Y8888P""
                  ");
                Console.WriteLine("Ultrapowa Clash Server");
                Console.WriteLine("Visit www.ultrapowa.com | www.shard.site");
                Console.WriteLine("Starting the server...");
                Console.WriteLine("");
                Debugger.SetLogLevel(int.Parse(ConfigurationManager.AppSettings["loggingLevel"]));
                Logger.SetLogLevel(int.Parse(ConfigurationManager.AppSettings["loggingLevel"]));
                NetworkThread.Start();
                MemoryThread.Start();
                while ((Command = Console.ReadLine()) != null)
                    CommandParser.Parse(Command);
            });
            T.Start();
        }

        /// <summary>
        /// Stops the Thread
        /// </summary>
        public static void Stop()
        {
            if (T.ThreadState == ThreadState.Running)
                T.Abort();
        }
    }
}