using System;
using System.Net.Sockets;
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

        /// <summary>
        /// Parses message and returns a valid response
        /// </summary>
        /// <param name="client"></param>
        /// <returns>An Ack object or a response that can be send back</returns>
        public object ExecuteMessage(TcpClient client)
        {
            var userController = UserController.GetUserController();
            switch (_message.MessageType)
            {
                case "StatusChange":
                {
                    var messageData = new {userId = new uint(), status = UserStatus.Offline};
                    messageData = JsonConvert.DeserializeAnonymousType(_message.MessageData, messageData);
                    
                    var statusChangSuccess = userController.UserChangesStatus(messageData.userId, messageData.status);

                    return new {messageType = "StatusChangeAck", messageData = JsonConvert.SerializeObject(new {MessageData = statusChangSuccess})};
                }
                case "JoinQueue":
                {
                    var messageData = new {userId = new uint()};
                    messageData = JsonConvert.DeserializeAnonymousType(_message.MessageData, messageData);

                    var ableToJoinQueue = userController.UserEntersQueue(messageData.userId);

                    return new {MessageType = "JoinQueueAck", messageData = JsonConvert.SerializeObject(new {MessageData = ableToJoinQueue})};
                }
                case "LeaveQueue":
                {
                    var messageData = new {userId = new uint()};
                    messageData = JsonConvert.DeserializeAnonymousType(_message.MessageData, messageData);

                    var ableToLeaveQueue = userController.UserEntersQueue(messageData.userId);

                    return new {MessageType = "LeaveQueueAck", messageData = JsonConvert.SerializeObject(new {MessageData = ableToLeaveQueue})};
                }
                case "GetUserStatus":
                {
                    var messageData = new {userId = new uint()};
                    messageData = JsonConvert.DeserializeAnonymousType(_message.MessageData, messageData);
                    var userStatus = userController.GetUserUserStatus(messageData.userId);

                    return new
                    {
                        MessageType = "ReturnGetUserStatus",
                        MessageData = JsonConvert.SerializeObject(new {UserId = messageData.userId, UserStatus = userStatus})
                    };
                }
                default:
                    return null;
            }
        }
    }
}