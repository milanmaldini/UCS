using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace UCS.Network
{
    internal class SocketAsyncEventArgsPool : IDisposable
    {
        private bool _disposed;

        private object _objLock;
        private Stack<SocketAsyncEventArgs> _pool;

        public SocketAsyncEventArgsPool(int capacity)
        {
            if (capacity < 1)
                throw new ArgumentOutOfRangeException("capacity cannot be less that 1.");

            Capacity = capacity;
            _objLock = new object();
            _pool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        public int Capacity { get; private set; }

        public int Count
        {
            get { return _pool.Count; }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public SocketAsyncEventArgs Pop()
        {
            lock (_objLock)
            {
                return _pool.Pop();
            }
        }

        public void Push(SocketAsyncEventArgs args)
        {
            lock (_objLock)
            {
                _pool.Push(args);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    for (int i = 0; i < _pool.Count; i++)
                    {
                        var args = _pool.Pop();
                        args.Dispose();
                    }
                }
                _disposed = true;
            }
        }
    }
}