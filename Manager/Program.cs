using System;
using dotenv.net;

namespace Manager
{
    class Program
    {

        public static void Main(string[] args)
        {
            DotEnv.Config();
            var manager = new CommunicationManager();
            manager.NewServerNeeded += ManagerOnNewServerNeeded;
        }

        private static void ManagerOnNewServerNeeded(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}