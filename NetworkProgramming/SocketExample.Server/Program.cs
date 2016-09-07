using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SocketExample.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server(IPAddress.Loopback, 10000);
            var startResult = server.Start();
            if (!startResult)
            {
                Console.ReadLine();
                return;
            }
            var client = server.WaitClient();
            if (client == null)
            {
                Extensions.WriteError(new Exception("Client connection fail"));
                return;
            }
            Extensions.WriteMessage($"Client {client.Address}:{client.Port} successfully connected");
            server.SendMessage("Hello world!");

            do
            {
                var message = server.ReceiveMessage();
                if (message.Equals(String.Empty))
                {
                    Console.ReadLine();
                    return;
                }
                Extensions.WriteMessage($"Client send: {message}");
                if(message.ToLower().Contains("bye")) break;
                Console.Write("Enter message to client: ");
                var messageToClient = Console.ReadLine();
                server.SendMessage(messageToClient);
            } while (true);
            if (!server.DisconnectClient())
            {
                Console.ReadLine();
                return;
            }
            server.Stop();
            Console.ReadLine();
        }
    }
}
