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
        string APIKey = ConfigurationManager.AppSettings["UCSList - APIKey"];
        int Status = CheckStatus();
        string UCSPanel = "https://www.ucspanel.tk/api/";

        public UCSList()
        {
            if (!string.IsNullOrEmpty(APIKey) && APIKey.Length == 25)
            {
                new Timer((Object stateInfo) => {
                    SendData();
                }, new AutoResetEvent(false), 0, 600000);
            }
            else
                Console.WriteLine("[UCSList] UCSList API is disabled - Visit www.ucslist.tk for more info.");
        }

        public void SendData()
        {
            var response = Http.Post(UCSPanel, new NameValueCollection() {
                { "ApiKey", APIKey },
                { "OnlinePlayers", Convert.ToString(ResourcesManager.GetOnlinePlayers().Count) },
                { "Status", Convert.ToString(Status) }
            });

            string result = System.Text.Encoding.UTF8.GetString(response);
            
            if (result == "true")
                Console.Write("[UCSList] UCS Sent data successfully.");
            else
                Console.WriteLine("[UCSList] UCSList Server answer uncorrectly, maybe wrong API Key ?");
        }

        public static class Http
        {
            public static byte[] Post(string uri, NameValueCollection pairs)
            {
                byte[] response = null;
                using (WebClient client = new WebClient())
                {
                    response = client.UploadValues(uri, pairs);
                }
                return response;
            }
        }

        public static int CheckStatus()
        {
            bool stat = Convert.ToBoolean(ConfigurationManager.AppSettings["maintenanceMode"]);
            if (stat)
                return 1;
            else
                return 2;
        }
    }
}
