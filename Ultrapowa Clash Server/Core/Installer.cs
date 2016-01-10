using System;
using System.Configuration;

namespace UCS.Core
{
    internal class Installer
    {
        private const string REGISTRY_KEY = @"HKEY_CURRENT_USER\UltraClashServer";
        private const string REGISTY_VALUE = "FirstRun";

        public static void Main()
        {
            if (Convert.ToInt32(Microsoft.Win32.Registry.GetValue(REGISTRY_KEY, REGISTY_VALUE, 0)) == 0)
            {
                Console.WriteLine("[INSTALLER] : Seem to be the first time you use UCS.");
                Console.WriteLine("[INSTALLER] : Let's configure it together ! \n");

            A:
                Console.WriteLine("[INSTALLER] : Global player starting level (1 - 499) => ");
                var a = Console.ReadLine();
                if (Convert.ToInt32(a) < 1 || Convert.ToInt32(a) > 499)
                {
                    Console.WriteLine("[INSTALLER] : Value must be between 1 and 499 !");
                    goto A;
                }
                ConfigurationManager.AppSettings.Set("startingLevel", a);

            B:
                Console.WriteLine("[INSTALLER] : Global player starting experience (0 - 500k) => ");
                var b = Console.ReadLine();
                if (Convert.ToInt32(b) < 0 || Convert.ToInt32(b) > 500000)
                {
                    Console.WriteLine("[INSTALLER] : Error, value must be between 0 and 500 000 !");
                    goto B;
                }
                ConfigurationManager.AppSettings.Set("startingExperience", b);

            C:
                Console.WriteLine("[INSTALLER] : Global player starting gold (0 - 999m) => ");
                var c = Console.ReadLine();
                if (Convert.ToInt32(c) < 0 || Convert.ToInt32(c) > 999999999)
                {
                    Console.WriteLine("[INSTALLER] : Error, value must be between 0 and 999 999 999 !");
                    goto C;
                }
                ConfigurationManager.AppSettings.Set("startingGold", c);

            D:
                Console.WriteLine("[INSTALLER] : Global player starting elixir (0 - 999m) => ");
                var d = Console.ReadLine();
                if (Convert.ToInt32(d) < 0 || Convert.ToInt32(d) > 999999999)
                {
                    Console.WriteLine("[INSTALLER] : Error, value must be between 0 and 999 999 999 !");
                    goto D;
                }
                ConfigurationManager.AppSettings.Set("startingElixir", d);

            E:
                Console.WriteLine("[INSTALLER] : Global player starting dark elixir (0 - 99m) => ");
                var e = Console.ReadLine();
                if (Convert.ToInt32(e) < 0 || Convert.ToInt32(e) > 99999999)
                {
                    Console.WriteLine("[INSTALLER] : Error, value must be between 0 and 99 999 999 !");
                    goto E;
                }
                ConfigurationManager.AppSettings.Set("startingDarkElixir", e);

            F:
                Console.WriteLine("[INSTALLER] : Global player starting gems (0 - 999m) => ");
                var f = Console.ReadLine();
                if (Convert.ToInt32(f) < 0 || Convert.ToInt32(f) > 999999999)
                {
                    Console.Write("[INSTALLER] : Error, value must be between 0 and 999 999 999 !");
                    goto F;
                }
                ConfigurationManager.AppSettings.Set("startingGems", f);

            G:
                Console.WriteLine("[INSTALLER] : Global player starting trophies (0 - 5000) => ");
                var g = Console.ReadLine();
                if (Convert.ToInt32(g) < 0 || Convert.ToInt32(g) > 5000)
                {
                    Console.WriteLine("[INSTALLER] : Error, value must be between 0 and 5000 !");
                    goto G;
                }
                ConfigurationManager.AppSettings.Set("startingTrophies", g);

            H:
                Console.WriteLine("[INSTALLER] : Global player starting shield time (0 - x) => ");
                var h = Console.ReadLine();
                if (Convert.ToInt32(h) < 0)
                {
                    Console.WriteLine("[INSTALLER] : Error, value must be equal or superior than 0 !");
                    goto H;
                }
                ConfigurationManager.AppSettings.Set("startingShieldTime", h);

            I:
                Console.WriteLine("[INSTALLER] : This server is for CoC version (7 - 7.200.19) => ");
                Console.WriteLine("[INSTALLER] : - 1 - Clash of Clans - 7.200.19");
                Console.WriteLine("[INSTALLER] : - 2 - Clash of Clans - 7.200.13");
                Console.WriteLine("[INSTALLER] : - 3 - Clash of Clans - 7.200.12");
                Console.WriteLine("[INSTALLER] : - 4 - Clash of Clans - 7.156.10");
                var i = Console.ReadLine();
                if (Convert.ToInt16(i) == 1)
                {
                    i = "7.200.19";
                }
                else if (Convert.ToInt16(i) == 2)
                {
                    i = "7.200.13";
                }
                else if (Convert.ToInt32(i) == 3)
                {
                    i = "7.200.12";
                }
                else if (Convert.ToInt32(i) == 4)
                {
                    i = "7.156.10";
                }
                else
                {
                    Console.WriteLine("[INSTALLER] : Error, value must be 1, 2, 3, or 4 !");
                    goto I;
                }
                ConfigurationManager.AppSettings.Set("clientVersion", i);

            J:
                Console.WriteLine("[INSTALLER] : Do you want to use modded gamefiles (yes / no) => ");
                var j = Console.ReadLine();

                if (j == "no" || j == "n")
                {
                    var jr = false;
                    ConfigurationManager.AppSettings.Set("useCustomPatch", Convert.ToString(jr));
                }
                else if (j == "yes" || j == "y")
                {
                    var jr = true;
                    ConfigurationManager.AppSettings.Set("useCustomPatch", Convert.ToString(jr));
                }
                else
                {
                    Console.WriteLine("[INSTALLER] : Error, value must be Yes or No !");
                    goto J;
                }

            K:
                if (Convert.ToBoolean(j))
                {
                    Console.WriteLine("[INSTALLER] : Enter the URL of youre patch server => ");
                    var k = Console.ReadLine();
                    if (!k.EndsWith("/") || !k.StartsWith("http://") || !k.StartsWith("https://"))
                    {
                        Console.WriteLine("[INSTALLER] : Error when parsing URL ! Please enter a valid url, example : ");
                        Console.WriteLine("[INSTALLER] : http://www.ultrapowa.com/patch/");
                        Console.WriteLine("[INSTALLER] : https://patch.gobelinland.fr/");
                        Console.WriteLine("[INSTALLER] : https://www.flamewall.com/ucs/patch/");
                        goto K;
                    }
                    else
                    {
                        ConfigurationManager.AppSettings.Set("patchingServer", Convert.ToString(k));
                    }
                }

            L:
                Console.WriteLine("[INSTALLER] : You're server website (leave it empty if not) => ");
                var l = Console.ReadLine();
                if (string.IsNullOrEmpty(l))
                {
                    ConfigurationManager.AppSettings.Set("oldClientVersion", "http://www.ultrapowa.com/");
                }
                else if (!l.EndsWith("/") || !l.StartsWith("http://") || !l.StartsWith("https://"))
                {
                    Console.WriteLine("[INSTALLER] : Error when parsing URL ! Please enter a valid url, example : ");
                    Console.WriteLine("[INSTALLER] : http://www.ultrapowa.com/patch/");
                    Console.WriteLine("[INSTALLER] : https://patch.gobelinland.fr/");
                    Console.WriteLine("[INSTALLER] : https://www.flamewall.com/ucs/patch/");
                    goto L;
                }
                else
                {
                    ConfigurationManager.AppSettings.Set("oldClientVersion", l);
                }

            M:
                Console.WriteLine("[INSTALLER] : Database configuration => ");
                Console.WriteLine("[INSTALLER] : - 1 - MySQL");
                Console.WriteLine("[INSTALLER] : - 2 - SQLite");
                Console.WriteLine("[INSTALLEE] : Youre choice => ");
                var m = Console.ReadLine();
                if (Convert.ToInt16(m) == 1)
                {
                    m = "ucsdbEntities";
                    Console.WriteLine("[INSTALLER] : Warning, for ucs work with MySQL, you need to edit ucsdbEntities connection string in App.Config !");
                }
                else if (Convert.ToInt16(m) == 2)
                {
                    m = "sqliteEntities";
                }
                ConfigurationManager.AppSettings.Set("databaseConnectionName", m);
            }

        N:
            Console.WriteLine("[INSTALLER] : Logging configuration => ");
            Console.WriteLine("[INSTALLER] : - 1 - Almost no log");
            Console.WriteLine("[INSTALLER] : - 2 - Debugging log");
            Console.WriteLine("[INSTALLEE] : Youre choice => ");
            var n = Console.ReadLine();
            if (Convert.ToInt16(n) != 1 || Convert.ToInt16(n) != 2)
            {
                Console.WriteLine("[INSTALLER] : Error, value must be 1 or 2 !");
                goto N;
            }
            if (Convert.ToInt16(n) == 2)
            {
                n = "4";
            }
            ConfigurationManager.AppSettings.Set("logginLevel", n);

            // Berkan is going to sleep...
        }
    }
}