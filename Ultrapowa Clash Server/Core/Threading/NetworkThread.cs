using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using UCS.Network;
using System.Configuration;

namespace UCS.Core.Threading
{
    class NetworkThread
    {
        public static int ParseConfigInt(string str)
        {
            return int.Parse(ConfigurationManager.AppSettings[str]);
        }

        public static string parseConfigString(string str)
        {
            return ConfigurationManager.AppSettings[str];
        }

        public static string Name = "Network Thread";
        public static string Description = "Includes the Core (PacketManager etc.)";
        public static string Version = "1.0.0";
        public static string Author = "ExPl0itR";

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
                    Gateway g = new Gateway();
                    PacketManager ph = new PacketManager();
                    MessageManager dp = new MessageManager();
                    ResourcesManager rm = new ResourcesManager();
                    ObjectManager pm = new ObjectManager();
                    var ws = new ApiManagerPro(ApiManagerPro.SendResponse, "http://+:" + ParseConfigInt("proDebugPort") + "/" + parseConfigString("ApiKey") + "/");
                    Console.WriteLine("Your API key : {0}", parseConfigString("ApiKey"));
                    dp.Start();
                    ph.Start();
                    g.Start();
                    ws.Run();
                    ApiManager api = new ApiManager();
                    UCSList listapi = new UCSList();
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
