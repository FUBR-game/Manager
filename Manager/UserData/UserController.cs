using System;
using Manager.Interfaces;

namespace Manager.UserData
{
    public static class UserController
    {
        private static IUserController _controller;

        public static IUserController GetUserController()
        {
            if (_controller != null) return _controller;
            var cacheController = Environment.GetEnvironmentVariable("CacheController");
            switch (cacheController)
            {
                case "Memory":
                    goto default;
                default:
                    _controller = new MemoryUserController();
                    break;
            }

            return _controller;
        }
    }
}