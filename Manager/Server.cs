﻿using System;
 using System.Net;
using System.Net.Sockets;
 using System.Reflection;
 using System.Runtime.CompilerServices;
 using System.Text;
 using System.Threading;
using System.Threading.Tasks;
 using Newtonsoft.Json;

 namespace Manager
{
    public abstract class Server
    {
        private readonly TcpListener server;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port">Port for the server to listen to</param>
        public Server(int port)
        {
            var localAddr = IPAddress.Parse("127.0.0.1");
            server = new TcpListener(localAddr, port);
            server.Start();
        }

        /// <summary>
        /// Starts a new thread that waits for a connection
        /// </summary>
        public void StartListener()
        {
            new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        Console.WriteLine("waiting for a connection...");
                        var client = server.AcceptTcpClient();

                        Console.WriteLine("New connection");
                        var thread = new Thread(HandleClient);
                        thread.Start(client);
                    }
                }
                catch (SocketException)
                {
                    Console.WriteLine("Client Disconnected");
                    server.Stop();
                }
            }).Start();
        }

        /// <summary>
        /// The method that gets called when a client connects
        /// </summary>
        /// <param type="TcpClient" name="obj">TcpClient that connected to the server</param>
        protected abstract void HandleClient(object obj);

        /// <summary>
        /// Sends object as json to client
        /// </summary>
        /// <param type="TcpClient" name="client"></param>
        /// <param name="messageObject"></param>
        protected void SendMessage(TcpClient client, object messageObject)
        {
            // return is client is no longer connected
            if (!client.Connected) return;
            
            // serialize object into json string
            var message = JsonConvert.SerializeObject(messageObject);
            
            // Get stream to write to
            var stream = client.GetStream();

            // Convert Message to byte array
            var messageByteArray = Encoding.UTF8.GetBytes(message);

            // Determine message length
            var messageLength = messageByteArray.Length;
            var messageLengthByteArray = new byte[4];
            messageLengthByteArray = BitConverter.GetBytes(messageLength);

            // Prepend message length to message
            var fullMessage = new byte[messageLength+4];
            Buffer.BlockCopy(messageLengthByteArray,0,fullMessage,0, messageLengthByteArray.Length);
            Buffer.BlockCopy(messageByteArray,0,fullMessage,messageLengthByteArray.Length, messageByteArray.Length);
            
            // Send full messsage
            stream.Write(fullMessage);
            stream.Flush();
        }
    }

}