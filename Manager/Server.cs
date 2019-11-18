﻿using System;
 using System.Net;
using System.Net.Sockets;
 using System.Reflection;
 using System.Runtime.CompilerServices;
 using System.Threading;
using System.Threading.Tasks;

namespace Manager
{
    public abstract class Server
    {
        private int _connectionsHandled;
        private readonly TcpListener server;

        public Server(int port)
        {
            var localAddr = IPAddress.Parse("127.0.0.1");
            server = new TcpListener(localAddr, port);
            server.Start();
        }

        public void StartListener()
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
            catch (SocketException e)
            {
                server.Stop();
            }
        }

        protected abstract void HandleClient(object obj);
    }

}