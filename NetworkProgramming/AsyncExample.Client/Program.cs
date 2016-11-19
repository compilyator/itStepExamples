using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace AsyncExample.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client();
            client.Connect(IPAddress.Loopback, 10000);
            Console.ReadLine();
        }
    }
}
