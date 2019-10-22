using System;
using System.Net.Sockets;
using RDJTPServer.Helpers;

namespace RDJTPServer
{
    public class ClientSetup
    {
        public void HandleClientRequests(object clientObject)
        {
            var client = clientObject as TcpClient;
            try
            {
                var request = client.ReadRequest();
                var validation = new Validation();
                var response = validation.ValidateRequest(request);
                if (response.Status.Contains("1"))
                {
                    Console.WriteLine($"Response status was OK. Handling request.");
                    response = new HandleRequest().Respond(request);
                }
                var responseToSend = response.ToJson();

                Console.WriteLine($"Response sent: {responseToSend}");
                client.SendResponse(responseToSend);
            }
            catch (Exception e)
            {
                Console.WriteLine($"HandleClientRequests failed. Error was: \n'{e}'");
                client.Close();
            }
        }
    }
}