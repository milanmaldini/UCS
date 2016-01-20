using System;
using System.Net.Sockets;

//using System.Collections.Generic;

namespace UCS.Network
{
    public class SocketRead
    {
        public delegate void IncomingReadErrorHandler(SocketRead read, Exception exception);

        public delegate void IncomingReadHandler(SocketRead read, byte[] data);

        public const int kBufferSize = 256;
        byte[] buffer = new byte[kBufferSize];
        IncomingReadErrorHandler errorHandler;
        IncomingReadHandler readHandler;

        Socket socket;

        SocketRead(Socket socket, IncomingReadHandler readHandler, IncomingReadErrorHandler errorHandler = null)
        {
            this.socket = socket;
            this.readHandler = readHandler;
            this.errorHandler = errorHandler;

            BeginReceive();
        }

        public Socket Socket
        {
            get { return socket; }
        }

        void BeginReceive()
        {
            socket.BeginReceive(buffer, 0, kBufferSize, SocketFlags.None, OnReceive, this);
        }

        public static SocketRead Begin(Socket socket, IncomingReadHandler readHandler,
            IncomingReadErrorHandler errorHandler = null)
        {
            return new SocketRead(socket, readHandler, errorHandler);
        }

        void OnReceive(IAsyncResult result)
        {
            try
            {
                if (result.IsCompleted)
                {
                    var bytesRead = socket.EndReceive(result);

                    if (bytesRead > 0)
                    {
                        var read = new byte[bytesRead];
                        Array.Copy(buffer, 0, read, 0, bytesRead);

                        readHandler(this, read);
                        Begin(socket, readHandler, errorHandler);
                    }
                }
            }
            catch (Exception e)
            {
                if (errorHandler != null)
                {
                    errorHandler(this, e);
                }
            }
        }
    }
}