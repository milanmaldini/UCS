using Sodium;
using System;
using System.Reflection;
using System.Threading;

namespace UCS_Key_Generator
{
    internal class Program
    {
        public static string Author = "Aidid";
        public static string Description = "Generate Public And Private Key For UCS";
        public static string Name = "Key Generator";
        public static string Version = "1.0.0";
        private static string _title, _tmp;
        private static Thread T { get; set; }

        internal static void Main(string[] args)
        {
            T = new Thread(() =>
            {
                /* Animated Console Title */
                _title = "UCS Key Generator " + " v" + Assembly.GetExecutingAssembly().GetName().Version;
                for (var i = 0; i < _title.Length; i++)
                {
                    _tmp += _title[i];
                    Console.Title = _tmp;
                    Thread.Sleep(35);
                }
                /* ASCII Art centered */
                Console.WriteLine(
                    @"
    888     888 888    88888888888 8888888b.         d8888 8888888b.   .d88888b.  888       888        d8888
    888     888 888        888     888   Y88b       d88888 888   Y88b d88P' 'Y88b 888   o   888       d88888
    888     888 888        888     888    888      d88P888 888    888 888     888 888  d8b  888      d88P888
    888     888 888        888     888   d88P     d88P 888 888   d88P 888     888 888 d888b 888     d88P 888
    888     888 888        888     8888888P'     d88P  888 8888888P'  888     888 888d88888b888    d88P  888
    888     888 888        888     888 T88b     d88P   888 888        888     888 88888P Y88888   d88P   888
    Y88b. .d88P 888        888     888  T88b   d8888888888 888        Y88b. .d88P 8888P   Y8888  d8888888888
     'Y88888P'  88888888   888     888   T88b d88P     888 888         'Y88888P'  888P     Y888 d88P     888
                  ");
                Console.WriteLine("[UCS]    -> This Program is made by the Ultrapowa Network Developer Team!");
                Console.WriteLine("[UCS]    -> You can find the source at www.ultrapowa.com and www.github.com/ucsteam/ucskey");
                Console.WriteLine("[UCS]    -> Don't forget to visit www.ultrapowa.com daily for news update !");
                Console.WriteLine("[UCS]    -> UCS Key Generator is now generating key..");
                while (true)
                {
                    var key = PublicKeyBox.GenerateKeyPair();
                    Console.WriteLine("[UCS]    -> Public Key = 0x" + BitConverter.ToString(key.PublicKey).Replace("-", ", 0x"));
                    Console.WriteLine("[UCS]    -> Private Key = 0x" + BitConverter.ToString(key.PrivateKey).Replace("-", ", 0x"));
                    Console.WriteLine("[UCS]    -> Need other key? Press y or if you want to exit press N");
                    ConsoleKeyInfo result = Console.ReadKey();
                    if ((result.KeyChar == 'Y') || (result.KeyChar == 'y'))
                        Console.Clear();
                    if ((result.KeyChar == 'N') || (result.KeyChar == 'n'))
                        Environment.Exit(1);
                    else
                        Console.WriteLine("[UCS]    -> Please choose between Y and N");
                }
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