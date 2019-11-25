using System;
using System.Collections.Generic;
using Manager.Interfaces;

namespace Manager.UserData
{
    public sealed class MemoryUserController : IUserController
    {
        private static readonly Dictionary<uint, UserStatus> OnlineUsers = new Dictionary<uint, UserStatus>{{2,UserStatus.Online},{3,UserStatus.Busy},{4,UserStatus.Away}};
        private static readonly List<uint> ServerQueue = new List<uint>();

        public UserStatus GetUserUserStatus(uint userId)
        {
            return OnlineUsers.ContainsKey(userId) ? OnlineUsers[userId] : UserStatus.Offline;
        }

        public bool UserChangesStatus(uint userId, UserStatus userStatus)
        {
            if (userStatus == UserStatus.Offline)
            {
                return OnlineUsers.Remove(userId);
            }

            OnlineUsers[userId] = userStatus;

            return true;
        }

        public bool UserEntersQueue(uint userId)
        {
            if (ServerQueue.Contains(userId))
            {
                return false;
            }
            ServerQueue.Add(userId);

            if (UsersInQueue() >= uint.Parse(Environment.GetEnvironmentVariable("MaxPlayersInServer") ?? throw new NullReferenceException()))
            {
                OnServerQueueFull();
            }

            return true;
        }

        public bool UserLeavesQueue(uint userId)
        {
            return ServerQueue.Remove(userId);
        }

        public event EventHandler ServerQueueFull;

        public int UsersInQueue()
        {
            return ServerQueue.Count;
        }

        private void OnServerQueueFull()
        {
            ServerQueueFull?.Invoke(this, EventArgs.Empty);
        }
    }
}