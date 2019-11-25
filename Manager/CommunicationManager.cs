using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Manager.MessageTypes;
using Manager.UserData;
using Newtonsoft.Json;

namespace Manager
{
    public class CommunicationManager
    {
        
        private const int ClientPort = 61681;
        private const int ServerPort = 61683;

        private ClientManager _clientManager = new ClientManager();
        private ServerManager _serverManager = new ServerManager();

        public CommunicationManager()
        {
            var userController = UserController.GetUserController();
            userController.ServerQueueFull += UserControllerOnServerQueueFull;
        }

        private void UserControllerOnServerQueueFull(object sender, EventArgs e)
        {
            NewServerNeeded?.Invoke(sender, e);
        }

        /// <summary>
        /// Queue has enough players for new server, should start a new server
        /// </summary>
        public event EventHandler NewServerNeeded;

        private class ClientManager : Server
        {
            public ClientManager() : base(ClientPort)
            {
                StartListener();
            }

            /// <inheritdoc />
            protected override async void HandleClient(object obj)
            {
                var client = (TcpClient) obj;

                var networkStream = client.GetStream();

                while (client.Connected)
                {
                    if (networkStream.DataAvailable)
                    {
                        byte[] messageByteArray;
                        await using (var memStream = new MemoryStream())
                        {
                            // Read message size
                            var dataSize = new byte[4];
                            networkStream.Read(dataSize, 0, 4);
                            var messageSize = BitConverter.ToInt32(dataSize);

                            // Read message
                            var numBytesRead = 0;
                            while (numBytesRead != messageSize)
                            {
                                var data = new byte[messageSize];
                                numBytesRead = networkStream.Read(data, 0, messageSize-numBytesRead);
                                memStream.Write(data, 0, numBytesRead);
                            }
                            messageByteArray = memStream.ToArray();
                        }

                        // Parse message
                        var jsonString = Encoding.UTF8.GetString(messageByteArray);
                        var message = (Message)JsonConvert.DeserializeObject(jsonString,typeof(Message));
                
                        var incomingMessage = new IncomingMessage(message);
                        var returnData = incomingMessage.ExecuteMessage(client);

                        if (returnData != null)
                        {
                            SendMessage(client, returnData);
                        }
                    }
                    Thread.Sleep(500);
                }
            }
        }

        private class ServerManager : Server
        {
            public ServerManager() : base(ServerPort)
            {
                StartListener();
            }

            protected override void HandleClient(object obj)
            {
                throw new NotImplementedException();
            }
        }
    }
}