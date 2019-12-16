using Bespoke.Osc;
using NetCoreServer;
using PubSub;
using System;
using System.Collections.Generic;

using System.Text;
using System.Threading;

namespace EosWeb.Core.OSC
{

    public class OscClient : TcpClient
    {
        Hub Hub = Hub.Default;
        public OscClient(string address, int port) : base(address, port) { }

        public void DisconnectAndStop()
        {
            _stop = true;
            DisconnectAsync();
            while (IsConnected)
            {
                Thread.Yield();
            }
        }

        protected override void OnConnected()
        {
            Console.WriteLine($"Chat TCP client connected a new session with Id {Id}");
        }

        protected override void OnDisconnected()
        {
            Console.WriteLine($"Chat TCP client disconnected a session with Id {Id}");

            // Wait for a while...
            Thread.Sleep(1000);

            // Try to connect again
            if (!_stop)
            {
                ConnectAsync();
            }
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            string converted = Encoding.UTF8.GetString(buffer, 4, Convert.ToInt32(size) - 4);
            int start = 4;
            var packet = OscMessage.FromByteArray(null, buffer, ref start, Convert.ToInt32(size) - 4);
            Hub.Publish<OscPacket>(packet);
        }

        protected override void OnError(System.Net.Sockets.SocketError error)
        {
            Console.WriteLine($"OSC TCP client caught an error with code {error}");
        }

        private bool _stop;
    }
}
