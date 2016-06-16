using System;

namespace PVDevelop.UCoach.Server.UserManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var executor = ExecutorFactory.CreateExecutor(args);
                executor.Execute();
            }
            catch
            {
                Console.WriteLine("Failed");
                new HelpExecutor().Execute();
            }
        }
    }
}
