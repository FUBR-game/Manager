using System;
using Manager.UserData;

namespace Manager.Interfaces
{
    public interface IUserController
    {
        UserStatus GetUserUserStatus(string googleToken);
        bool UserChangesStatus(string googleToken, UserStatus userStatus);
        bool UserEntersQueue(string googleToken);
        bool UserLeavesQueue(string googleToken);
        int UsersInQueue();

        event EventHandler ServerQueueFull;
    }
}