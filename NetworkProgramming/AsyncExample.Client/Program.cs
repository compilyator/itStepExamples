using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
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

    public class Client
    {
        private bool _connected;

        public static int SendChunkLength = 10240;
        public static int ReceiveChunkLength = 10240;
        
        class SendCallbackInfo
        {
            public byte[] Array;

            public int Offset;

            public int ClientIndex;
        }

        class ReceiveObject
        {
            public List<byte> Bytes = new List<byte>();
            public byte[] TempBuffer = new byte[ReceiveChunkLength];
            public Socket Socket;
        }

        public void Connect(IPAddress address, int port)
        {
            if (_connected)
            {
                throw new InvalidOperationException("Already connected");
            }
            var socket = new Socket(SocketType.Stream, ProtocolType.IP);
            socket.Connect(address, port);
            if (!socket.Connected) return;
            Console.WriteLine("Connected!");
            var ro = new ReceiveObject
            {
                Socket = socket
            };
            socket.BeginReceive(
                buffer: ro.TempBuffer, 
                offset: 0, 
                size: ReceiveChunkLength, 
                socketFlags: SocketFlags.None, 
                callback: ReceiveCallback, 
                state: ro);
            _connected = true;
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            var ro = ar.AsyncState as ReceiveObject;
            if(ro == null) return;
            var length = ro.Socket.EndReceive(ar);
            ro.Bytes.AddRange(ro.TempBuffer.Take(length));
            if (length < ReceiveChunkLength)
            {
                var message = Encoding.UTF8.GetString(ro.Bytes.ToArray());
                WriteInfo($"Message from server: {message}");
                ro = new ReceiveObject
                {
                    Socket = ro.Socket
                };
            }
            ro.Socket.BeginReceive(
                buffer: ro.TempBuffer,
                offset: 0,
                size: ReceiveChunkLength,
                socketFlags: SocketFlags.None,
                callback: ReceiveCallback,
                state: ro);
        }

        private void WriteInfo(string message)
        {
            Console.WriteLine($"[{DateTime.Now:F}]: {message}");
        }
    }
}
