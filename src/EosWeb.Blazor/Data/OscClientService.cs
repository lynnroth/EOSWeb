using Bespoke.Osc;
using PubSub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EosWeb.Blazor.Data
{
    public class OscClientService : IDisposable
    {
        static Hub Hub = Hub.Default;
        static Core.OSC.OscClient Client;
        static bool Connected = false;
        public static void Connect()
        {
            string address = "172.16.32.225";
            int port = 3032;

            Client = new EosWeb.Core.OSC.OscClient(address, port);
            Client.ConnectAsync();
            Connected = true;
        }

        public static void Disconnect()
        {
            // Disconnect the client
            Console.Write("Client disconnecting...");
            Client.DisconnectAndStop();
            Connected = false;
        }

        public static async void Send(string line)
        {
            if (!Connected)
            {
                Connect();
            }


            OscMessage message = new OscMessage(null, line, null);
            var messagebytes = message.ToByteArray();
            // Send the entered text to the chat server

            var packetbytes = OscPacket.ValueToByteArray(messagebytes);
            await Hub.PublishAsync(message).ConfigureAwait(true);
            Client.SendAsync(packetbytes);
        }

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Client.Disconnect();
                    Client.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
