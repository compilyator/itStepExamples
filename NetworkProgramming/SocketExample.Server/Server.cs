using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketExample.Server
{
    public class Server
    {
        private Socket serverSocket;
        private Socket clientSocket;

        public Server(IPAddress address, int port)
        {
            Address = address;
            Port = port;
            serverSocket = new Socket(SocketType.Stream, ProtocolType.IP);
            serverSocket.Bind(new IPEndPoint(Address, Port));
        }

        public IPAddress Address { get; }

        public int Port { get; }

        public bool Start()
        {
            try
            {
                serverSocket.Listen(10);
                Extensions.WriteMessage($"Server started at {Address}:{Port}");
                return true;
            }
            catch (Exception exception)
            {
                exception.WriteError();
                return false;
            }
        }

        public IPEndPoint WaitClient()
        {
            try
            {
                clientSocket = serverSocket.Accept();
                return clientSocket.RemoteEndPoint as IPEndPoint;
            }
            catch (Exception exception)
            {
                exception.WriteError();
                return null;
            }
        }

        public void SendMessage(string message)
        {
            try
            {
                var formattedMessage = $"[{DateTime.Now:dd.MM.yyyy HH:mm:SS}]: {message}{Environment.NewLine}";
                var bytes = Encoding.UTF8.GetBytes(formattedMessage);
                clientSocket.Send(bytes);
            }
            catch (Exception exception)
            {
                exception.WriteError();
            }
        }

        public string ReceiveMessage()
        {
            try
            {
                byte[] bytes = new byte[1024];
                var builder = new StringBuilder();
                do
                {
                    var count = clientSocket.Receive(bytes);
                    var subStr = Encoding.UTF8.GetString(bytes, 0, count);
                    builder.Append(subStr);
                    if (subStr.Contains(Environment.NewLine)) break;
                } while (true);
                return builder.ToString();
            }
            catch (Exception exception)
            {
                exception.WriteError();
                return String.Empty;
            }
        }

        public bool DisconnectClient()
        {
            try
            {
                clientSocket.Close();
                return true;
            }
            catch (Exception exception)
            {
                exception.WriteError();
                return false;
            }
        }

        public void Stop()
        {
            try
            {
                serverSocket.Shutdown(SocketShutdown.Both);
                Extensions.WriteMessage("Server stopped");
            }
            catch (Exception e)
            {
                e.WriteError();
            }
        }
    }
}