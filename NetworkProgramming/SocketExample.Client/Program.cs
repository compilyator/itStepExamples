using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SocketExample.Server;

namespace SocketExample.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client();
            if (!client.Connect(IPAddress.Loopback, 10000))
            {
                Console.ReadLine();
                return;
            }
            Extensions.WriteMessage($"Successfully connected to server");
            do
            {
                var message = client.Receive();
                Console.WriteLine(message);
                if (message.ToLower().Contains("bye")) break;
                Console.Write("Enter message to server: ");
                string messageToServer = Console.ReadLine();
                client.Send(messageToServer);
            } while (true);
            if (!client.Disconnect())
            {
                Console.ReadLine();
            }
        }
    }
}
