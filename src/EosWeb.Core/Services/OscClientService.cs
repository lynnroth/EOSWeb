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
        static readonly Hub Hub = Hub.Default;
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
            int port = OscPort ?? throw new ArgumentException("OscPort Must have a value");

            Client = new EosWeb.Core.OSC.OscClient(address, port);
            Client.ConnectAsync();
            Connected = true;
            Client.SendAsync("/eos/subscribe=1");
            
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

            Bespoke.Osc.OscMessage message = new Bespoke.Osc.OscMessage(null, line, null);
            var messagebytes = message.ToByteArray();
            // Send the entered text to the osc server

            var packetbytes = Bespoke.Osc.OscPacket.ValueToByteArray(messagebytes);
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
