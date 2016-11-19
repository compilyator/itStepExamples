namespace AsyncExample.Server
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

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
            this.Port = port;
            this.Address = address;
            this.Clients = new List<Socket>();
        }

        public void Start()
        {
            if (this._started)
            {
                throw new InvalidOperationException("Server already started");
            }
            this._serverSocket = new Socket(SocketType.Stream, ProtocolType.IP);
            this._serverSocket.Bind(new IPEndPoint(this.Address, this.Port));
            this._serverSocket.Listen(10);
            this._serverSocket.BeginAccept(this.AcceptCallback, this._serverSocket);
            this._started = true;
            this.WriteInfo("Server started");
        }

        public void Stop()
        {
            if (!this._started)
            {
                throw new InvalidOperationException("Server already stopped");
            }
            this.Clients.ForEach(client => client.Disconnect(false));
            this.Clients.Clear();
            this._serverSocket.Shutdown(SocketShutdown.Both);
            this._started = false;
            this.WriteInfo("Server stopped");
        }

        public void Send(int index, string message)
        {
            if (index == -1)
            {
                this.SendToAll(message);
                return;
            }
            if (index < 0 || index >= this.Clients.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Client with index doesn't exists");
            }
            var client = this.Clients[index];
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
                callback: this.SendCallback,
                state: callbackInfo);
        }

        private void SendCallback(IAsyncResult ar)
        {
            var callbackInfo = ar.AsyncState as SendCallbackInfo;
            if (callbackInfo == null) return;
            var length = this.Clients[callbackInfo.ClientIndex].EndSend(ar);
            callbackInfo.Offset += length;
            if (callbackInfo.Array.Length == callbackInfo.Offset)
            {
                this.WriteInfo($"Message to client {callbackInfo.ClientIndex} sended");
                return;
            }
            var size = callbackInfo.Array.Length - callbackInfo.Offset;
            this.Clients[callbackInfo.ClientIndex].BeginSend(
                buffer: callbackInfo.Array,
                offset: callbackInfo.Offset,
                size: size < SendChunkLength ? size : SendChunkLength,
                socketFlags: SocketFlags.None,
                callback: this.SendCallback,
                state: callbackInfo);
        }

        private void WriteInfo(string message)
        {
            Console.WriteLine($"[{DateTime.Now:F}]: {message}");
        }

        private void SendToAll(string message)
        {
            for (int i = 0; i < this.Clients.Count; i++)
            {
                this.Send(i, message);
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            var server = ar.AsyncState as Socket;
            if (server == null) return;
            var clientSocket = server.EndAccept(ar);
            this.Clients.Add(clientSocket);
            this.WriteInfo($"Client {clientSocket.RemoteEndPoint} connected as number {this.Clients.Count - 1}");
            server.BeginAccept(this.AcceptCallback, server);
        }
    }
}