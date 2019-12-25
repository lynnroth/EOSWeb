using Bespoke.Osc;
using Microsoft.Extensions.Configuration;
using PubSub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EosWeb.Core.Services
{
    public interface IOscClient
    {
        public void SendAsync(string line);
    }

    public class OscClientService : IOscClient, IDisposable
    {
        static Hub Hub = Hub.Default;
        static Core.OSC.OscClient Client;
        static bool Connected = false;

        readonly IConfiguration _configuration;
        public string OscHost => _configuration.GetSection("Eos")["EosOSCHost"];
        public int? OscPort => int.Parse(_configuration.GetSection("Eos")["EosOSCPort"]);
        
        public OscClientService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void Connect()
        {
            string address = OscHost;
            int port = OscPort.HasValue ? OscPort.Value : throw new ArgumentException("OscPort Must have a value");

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

        public async void SendAsync(string line)
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
