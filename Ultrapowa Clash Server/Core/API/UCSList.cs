using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Collections.Specialized;

namespace UCS.Core
{
    class UCSList
    {
        static string APIKey = ConfigurationManager.AppSettings["UCSList - APIKey"];
        static int Status = CheckStatus();
        static string UCSPanel = "https://www.ucspanel.tk/api/";

        public UCSList()
        {
            if (!string.IsNullOrEmpty(APIKey) && APIKey.Length == 25)
            {
                new Timer((Object stateInfo) =>
                {
                    SendData();
                }, new AutoResetEvent(false), 0, 300000);
            }
            else
                Console.WriteLine("[UCSList] UCSList API is disabled - Visit www.ucspanel.tk for more info.");
        }

        public static void SendData()
        {
            string result = Http.Post(UCSPanel, new NameValueCollection() {
                { "ApiKey", APIKey },
                { "OnlinePlayers", Convert.ToString(ResourcesManager.GetOnlinePlayers().Count) },
                { "Status", Convert.ToString(Status) }
            }).Remove(0, 1);

            if (result == "OK")
                Console.WriteLine("[UCSList] UCS Sent data successfully.");
            else
                Console.WriteLine("[UCSList] UCSList Server answer uncorrectly : " + result);
        }

        public static class Http
        {
            public static string Post(string uri, NameValueCollection pairs)
            {
                byte[] response = null;
                using (WebClient client = new WebClient())
                {
                    response = client.UploadValues(uri, pairs);
                }
                return System.Text.Encoding.UTF8.GetString(response);
            }
        }

        public static int CheckStatus()
        {
            bool stat = Convert.ToBoolean(ConfigurationManager.AppSettings["maintenanceMode"]);
            if (stat)
                return 2;
            else
                return 1;
        }
    }
}