using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using UCS.Core;
using UCS.Helpers;
using UCS.Network;
using Debugger = UCS.Core.Debugger;
using Menu = UCS.Core.Menu;

namespace UCS
{
    internal class Program
    {
        public static int Port = 9339;

        private static EventHandler _handler;
        private static bool isclosing = false;

        public static void ExitProgram()
        {
            Debugger.WriteLine("Starting saving all player to database", null, 0, ConsoleColor.Cyan);
            ResourcesManager.GetOnlinePlayers().ForEach(DatabaseManager.Singelton.Save);
            Environment.Exit(1);
        }

        [DllImport("Kernel32")]
        public static extern int GetConsoleWindow();

        public static void RestartProgram()
        {
            Debugger.WriteLine("Starting saving all player to database", null, 0, ConsoleColor.Cyan);
            ResourcesManager.GetOnlinePlayers().ForEach(DatabaseManager.Singelton.Save);
            Process.Start(@"tools\ucs-restart.bat");
        }

        private static void InitConsoleStuff()
        {
            Console.Title = GlobalStrings.__UCS;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(GlobalStrings.__ASCIIART);
            Console.WriteLine(GlobalStrings.__UCS);
            Console.WriteLine("Codename : {0}", GlobalStrings.__Codename);
            Console.WriteLine("www.ultrapowa.com");
            Console.WriteLine("");
            Console.WriteLine("Server starting...");
            Console.ResetColor();
        }

        private static void InitProgramThreads()
        {
            Debugger.WriteLine("\t", null, 5);
            Debugger.WriteLine("Server Thread's:", null, 5, ConsoleColor.Blue);
            var programThreads = new List<Thread>();
            for (var i = 0; i < int.Parse(ConfigurationManager.AppSettings["programThreadCount"]); i++)
            {
                var pt = new ProgramThread();
                programThreads.Add(new Thread(pt.Start));
                programThreads[i].Start();
                Debugger.WriteLine("\tServer Running On Thread " + i, null, 5, ConsoleColor.Blue);
            }
            Console.ResetColor();
        }

        private static void InitUCS()
        {
            if (!Directory.Exists("logs"))
            {
                Console.WriteLine("Folder \"logs/\" does not exist. Let me create one..");
                Directory.CreateDirectory("logs");
            }

            if (Convert.ToBoolean(Utils.parseConfigString("UCSList")))
                Ucslist.Start();
            new ResourcesManager();
            new ObjectManager();
            new Gateway().Start();

            if (Convert.ToBoolean(Utils.parseConfigString("apiManager")))
                new ApiManager();

            if (Convert.ToBoolean(Utils.parseConfigString("apiManagerPro")))
            {
                if (ConfigurationManager.AppSettings["ApiKey"] == "ucs")
                {
                    var ch = Utils.GenerateApi();
                    ConfigurationManager.AppSettings.Set("ApiKey", ch);
                    Console.WriteLine("Your Random API key : {0}", ch);
                }
                var ws = new ApiManagerPro(ApiManagerPro.SendResponse,
                                           "http://+:" + Utils.ParseConfigInt("proDebugPort") + "/" +
                                           Utils.parseConfigString("ApiKey") + "/");
                Console.WriteLine("Your API key : {0}", Utils.parseConfigString("ApiKey"));
                ws.Run();
            }

            Debugger.SetLogLevel(Utils.ParseConfigInt("loggingLevel"));
            Logger.SetLogLevel(Utils.ParseConfigInt("loggingLevel"));

            InitProgramThreads();

            if (Utils.ParseConfigInt("loggingLevel") >= 5)
            {
                Debugger.WriteLine("\t", null, 5);
                Debugger.WriteLine("Played ID's:", null, 5, ConsoleColor.Cyan);
                ResourcesManager.GetAllPlayerIds()
                    .ForEach(id => Debugger.WriteLine("\t" + id, null, 5, ConsoleColor.Cyan));
                Debugger.WriteLine("\t", null, 5);
            }
            Console.WriteLine("Server started on port " + Port + ". Let's play Clash of Clans!");

            if (Convert.ToBoolean(Utils.parseConfigString("consoleCommand")))
                new Menu();
            else
                Application.Run(new UCSManager());
        }

        private static void Main()
        {
            var win = GetConsoleWindow();
            if (Convert.ToBoolean(Utils.parseConfigString("guiMode")))
            {
                ShowWindow(win, 0);
                Application.Run(new UCSGui());
            }
            else
            {
                ShowWindow(win, 5);
                _handler += ExitHandler.Handler;
                SetConsoleCtrlHandler(_handler, true);
                InitConsoleStuff();
                InitUCS();
            }
        }

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(int handle, int showState);

        private delegate bool EventHandler(ExitHandler.CtrlType sig);
    }
}