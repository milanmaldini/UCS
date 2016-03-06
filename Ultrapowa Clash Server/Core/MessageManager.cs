using System;
using System.Collections.Concurrent;
using System.Threading;
using UCS.PacketProcessing;

namespace UCS.Core
{
    internal class MessageManager
    {
        private static readonly EventWaitHandle m_vWaitHandle = new AutoResetEvent(false);
        private static ConcurrentQueue<Message> m_vPackets;
        private bool m_vIsRunning;

        public MessageManager()
        {
            m_vPackets = new ConcurrentQueue<Message>();
            m_vIsRunning = false;
        }

        public static void ProcessPacket(Message p)
        {
            m_vPackets.Enqueue(p);
            m_vWaitHandle.Set();
        }

        public void Start()
        {
            PacketProcessingDelegate packetProcessing = PacketProcessing;
            packetProcessing.BeginInvoke(null, null);
            m_vIsRunning = true;
            Console.WriteLine("[UCS]    Message Manager started successfully");
        }

        private void PacketProcessing()
        {
            while (m_vIsRunning)
            {
                m_vWaitHandle.WaitOne();

                Message p;
                while (m_vPackets.TryDequeue(out p))
                {
                    var pl = p.Client.GetLevel();
                    var player = "";
                    if (pl != null)
                    {
                        player = " (" + pl.GetPlayerAvatar().GetId() + ", " + pl.GetPlayerAvatar().GetAvatarName() + ")";
                    }
                    try
                    {
                        Debugger.WriteLine("[UCS]   " + p.GetMessageType() + " " + p.GetType().Name + player);
                        //if (p.GetMessageType() != 10101)
                            p.Decode();
                        p.Process(pl);
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Debugger.WriteLine("[UCS]  An exception occured during processing of message " + p.GetType().Name + player, ex);
                        Console.ResetColor();
                    }
                }
            }
        }

        private delegate void PacketProcessingDelegate();
    }
}