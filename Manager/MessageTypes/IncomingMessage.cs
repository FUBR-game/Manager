using Manager.UserData;
using Newtonsoft.Json;

namespace Manager.MessageTypes
{
    public class IncomingMessage
    {
        private Message _message;

        public IncomingMessage(Message message)
        {
            _message = message;
        }

        public void ExecuteMessage()
        {
            var userController = UserController.GetUserController();
            switch (_message.MessageType)
            {
                case "StatusChange":
                {
                    var messageData = new {googleToken = "", status = UserStatus.Offline};
                    userController.UserChangesStatus(messageData.googleToken, messageData.status);
                    break;
                }
                case "JoinQueue":
                {
                    var messageData = new {googleToken = ""};
                    userController.UserEntersQueue(messageData.googleToken);
                    break;
                }
                case "LeaveQueue":
                {
                    var messageData = new {googleToken = ""};
                    userController.UserLeavesQueue(messageData.googleToken);
                    break;
                }
                default:
                    break;
            }
        }
    }
}