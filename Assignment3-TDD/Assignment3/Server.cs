using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using RDJTPServer.Helpers;

namespace RDJTPServer
{
    public class Server
    {
        public static void CreateServer()
        {
            Console.WriteLine("RDJTP server is starting.");
            var server = new TcpListener(IPAddress.Loopback, 5000);
            server.Start();
            try
            {
                while (true)
                {
                    Console.WriteLine("Server started. Waiting for a connection...");
                    var client = server.AcceptTcpClient();
                    var clientSetup = new ClientSetup();
                    Console.WriteLine("Connected.");

                    // Read more here: https://codinginfinite.com/multi-threaded-tcp-server-core-example-csharp/

                    var thread = new Thread(new ParameterizedThreadStart(clientSetup.HandleClientRequests));
                    thread.Start(client);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine($"CreateServer failed with error: {e.Message}");
                server.Stop();
            }
        }
    }

}