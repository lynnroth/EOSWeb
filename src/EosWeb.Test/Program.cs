using System;
using System.Net.Sockets;
using System.Threading;
using TcpClient = NetCoreServer.TcpClient;
using System.Text;
using Bespoke.Osc;
using PubSub;
using System.Threading.Tasks;

namespace EosWeb.Test
{

    public class S1
    {

        Hub hub = Hub.Default;

        public S1()
        {
            Console.WriteLine("Subscribing");

            hub.Subscribe<OscMessage>(null, m =>
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
            S1 s = new S1();

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
            using (var client = new OscClient(address, port))
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

                    OscMessage message = new OscMessage(null, line, null);
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

    class OscClient : TcpClient
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
            //int offsetint = Convert.ToInt32(offset);

            //int length = BitConverter.ToInt32(buffer, 0) - 4;


            string converted = Encoding.UTF8.GetString(buffer, 4, Convert.ToInt32(size) - 4);
            int start = 4;
            var packet = OscMessage.FromByteArray(null, buffer, ref start, Convert.ToInt32(size) - 4);
            Hub.Publish<OscPacket>(packet);
            //Console.WriteLine(Encoding.UTF8.GetString(buffer, (int)offset, (int)size));
            //Console.WriteLine(packet);
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Chat TCP client caught an error with code {error}");
        }

        private bool _stop;
    }

}
