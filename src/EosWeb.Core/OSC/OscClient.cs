using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using NetCoreServer;
using PubSub;

namespace EosWeb.Core.OSC
{

    public class OscClient : TcpClient
    {
        readonly Hub Hub = Hub.Default;
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
            int start = 4;
            var packet = Bespoke.Osc.OscPacket.FromByteArray(null, buffer, ref start, Convert.ToInt32(size) - 4);

            if (packet.IsBundle)
            {
                
            }
            else
            {
                var message = new OscMessage(packet.Address, packet.Data.ToList());
                Hub.Publish<OscMessage>(message);
            }
        }

        protected override void OnError(System.Net.Sockets.SocketError error)
        {
            Console.WriteLine($"OSC TCP client caught an error with code {error}");
        }

        private bool _stop;
    }
}
