using PVDevelop.UCoach.Server.Logging;
using StructureMap;
using System;
using PVDevelop.UCoach.Server.UserManagement.Executor;

namespace PVDevelop.UCoach.Server.UserManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = LoggerFactory.CreateLogger<Program>();
            logger.Debug("Приложение запущено.");

            if (args.Length > 0)
            {
                try
                {
                    var executor = ExecutorFactory.CreateAndSetupExecutor(args);
                    executor.Execute();
                    Console.WriteLine(executor.GetSuccessString());
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Ошибка при выполнении команды.");
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
                    logger.Error(ex, "Ошибка при выводе help.");
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
