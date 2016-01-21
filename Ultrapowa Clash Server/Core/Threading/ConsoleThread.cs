using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Configuration;
using UCS.Helpers;
namespace UCS.Core.Threading
{
    class ConsoleThread
    {
        public static string Name = "Console Thread";
        public static string Description = "Manages Console I/O";
        public static string Version = "1.0.0";
        public static string Author = "ExPl0itR";

        /// <summary>
        /// Variable holding the thread itself
        /// </summary>
        private static Thread T { get; set; }

        private static string Title, Tmp, Command;
        /// <summary>
        /// Starts the Thread
        /// </summary>
        public static void Start()
        {
            T = new Thread(() =>
            {
                /* Animated Console Title */
                Title = "Ultrapowa Clash Server v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                for (int i = 0; i < Title.Length; i++)
                {
                    Tmp += Title[i];
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
                if(!System.IO.Directory.Exists("logs"))
                    System.IO.Directory.CreateDirectory("logs");
                Debugger.SetLogLevel(Int32.Parse(ConfigurationManager.AppSettings["loggingLevel"]));
                Logger.SetLogLevel(Int32.Parse(ConfigurationManager.AppSettings["loggingLevel"]));
                NetworkThread.Start();
                MemoryThread.Start();
                while ((Command = Console.ReadLine()) != null)
                {
                    CommandParser.Parse(Command);
                }
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
