using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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

    public class Server
    {
        public static int SendChunkLength = 10240;

        class SendCallbackInfo
        {
            public byte[] Array;

            public int Offset;

            public int ClientIndex;
        }

        private bool _started;
        private Socket _serverSocket;

        public int Port { get; }

        public IPAddress Address { get; }

        public List<Socket> Clients { get; }

        public Server(int port, IPAddress address)
        {
            Port = port;
            Address = address;
            Clients = new List<Socket>();
        }

        public void Start()
        {
            if (_started)
            {
                throw new InvalidOperationException("Server already started");
            }
            _serverSocket = new Socket(SocketType.Stream, ProtocolType.IP);
            _serverSocket.Bind(new IPEndPoint(Address, Port));
            _serverSocket.Listen(10);
            _serverSocket.BeginAccept(AcceptCallback, _serverSocket);
            _started = true;
            WriteInfo("Server started");
        }

        public void Stop()
        {
            if (!_started)
            {
                throw new InvalidOperationException("Server already stopped");
            }
            Clients.ForEach(client => client.Disconnect(false));
            Clients.Clear();
            _serverSocket.Shutdown(SocketShutdown.Both);
            _started = false;
            WriteInfo("Server stopped");
        }

        public void Send(int index, string message)
        {
            if (index == -1)
            {
                SendToAll(message);
                return;
            }
            if (index < 0 || index >= Clients.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Client with index doesn't exists");
            }
            var client = Clients[index];
            var bytes = Encoding.UTF8.GetBytes(message);
            var callbackInfo = new SendCallbackInfo
            {
                ClientIndex = index,
                Array = bytes,
                Offset = 0
            };
            client.BeginSend(
                buffer: callbackInfo.Array,
                offset: callbackInfo.Offset,
                size: callbackInfo.Array.Length < SendChunkLength
                ? callbackInfo.Array.Length
                : SendChunkLength,
                socketFlags: SocketFlags.None,
                callback: SendCallback,
                state: callbackInfo);
        }

        private void SendCallback(IAsyncResult ar)
        {
            var callbackInfo = ar.AsyncState as SendCallbackInfo;
            if (callbackInfo == null) return;
            var length = Clients[callbackInfo.ClientIndex].EndSend(ar);
            callbackInfo.Offset += length;
            if (callbackInfo.Array.Length == callbackInfo.Offset)
            {
                WriteInfo($"Message to client {callbackInfo.ClientIndex} sended");
                return;
            }
            var size = callbackInfo.Array.Length - callbackInfo.Offset;
            Clients[callbackInfo.ClientIndex].BeginSend(
                buffer: callbackInfo.Array,
                offset: callbackInfo.Offset,
                size: size < SendChunkLength ? size : SendChunkLength,
                socketFlags: SocketFlags.None,
                callback: SendCallback,
                state: callbackInfo);
        }

        private void WriteInfo(string message)
        {
            Console.WriteLine($"[{DateTime.Now:F}]: {message}");
        }

        private void SendToAll(string message)
        {
            for (int i = 0; i < Clients.Count; i++)
            {
                Send(i, message);
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            var server = ar.AsyncState as Socket;
            if (server == null) return;
            var clientSocket = server.EndAccept(ar);
            Clients.Add(clientSocket);
            WriteInfo($"Client {clientSocket.RemoteEndPoint} connected as number {Clients.Count - 1}");
            server.BeginAccept(AcceptCallback, server);
        }
    }
}
