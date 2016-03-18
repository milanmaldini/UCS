using System;
using System.IO;
using System.Text.RegularExpressions;
using UCS.PacketProcessing;

namespace UCS.Core
{
    internal static class Logger
    {
        private static readonly object m_vSyncObject = new object();
        private static readonly TextWriter m_vTextWriter;
        private static int m_vLogLevel;

        static Logger()
        {
            m_vTextWriter = TextWriter.Synchronized(File.AppendText("logs/data_" + DateTime.Now.ToString("yyyyMMdd") + ".log"));
            m_vLogLevel = 1;
        }

        public static void SetLogLevel(int level)
        {
            m_vLogLevel = level;
        }

        public static void WriteLine(Message p, string prefix = null, int logLevel = 4)
        {
            if (logLevel <= m_vLogLevel)
            {
                lock (m_vSyncObject)
                {
                    m_vTextWriter.Write(DateTime.Now.ToString("yyyyMMddHHmmss"), ";");
                    if (prefix != null)
                    {
                        m_vTextWriter.Write(prefix);
                        m_vTextWriter.Write(";");
                    }
                    m_vTextWriter.Write(p.GetMessageType().ToString(), "(", p.GetMessageVersion().ToString(), ");", p.GetLength().ToString(), ";", p.ToHexString(), "\n",
                        Regex.Replace(p.ToString(), @"[^\u0020-\u007F]", "."), "\n");
                    m_vTextWriter.Flush();
                }
            }
        }

        public static void WriteLine(string s, string prefix = null, int logLevel = 4)
        {
            if (logLevel <= m_vLogLevel)
            {
                lock (m_vSyncObject)
                {
                    m_vTextWriter.Write("{0} {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), ";");
                    if (prefix != null)
                    {
                        m_vTextWriter.Write(prefix, ";");
                    }
                    m_vTextWriter.WriteLine(s);
                    m_vTextWriter.Flush();
                }
            }
        }
    }
}
