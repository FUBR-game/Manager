using System;
using System.Collections.Generic;
using Manager.Interfaces;

namespace Manager.UserData
{
    public class MemoryUserController : IUserController
    {
        private static readonly Dictionary<string, UserStatus> OnlineUsers = new Dictionary<string, UserStatus>();
        private static readonly List<string> ServerQueue = new List<string>();

        public UserStatus GetUserUserStatus(string googleToken)
        {
            return OnlineUsers.ContainsKey(googleToken) ? OnlineUsers[googleToken] : UserStatus.Offline;
        }

        public bool UserChangesStatus(string googleToken, UserStatus userStatus)
        {
            if (userStatus == UserStatus.Offline)
            {
                return OnlineUsers.Remove(googleToken);
            }

            OnlineUsers[googleToken] = userStatus;

            return true;
        }

        public bool UserEntersQueue(string googleToken)
        {
            if (ServerQueue.Contains(googleToken))
            {
                return false;
            }
            ServerQueue.Add(googleToken);

            if (UsersInQueue() >= int.Parse(Environment.GetEnvironmentVariable("MaxPlayersInServer") ?? throw new NullReferenceException()))
            {
                OnServerQueueFull();
            }

            return true;
        }

        public bool UserLeavesQueue(string googleToken)
        {
            return ServerQueue.Remove(googleToken);
        }

        public event EventHandler ServerQueueFull;

        public int UsersInQueue()
        {
            return ServerQueue.Count;
        }

        protected virtual void OnServerQueueFull()
        {
            ServerQueueFull?.Invoke(this, EventArgs.Empty);
        }
    }
}