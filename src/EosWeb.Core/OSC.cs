//using Bespoke.Common;
//using Bespoke.Common.Net;
//using Bespoke.Osc;
//using System;
//using System.Net;
//using System.Reactive.Linq;

//namespace EosWeb.Core
//{
//    public class OSC
//    {
//        private static int Port = 3032;
//        private static readonly IPAddress IPAddress = IPAddress.Parse("172.16.32.225");
//        static IPEndPoint EosOscEndpoint = new System.Net.IPEndPoint(IPAddress, Port);
//        static OscClient Client = new OscClient(EosOscEndpoint);
//        //static OscServer Server = new OscServer(TransportType.Tcp, IPAddress, Port, true);

        

//        static OSC()
//        {
//            //Server.FilterRegisteredMethods = false;
//            ////Server.RegisterMethod(AliveMethod);
//            ////Server.RegisterMethod(TestMethod);
//            //Server.BundleReceived += new EventHandler<OscBundleReceivedEventArgs>(Server_BundleReceived);
//            //Server.MessageReceived += new EventHandler<OscMessageReceivedEventArgs>(oscServer_MessageReceived);
//            //Server.ReceiveErrored += new EventHandler<ExceptionEventArgs>(oscServer_ReceiveErrored);
//            //Server.ConsumeParsingExceptions = false;

//            //Server.Start();

//            Client.Connect();
            
//        }

//        //public static IObservable<OscMessage> ObserveEos(OscMessage message)
//        //{


//        //}
//        private static int BundlesReceivedCount;
//        private static int MessagesReceivedCount;

//        private static void Server_BundleReceived(object sender, OscBundleReceivedEventArgs e)
//        {
//            BundlesReceivedCount++;

//            OscBundle bundle = e.Bundle;
//            Console.WriteLine(string.Format("\nBundle Received [{0}:{1}]: Nested Bundles: {2} Nested Messages: {3}", bundle.SourceEndPoint.Address, bundle.TimeStamp, bundle.Bundles.Count, bundle.Messages.Count));
//            Console.WriteLine("Total Bundles Received: {0}", BundlesReceivedCount);
//        }

//        private static void oscServer_MessageReceived(object sender, OscMessageReceivedEventArgs e)
//        {
//            MessagesReceivedCount++;

//            OscMessage message = e.Message;

//            Console.WriteLine(string.Format("\nMessage Received [{0}]: {1}", message.SourceEndPoint.Address, message.Address));
//            Console.WriteLine(string.Format("Message contains {0} objects.", message.Data.Count));

//            for (int i = 0; i < message.Data.Count; i++)
//            {
//                string dataString;

//                if (message.Data[i] == null)
//                {
//                    dataString = "Nil";
//                }
//                else
//                {
//                    dataString = (message.Data[i] is byte[]? BitConverter.ToString((byte[])message.Data[i]) : message.Data[i].ToString());
//                }
//                Console.WriteLine(string.Format("[{0}]: {1}", i, dataString));
//            }

//            Console.WriteLine("Total Messages Received: {0}", MessagesReceivedCount);
//        }

//        private static void oscServer_ReceiveErrored(object sender, ExceptionEventArgs e)
//        {
//            Console.WriteLine("Error during reception of packet: {0}", e.Exception.Message);
//        }


//        public static void Send(string key)
//        {
//            var EosOscEndpoint = new System.Net.IPEndPoint(IPAddress.Parse("172.16.32.225"), 3032);

//            Bespoke.Osc.OscMessage message = new Bespoke.Osc.OscMessage(EosOscEndpoint, $"/eos/key/{key}", Client);
//            message.Send();
//        }
        
//    }
//}
