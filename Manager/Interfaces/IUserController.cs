using System;
using Manager.UserData;

namespace Manager.Interfaces
{
    public interface IUserController
    {
        /// <summary>
        /// Get UserStatus from a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns type="UserStatus"></returns>
        UserStatus GetUserUserStatus(uint userId);

        /// <summary>
        /// Change UserStatus from a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userStatus"></param>
        /// <returns></returns>
        bool UserChangesStatus(uint userId, UserStatus userStatus);

        /// <summary>
        /// Enters user into queue for a game
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool UserEntersQueue(uint userId);

        /// <summary>
        /// removes user from queue for a game
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool UserLeavesQueue(uint userId);

        int UsersInQueue();

        /// <summary>
        /// Queue has enough players to start a new server
        /// </summary>
        event EventHandler ServerQueueFull;
    }
}