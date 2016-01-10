using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UCS.Core;
using UCS.PacketProcessing;

namespace UCS.Network
{
    internal class Gateway
    {
        public Gateway()
        {
            for (int i = 0; i < _acceptPool.Capacity; i++) // populate 
                _acceptPool.Push(CreateAcceptArgs());
        }

        public Socket Listener = null;

        private IPAddress _ip;
        public IPAddress IP
        {
            get
            {
                if (_ip == null)
                {
                    _ip = (
                        from entry in Dns.GetHostEntry(Dns.GetHostName()).AddressList
                        where entry.AddressFamily == AddressFamily.InterNetwork
                        select entry
                        ).FirstOrDefault();
                }

                return _ip;
            }
        }

        private const int kHostConnectionBacklog = 30;
        private readonly int _port = Program.Port;
        private readonly SocketAsyncEventArgsPool _acceptPool = new SocketAsyncEventArgsPool(kHostConnectionBacklog * 2);

        public bool Host(int port)
        {
            Debugger.WriteLine("Hosting on port " + port, null, 5);
            SocketInformation si = new SocketInformation();
            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                var endPoint = new IPEndPoint(IPAddress.Any, Program.Port);
                Listener.Bind(endPoint);
                Listener.Listen(kHostConnectionBacklog);
                StartAccept();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception when attempting to host (" + port + "): " + ex);
                Listener = null;
                return false;
            }
            return true;
        }

        public void Start()
        {
            if (Host(_port))
            {
                Console.WriteLine("Gateway started on port " + _port + "\nMessage Manager started\nPacket Manager started");
            }
        }

        private void StartAccept()
        {
            var args = (SocketAsyncEventArgs) null;
            try
            {
                if (_acceptPool.Count > 1)
                {
                    try
                    {
                        args = _acceptPool.Pop();
                    }
                    catch
                    {
                        args = CreateAcceptArgs();
                    }
                }
                else
                {
                    args = CreateAcceptArgs();
                }

                if (!Listener.AcceptAsync(args))
                    ProcessAccept(args);
            }
            catch
            {
                try
                {
                    if (_acceptPool.Count > 1)
                    {
                        try
                        {
                            args = _acceptPool.Pop();
                        }
                        catch
                        {
                            args = CreateAcceptArgs();
                        }
                    }
                    else
                    {
                        args = CreateAcceptArgs();
                    }

                    if (!Listener.AcceptAsync(args))
                        ProcessAccept(args);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in StartAccept : " + ex);
                }
            }
        }

        private void ProcessAccept(SocketAsyncEventArgs args)
        {
            if (args.SocketError != SocketError.Success)
            {
                StartAccept(); // start to accept as soon as possible
                ProcessBadAccept(args);
                return;
            }

            StartAccept(); // start to accept as soon as possible

            var connection = args.AcceptSocket;
            var remoteEndPoint = (IPEndPoint)connection.RemoteEndPoint;

            ResourcesManager.AddClient(new Client(connection));
            SocketRead.Begin(connection, OnReceive, OnReceiveError);

            Console.WriteLine("Client connected ({0}:{1})", remoteEndPoint.Address, remoteEndPoint.Port);

            args.AcceptSocket = null;
            _acceptPool.Push(args);
        }

        private void ProcessBadAccept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket.Close(); // gently dispose and close the socket
            _acceptPool.Push(args);
        }

        private void AcceptOperationCompleted(object sender, SocketAsyncEventArgs args)
        {
            ProcessAccept(args);
        }

        private SocketAsyncEventArgs CreateAcceptArgs()
        {
            var args = new SocketAsyncEventArgs();
            args.Completed += AcceptOperationCompleted;
            return args;
        }

        private void OnReceive(SocketRead read, byte[] data)
        {
            try
            {
                var socketHandle = read.Socket.Handle.ToInt64();
                var c = ResourcesManager.GetClient(socketHandle);
                c.DataStream.AddRange(data);
                Message p;
                while (c.TryGetPacket(out p))
                {
                    PacketManager.ProcessIncomingPacket(p);
                }
            }
            catch (Exception ex)
            {
                Debugger.WriteLine("Error when receiving packet from client : ", ex, 4, ConsoleColor.Red);
            }
        }

        private void OnReceiveError(SocketRead read, Exception exception)
        {
            Debugger.WriteLine("Error received: " + exception, null, 5);
        }
    }
}