﻿namespace ZeroMQ.Sockets
{
    using System;

    using ZeroMQ.Proxy;

    internal class SubscribeSocket : ZmqSocket, ISubscribeSocket
    {
        public SubscribeSocket(ISocketProxy proxy)
            : base(proxy)
        {
        }

        public new event EventHandler<ReceiveReadyEventArgs> ReceiveReady
        {
            add { base.ReceiveReady += value; }
            remove { base.ReceiveReady -= value; }
        }

        public ReceivedMessage Receive()
        {
            return this.Receive(SocketFlags.None);
        }

        public new ReceivedMessage Receive(TimeSpan timeout)
        {
            return base.Receive(timeout);
        }

        public void SubscribeAll()
        {
            this.Subscribe(new byte[0]);
        }

        public new void Subscribe(byte[] prefix)
        {
            base.Subscribe(prefix);
        }

        public void UnsubscribeAll()
        {
            this.Unsubscribe(new byte[0]);
        }

        public new void Unsubscribe(byte[] prefix)
        {
            base.Unsubscribe(prefix);
        }
    }
}
