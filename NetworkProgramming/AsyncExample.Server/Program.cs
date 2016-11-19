using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AsyncExample.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server(10000, IPAddress.Loopback);
            server.Start();
            int commandIndex;
            do
            {
                commandIndex = Menu();
                switch (commandIndex)
                {
                    case 1:
                        {
                            Console.Write("Enter client number (-1 - to all): ");
                            int clientNumber = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Enter message: ");
                            var message = Console.ReadLine();
                            server.Send(clientNumber, message);

                            break;
                        }
                }
                Console.ReadKey(true);
            } while (commandIndex != 10);
        }

        private static int Menu()
        {
            int result;
            do
            {
                Console.Clear();
                Console.WriteLine("1 - Send message");
                Console.WriteLine("10 - Exit");
                Console.Write("Enter number: ");
                if (!int.TryParse(Console.ReadLine(), out result))
                {
                    Console.WriteLine("Wrong input");
                    Console.ReadKey(true);
                }
            } while (result == 0);
            return result;
        }
    }
}
