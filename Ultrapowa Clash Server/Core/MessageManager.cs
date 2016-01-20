using System;
using System.Collections.Concurrent;
using System.Threading;
using UCS.PacketProcessing;

namespace UCS.Core
{
    class MessageManager
    {
        private static ConcurrentQueue<Message> m_vPackets;
        private static EventWaitHandle m_vWaitHandle = new AutoResetEvent(false);

        private bool m_vIsRunning;

        public MessageManager()
        {
            m_vPackets = new ConcurrentQueue<Message>();
            m_vIsRunning = false;
        }

        public void Start()
        {
            PacketProcessingDelegate packetProcessing = PacketProcessing;
            packetProcessing.BeginInvoke(null, null);

            m_vIsRunning = true;

            Console.WriteLine("Message Manager started");
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
                        player += " (" + pl.GetPlayerAvatar().GetId() + ", " + pl.GetPlayerAvatar().GetAvatarName() +
                                  ")";
                    try
                    {
                        Debugger.WriteLine("[R] " + p.GetMessageType() + " " + p.GetType().Name + player);
                        p.Decode();
                        p.Process(pl);
                        //Debugger.WriteLine("finished processing of message " + p.GetType().Name + player);
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Debugger.WriteLine(
                            "An exception occured during processing of message " + p.GetType().Name + player, ex);
                        Console.ResetColor();
                    }
                }
            }
        }

        public static void ProcessPacket(Message p)
        {
            m_vPackets.Enqueue(p);
            m_vWaitHandle.Set();
        }

        private delegate void PacketProcessingDelegate();
    }
}