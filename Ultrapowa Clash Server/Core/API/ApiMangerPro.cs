using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Threading;

namespace UCS.Core
{
    public class ApiManagerPro
    {
        public static string jsonapp;
        private static readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerRequest, string> _responderMethod;

        public ApiManagerPro(string[] prefixes, Func<HttpListenerRequest, string> method)
        {
            try
            {
                Array.ForEach(prefixes, _listener.Prefixes.Add);
                _responderMethod = method;
                _listener.Start();
            }
            catch (Exception e)
            {
                Debugger.WriteLine("Exception at ApiManagerPro", e);
            }
        }

        public ApiManagerPro(Func<HttpListenerRequest, string> method, params string[] prefixes)
            : this(prefixes, method)
        {
        }

        public HttpListenerTimeoutManager TimeoutManager { get; }

        public IPEndPoint RemoteEndPoint { get; }

        public static void JsonMain()
        {
            try
            {
                var ucsVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                var f = new JsonApi
                {
                    Ucs = new Dictionary<string, string>
                    {
                        /*
                    Only disabled Till it's implemented

                    {"StartingLevel", ConfigurationManager.AppSettings["startingLevel"]},
                    {"StartingExperience", ConfigurationManager.AppSettings["startingExperience"]},
                    */
                        {"StartingGems", ConfigurationManager.AppSettings["startingGems"]},
                        {"StartingGold", ConfigurationManager.AppSettings["startingGold"]},
                        {"StartingElixir", ConfigurationManager.AppSettings["startingElixir"]},
                        {"StartingDarkElixir", ConfigurationManager.AppSettings["startingDarkElixir"]},
                        {"StartingTrophies", ConfigurationManager.AppSettings["startingTrophies"]},
                        {"StartingShieldTime", ConfigurationManager.AppSettings["startingShieldTime"]},
                        {"PatchingServer", ConfigurationManager.AppSettings["patchingServer"]},
                        {"Maintenance", ConfigurationManager.AppSettings["maintenanceMode"]},
                        {"MaintenanceTimeLeft", ConfigurationManager.AppSettings["maintenanceTimeLeft"]},
                        {"ClientVersion", ConfigurationManager.AppSettings["clientVersion"]},
                        {"ServerVersion", ucsVersion},
                        {"LoggingLevel", ConfigurationManager.AppSettings["loggingLevel"]},
                        //{"OldClientVersion", ConfigurationManager.AppSettings["oldClientVersion"]},
                        {"DatabaseType", ConfigurationManager.AppSettings["databaseConnectionName"]},
                        //{"SaveThreadCount", ConfigurationManager.AppSettings["saveThreadCount"]},
                        {"OnlinePlayers", Convert.ToString(ResourcesManager.GetOnlinePlayers().Count)},
                        {"InMemoryPlayers", Convert.ToString(ResourcesManager.GetInMemoryLevels().Count)},
                        {"InMemoryClans", Convert.ToString(ObjectManager.GetInMemoryAlliances().Count)},
                        //{"TotalClans", Convert.ToString(ObjectManager.GetInMemoryAlliances().Count)},
                        {"TotalConnectedClients", Convert.ToString(ResourcesManager.GetConnectedClients().Count)}
                    }
                };
                jsonapp = JsonConvert.SerializeObject(f);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in jsonmain for  A.A.S : " + ex);
                var e = new JsonApiE
                {
                    Error = new Dictionary<string, string>
                    {
                        {"Issue", Convert.ToString(ex)}
                    }
                };
                jsonapp = JsonConvert.SerializeObject(e);
            }
        }

        public static string SendResponse(HttpListenerRequest request)
        {
            try
            {
                JsonMain();
                return jsonapp;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception happend in API Manager PRO Respone : " + ex);
                return "Error in API Manager PRO.";
            }
        }

        public static void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }

        public void Run()
        {
            try
            {
                ThreadPool.QueueUserWorkItem(o =>
                {
                    try
                    {
                        Console.WriteLine("Pro API Manager : Online");
                        while (_listener.IsListening)
                        {
                            ThreadPool.QueueUserWorkItem(c =>
                            {
                                var ctx = c as HttpListenerContext;
                                try
                                {
                                    Debugger.WriteLine("New API Request!", null, 5);
                                    var rstr = _responderMethod?.Invoke(ctx.Request);
                                    var buf = Encoding.UTF8.GetBytes(rstr);
                                    ctx.Response.ContentLength64 = buf.Length;
                                    ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                                }
                                finally
                                {
                                    ctx.Response.OutputStream.Close();
                                }
                            }, _listener.GetContext());
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("APIManagerPro : Error when starting API => " + ex);
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in run at A.A.S : " + ex);
            }
        }

        private class JsonApi
        {
            public Dictionary<string, string> Ucs { get; set; }
        }

        private class JsonApiE
        {
            public Dictionary<string, string> Error { get; set; }
        }
    }
}