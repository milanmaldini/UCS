using System;
using UCS.Core;
using System.Configuration;
using System.Net;
using System.Threading;
using System.Collections.Specialized;

namespace UCS.Network
{
    internal class Ucslist
    {

        private static string _api = ConfigurationManager.AppSettings["UCSListKey"];

        public static void Start()
        {
            const string REGISTRY_KEY = @"HKEY_CURRENT_USER\UltraClashServer";
            const string REGISTY_VALUE = "FirstRun";

            Console.WriteLine("UCSList : No API Key found for UCSList, please enter youre API, or leave it empty.");

            if (string.IsNullOrEmpty(_api) && Convert.ToInt32(Microsoft.Win32.Registry.GetValue(REGISTRY_KEY, REGISTY_VALUE, 0)) == 0)
            {
                Microsoft.Win32.Registry.SetValue(REGISTRY_KEY, REGISTY_VALUE, 1, Microsoft.Win32.RegistryValueKind.DWord);
                Console.WriteLine("UCSList : API Key => ");

                var rl = Console.ReadLine();

                if (rl != null && rl.Length != 25)
                {
                    Console.WriteLine("UCSList : Error when starting because API Key must be 25 characters long.");
                }
                else if (string.IsNullOrEmpty(rl) || string.IsNullOrWhiteSpace(rl))
                {
                    Console.WriteLine("UCSList : Skipping UCSList API Check...");
                }
                else if (rl.Length == 25)
                {
                    _api = rl;
                    ConfigurationManager.AppSettings.Set("UCSListKey", _api);
                    Console.WriteLine("Starting UCSList send data process with API : " + _api);
                    Timer();
                }
            }
            else if (string.IsNullOrEmpty(_api) && Convert.ToInt32(Microsoft.Win32.Registry.GetValue(REGISTRY_KEY, REGISTY_VALUE, 0)) == 0)
            {
                Console.WriteLine("UCSList : Skipping UCSList API Check...");
            }
            else if (_api.Length == 25)
            {
                Console.WriteLine("Starting UCSList send data process with API : " + _api);
                Timer();
            }
        }

        public static void Stop()
        {
            Console.WriteLine("Stopping UCSList API process..");
        }

        private static void Timer()
        {
            int startin = 60 - DateTime.Now.Second;
            var timer = new Timer(o => SendData(), null, startin * 1000, 300000);
        }

        private static void SendData()
        {
            var ucs = new WebClient();
            ucs.UploadValues("http://www.ucslist.com/api.php", new NameValueCollection
            {
                { "ApiKey", _api},
                { "OnlinePlayers", ResourcesManager.GetOnlinePlayers().Count.ToString()},
                { "TotalPlayers", ResourcesManager.GetAllPlayerIds().Count.ToString() }
            });
        }
    }
}
