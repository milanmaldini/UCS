using DNS.Server;
using System;
using System.Configuration;

namespace UCS_Dns
{
    internal class Program
    {
        public static bool Host()
        {
            try
            {
                // Proxy to google's DNS
                var server = new DnsServer("8.8.8.8");

                // Resolve these domain to localhost
                //server.MasterFile.AddIPAddressResourceRecord("google.com", "127.0.0.1");
                //server.MasterFile.AddIPAddressResourceRecord("github.com", "127.0.0.1");
                server.MasterFile.AddIPAddressResourceRecord("gamea.clashofclans.com", (ConfigurationManager.AppSettings["DnsIP"]));
                server.MasterFile.AddIPAddressResourceRecord("game.clashofclans.com", (ConfigurationManager.AppSettings["DnsIP"]));

                // Log every request
                server.Requested += (request) => Console.WriteLine(request);

                // On every successful request log the request and the response
                server.Responded += (request, response) => Console.WriteLine("{0} => {1}", request, response);

                // Start the server (by default it listents on port 53)
                Console.WriteLine(@"
888     888  .d8888b.   .d8888b.
888     888 d88P  Y88b d88P  Y88b
888     888 888    888 Y88b.
888     888 888         ""Y888b.
888     888 888            ""Y88b.
888     888 888    888       ""888
Y88b. .d88P Y88b  d88P Y88b  d88P
 ""Y88888P""   ""Y8888P""   ""Y8888P""
        ");
                Console.WriteLine("DNS Started on port 53");

                server.Listen();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception when attempting to host on port 53 : " + e);
                return false;
            }

            return true;
        }

        private static void Main()
        {
            // Start the server (by default it listents on port 53)
            if (Host())
            {
            }
        }
    }
}