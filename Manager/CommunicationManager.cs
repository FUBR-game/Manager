using System;
using Manager.MessageTypes;
using Newtonsoft.Json;

namespace Manager
{
    public class CommunicationManager
    {
        
        private const int ClientPort = 61682;
        private const int ServerPort = 61683;

        private ClientManager _clientManager = new ClientManager();
        private ServerManager _serverManager = new ServerManager();

        public CommunicationManager(){}

        public event EventHandler NewServerNeeded;

        private class ClientManager : Server
        {
            public ClientManager() : base(ClientPort)
            {
                StartListener();
            }

            protected override void HandleClient(object obj)
            {
                var jsonString = (string) obj;
                var message = (Message) JsonConvert.DeserializeObject(jsonString);
                
                var incomingMessage = new IncomingMessage(message);
                incomingMessage.ExecuteMessage();
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