using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UCS.Core;
using UCS.PacketProcessing;

namespace UCS.Network
{
    class Gateway
    {
        const int kPort = 9339, kHostConnectionBacklog = 30;
        // Berkan Code - 20/01/2016 - 01:50
        public static int ImPlayers = 0;

        private static Socket m_vServerSocket;
        IPAddress ip;

        public static Socket Socket
        {
            get { return m_vServerSocket; }
        }

        public IPAddress IP
        {
            get
            {
                if (ip == null)
                {
                    ip = (
                        from entry in Dns.GetHostEntry(Dns.GetHostName()).AddressList
                        where entry.AddressFamily == AddressFamily.InterNetwork
                        select entry
                        ).FirstOrDefault();
                }

                return ip;
            }
        }

        public void Start()
        {
            if (Host(kPort))
                Console.WriteLine("Gateway started on port " + kPort);
        }

        void Disconnect()
        {
            if (m_vServerSocket != null)
                m_vServerSocket.BeginDisconnect(false, OnEndHostComplete, m_vServerSocket);
        }

        void OnClientConnect(IAsyncResult result)
        {
            try
            {
                var clientSocket = m_vServerSocket.EndAccept(result);
                ResourcesManager.AddClient(new Client(clientSocket));
                SocketRead.Begin(clientSocket, OnReceive, OnReceiveError);
                Console.WriteLine("Client connected (" + ((IPEndPoint) clientSocket.RemoteEndPoint).Address + ":" +
                                  ((IPEndPoint) clientSocket.RemoteEndPoint).Port + ")");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception when accepting incoming connection: " + e);
            }
            try
            {
                m_vServerSocket.BeginAccept(OnClientConnect, m_vServerSocket);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception when starting new accept process: " + e);
            }
        }

        void OnReceive(SocketRead read, byte[] data)
        {
            try
            {
                var socketHandle = read.Socket.Handle.ToInt64();
                var c = ResourcesManager.GetClient(socketHandle);
                //Ajoute les données au stream client
                c.DataStream.AddRange(data);

                Message p;
                while (c.TryGetPacket(out p))
                {
                    PacketManager.ProcessIncomingPacket(p);
                }
            }
            catch (Exception)
            {
                //Client may not exist anymore
            }
        }

        void OnReceiveError(SocketRead read, Exception exception)
        {
            //Console.WriteLine("Error received: " + exception);
        }

        void OnEndHostComplete(IAsyncResult result)
        {
            m_vServerSocket = null;
        }

        public bool Host(int port)
        {
            //Console.WriteLine("Hosting on port " + port);

            m_vServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                m_vServerSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                m_vServerSocket.Listen(kHostConnectionBacklog);
                m_vServerSocket.BeginAccept(OnClientConnect, m_vServerSocket);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception when attempting to host (" + port + "): " + e);

                m_vServerSocket = null;

                return false;
            }

            return true;
        }
    }
}