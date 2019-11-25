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
                case "Redis":
                    throw new NotSupportedException();
                    break;
                case "Memory":
                    _controller = new MemoryUserController();
                    break;
                default:
                    throw new NotSupportedException();
            }

            return _controller;
        }
    }
}