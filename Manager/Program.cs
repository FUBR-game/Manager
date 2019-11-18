using System;

namespace Manager
{
    class Program
    {

        public static void Main(string[] args)
        {
            var manager = new CommunicationManager();
            manager.NewServerNeeded += ManagerOnNewServerNeeded;
        }

        private static void ManagerOnNewServerNeeded(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}