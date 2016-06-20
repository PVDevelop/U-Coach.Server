using StructureMap;
using System;

namespace PVDevelop.UCoach.Server.UserManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                try
                {
                    var executor = ExecutorFactory.CreateAndSetupExecutor(args);
                    executor.Execute();
                    Console.WriteLine(executor.GetSuccessString());
                }
                catch
                {
                    Console.WriteLine("Failed");
                    new HelpExecutor().PrintHelp();
                }
            }
            else
            {
                try
                {
                    new HelpExecutor().PrintHelp();
                }
                catch(Exception  ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
