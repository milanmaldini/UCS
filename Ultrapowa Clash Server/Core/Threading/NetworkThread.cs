using System;
using System.Configuration;
using System.Threading;
using UCS.Network;

namespace UCS.Core.Threading
{
    class NetworkThread
    {
        public static string Name = "Network Thread";
        public static string Description = "Includes the Core (PacketManager etc.)";
        public static string Version = "1.0.0";
        public static string Author = "ExPl0itR";

        /// <summary>
        /// Variable holding the thread itself
        /// </summary>
        private static Thread T { get; set; }

        public static int ParseConfigInt(string str)
        {
            return int.Parse(ConfigurationManager.AppSettings[str]);
        }

        public static string parseConfigString(string str)
        {
            return ConfigurationManager.AppSettings[str];
        }

        /// <summary>
        /// Starts the Thread
        /// </summary>
        public static void Start()
        {
            T = new Thread(() =>
            {
                var g = new Gateway();
                var ph = new PacketManager();
                var dp = new MessageManager();
                var rm = new ResourcesManager();
                var pm = new ObjectManager();
                var ws = new ApiManagerPro(ApiManagerPro.SendResponse,
                    "http://+:" + ParseConfigInt("proDebugPort") + "/" + parseConfigString("ApiKey") + "/");
                Console.WriteLine("Your API key : {0}", parseConfigString("ApiKey"));
                dp.Start();
                ph.Start();
                g.Start();
                ws.Run();
                var api = new ApiManager();
                Console.WriteLine("Server started, let's play Clash of Clans!");
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