using PVDevelop.UCoach.Server.Logging;
using StructureMap;
using System;
using PVDevelop.UCoach.Server.UserManagement.Executor;

namespace PVDevelop.UCoach.Server.UserManagement
{
    class Program
    {
        private static ILogger _logger;

        static void Main(string[] args)
        {
            _logger = LoggerFactory.CreateLogger<Program>();
            _logger.Debug("Приложение запущено.");

            if (args.Length > 0)
            {
                try
                {
                    var executor = ExecutorFactory.CreateAndSetupExecutor(args);
                    executor.Execute();
                    Console.WriteLine(executor.GetSuccessString());
                }
                catch(StructureMapException ex)
                {
                    _logger.Fatal(ex, "Ошибка конфигурирования приложения.");
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Ошибка при выполнении команды.");
                    SafePrintHelp();
                }
            }
            else
            {
                SafePrintHelp();
            }
        }

        private static void SafePrintHelp()
        {
            try
            {
                new HelpExecutor().PrintHelp();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка при выводе help.");
                Console.WriteLine("Ошибка при выводе help");
            }
        }
    }
}
