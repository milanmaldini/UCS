using System;
using System.Configuration;
using System.Threading;
using UCS.Network;

namespace UCS.Core.Threading
{
    internal class NetworkThread
    {
        public static string Author = "ExPl0itR";

        public static string Description = "Includes the Core (PacketManager etc.)";

        public static string Name = "Network Thread";

        public static string Version = "1.0.0";

        private static Thread T { get; set; }

        public static int ParseConfigInt(string str) => int.Parse(ConfigurationManager.AppSettings[str]);

        public static string parseConfigString(string str) => ConfigurationManager.AppSettings[str];
        
        public static void Start()
        {
            T = new Thread(() =>
            {
                new Gateway().Start();
                new PacketManager().Start();
                new MessageManager().Start();
                new ResourcesManager();
                new ObjectManager();
                //new ApiManagerPro(ApiManagerPro.SendResponse, "http://+:" + ParseConfigInt("proDebugPort") + "/" + parseConfigString("ApiKey") + "/").Run();
                //new UCSList();
                Console.WriteLine("[UCS]    Server started, let's play Clash of Clans!");
            });
            T.Start();
        }
        
        public static void Stop()
        {
            if (T.ThreadState == ThreadState.Running)
                T.Abort();
        }
    }
}