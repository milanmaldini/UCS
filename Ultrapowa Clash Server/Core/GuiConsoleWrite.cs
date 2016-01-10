using System;
using System.Configuration;

namespace UCS
{
    internal class GuiConsoleWrite
    {
        public static void cWrite(string Text) // write text to Gui Console
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["guiMode"]))
            {
                UCSGui._kForm.ConsoleCWrite(Text);
            }
        }

        public static void Write(string Text) // write new line of text to Gui Console

        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["guiMode"]))
            {
                UCSGui._kForm.update1(Text);
            }
        }
    }
}