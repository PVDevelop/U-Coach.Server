using System.Linq;
using PVDevelop.UCoach.Server.Logging;

namespace PVDevelop.UCoach.Server.UserManagement
{
    public static class ExecutorFactory
    {
        private static readonly ILogger _logger = LoggerFactory.CreateLogger(typeof(ExecutorFactory));

        public static IExecutor CreateAndSetupExecutor(string[] args)
        {
            var executors = ExecutorContainer.Instance.Container.GetAllInstances<IExecutor>();
            var arg = args[0];
            var executor = executors.Single(e => e.Command == arg);
            executor.Setup(args.Skip(1).ToArray());
            return executor;
        }
    }
}
