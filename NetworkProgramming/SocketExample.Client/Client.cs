using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SocketExample.Server;

namespace SocketExample.Client
{
    public class Client
    {
        private Socket serverSocket;

        public bool Connect(IPAddress address, int port)
        {
            serverSocket = new Socket(SocketType.Stream, ProtocolType.IP);
            try
            {
                serverSocket.Connect(address, port);
                return true;
            }
            catch (Exception exception)
            {
                exception.WriteError();
                return false;
            }
        }

        public bool Disconnect()
        {
            if (!serverSocket.Connected) return false;
            try
            {
                serverSocket.Disconnect(false);
                return true;
            }
            catch (Exception exception)
            {
                exception.WriteError();
                return false;
            }
        }

        public void Send(string message)
        {
            if (!serverSocket.Connected) return;
            var bytes = Encoding.UTF8.GetBytes(message + Environment.NewLine);
            try
            {
                serverSocket.Send(bytes);
            }
            catch (Exception e)
            {
                e.WriteError();
            }
        }

        public string Receive()
        {
            if (!serverSocket.Connected) return String.Empty;
            int count;
            var builder = new StringBuilder();
            byte[] bytes = new byte[1024];
            do
            {
                try
                {
                    count = serverSocket.Receive(bytes);
                }
                catch (Exception e)
                {
                    e.WriteError();
                    break;
                }
                builder.Append(Encoding.UTF8.GetString(bytes, 0, count));
            } while (count == bytes.Length);
            return builder.ToString();
        }
    }
}