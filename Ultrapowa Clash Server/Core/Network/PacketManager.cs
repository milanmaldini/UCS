using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;
using UCS.Core;
using UCS.PacketProcessing;

namespace UCS.Network
{
    internal class PacketManager
    {
        private static readonly EventWaitHandle m_vIncomingWaitHandle = new AutoResetEvent(false);
        private static readonly EventWaitHandle m_vOutgoingWaitHandle = new AutoResetEvent(false);
        private static ConcurrentQueue<Message> m_vIncomingPackets;
        private static ConcurrentQueue<Message> m_vOutgoingPackets;
        private bool m_vIsRunning;

        public PacketManager()
        {
            m_vIncomingPackets = new ConcurrentQueue<Message>();
            m_vOutgoingPackets = new ConcurrentQueue<Message>();

            m_vIsRunning = false;
        }

        public static void ProcessIncomingPacket(Message p)
        {
            m_vIncomingPackets.Enqueue(p);
            m_vIncomingWaitHandle.Set();
        }

        public static void ProcessOutgoingPacket(Message p)
        {
            if (p.GetMessageType() != 10101)
                p.Encode();

            try
            {
                var pl = p.Client.GetLevel();
                var player = "";
                if (pl != null)
                    player = " (" + pl.GetPlayerAvatar().GetId() + ", " + pl.GetPlayerAvatar().GetAvatarName() + ")";
                Debugger.WriteLine("[UCS]   " + p.GetMessageType() + " " + p.GetType().Name + player);
                m_vOutgoingPackets.Enqueue(p);
                m_vOutgoingWaitHandle.Set();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Start()
        {
            m_vIsRunning = true;

            IncomingProcessingDelegate incomingProcessing = IncomingProcessing;
            incomingProcessing.BeginInvoke(null, null);

            OutgoingProcessingDelegate outgoingProcessing = OutgoingProcessing;
            outgoingProcessing.BeginInvoke(null, null);

            Console.WriteLine("[UCS]    Packet Manager started successfully");
        }

        private void IncomingProcessing()
        {
            while (m_vIsRunning)
            {
                m_vIncomingWaitHandle.WaitOne();
                Message p;
                while (m_vIncomingPackets.TryDequeue(out p))
                {
                    if (p.GetMessageType() != 10100 && p.GetMessageType() != 10101)
                        p.Client.Decrypt(p.GetData());
                    Logger.WriteLine(p, "R");
                    MessageManager.ProcessPacket(p);
                }
            }
        }

        private void OutgoingProcessing()
        {
            while (m_vIsRunning)
            {
                m_vOutgoingWaitHandle.WaitOne();
                Message p;
                while (m_vOutgoingPackets.TryDequeue(out p))
                {
                    Logger.WriteLine(p, "S");
                    if (p.GetMessageType() == 20000)
                    {
                        var sessionKey = ((SessionKeyMessage) p).Key;
                        p.Client.Encrypt(p.GetData());
                        p.Client.UpdateKey(sessionKey);
                    }
                    else if (p.GetMessageType() != 20100 && p.GetMessageType() != 10101)
                            p.Client.Encrypt(p.GetData());

                    try
                    {
                        if (p.Client.Socket != null)
                            p.Client.Socket.Send(p.GetRawData());
                        else
                            ResourcesManager.DropClient(p.Client.GetSocketHandle());
                    }
                    catch (Exception)
                    {
                        try
                        {
                            ResourcesManager.DropClient(p.Client.GetSocketHandle());
                            p.Client.Socket.Shutdown(SocketShutdown.Both);
                            p.Client.Socket.Close();
                        }
                        catch (Exception) { }
                    }
                }
            }
        }

        private delegate void IncomingProcessingDelegate();
        private delegate void OutgoingProcessingDelegate();
    }
}