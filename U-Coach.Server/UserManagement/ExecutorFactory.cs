using System.Linq;

namespace PVDevelop.UCoach.Server.UserManagement
{
    public static class ExecutorFactory
    {
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
