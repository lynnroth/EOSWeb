using System;
using System.Net.Sockets;
using System.Threading;
using TcpClient = NetCoreServer.TcpClient;
using System.Text;
using Bespoke.Osc;
using PubSub;
using System.Threading.Tasks;
using EosWeb.Core.OSC;

namespace EosWeb.Test
{

    public class S1
    {
        readonly Hub hub = Hub.Default;

        public S1()
        {
            Console.WriteLine("Subscribing");

            hub.Subscribe<EosWeb.Core.OSC.OscMessage>(null, m =>
            {
                Console.Write($"Received message: {m.Address} ");
                m.Data.ForEach((x) => Console.Write(x));
                Console.WriteLine();
            });

            hub.Subscribe<Bespoke.Osc.OscMessage>(null, m =>
            {
                Console.WriteLine($"Received message: {m.Address}");
            });

            hub.Subscribe<OscPacket>(null, m =>
            {
                Console.WriteLine($"Received packet: {m.Address} {m.Client}");
            });

            hub.Subscribe<int>(null, m =>
            {
                Console.WriteLine($"Received message: {m}");
            });

        }

    }


    class Program
    {


        static async Task Main(string[] args)
        {
            Hub hub = Hub.Default;

            Console.WriteLine("Publish: 1");
            hub.Publish<int>(1);
            await hub.PublishAsync<int>(2).ConfigureAwait(true);
            Console.WriteLine("Connecting....");



            // TCP server address
            string address = "172.16.32.225";
            if (args.Length > 0)
            {
                address = args[0];
            }

            // TCP server port
            int port = 3032;
            if (args.Length > 1)
            {
                port = int.Parse(args[1]);
            }

            Console.WriteLine($"TCP server address: {address}");
            Console.WriteLine($"TCP server port: {port}");

            Console.WriteLine();

            // Create a new TCP chat client
            using (var client = new Core.OSC.OscClient(address, port))
            {

                // Connect the client
                Console.Write("Client connecting...");
                client.ConnectAsync();
                Console.WriteLine("Done!");

                Console.WriteLine("Press Enter to stop the client or '!' to reconnect the client...");

                // Perform text input
                for (; ; )
                {
                    string line = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        line = "/eos/ping";
                    }

                    // Disconnect the client
                    if (line == "!")
                    {
                        Console.Write("Client disconnecting...");
                        client.DisconnectAsync();
                        Console.WriteLine("Done!");
                        //continue;
                        break;
                    }

                    Bespoke.Osc.OscMessage message = new Bespoke.Osc.OscMessage(null, line, null);
                    var messagebytes = message.ToByteArray();
                    // Send the entered text to the chat server

                    var packetbytes = OscPacket.ValueToByteArray(messagebytes);
                    await hub.PublishAsync(message).ConfigureAwait(false);
                    client.SendAsync(packetbytes);
                }

                // Disconnect the client
                Console.Write("Client disconnecting...");
                client.DisconnectAndStop();
            }
            Console.WriteLine("Done!");
        }

    }

}
