using PVDevelop.UCoach.Server.Mongo;

namespace PVDevelop.UCoach.Server.UserManagement.Executor
{
    public class InitializeExecutor : IExecutor
    {
        public string[] ArgumentNames
        {
            get
            {
                return null;
            }
        }

        public string Command
        {
            get
            {
                return "initialize";
            }
        }

        public string Description
        {
            get
            {
                return "Инициализировать систему";
            }
        }

        public void Execute()
        {
        }

        public string GetSuccessString()
        {
            return "Система инициализирована";
        }

        public void Setup(string[] arguments)
        {
            foreach(var mongoInit in Initializer.Instance.GetAllInstances<IMongoInitializer>())
            {
                mongoInit.Initialize();
            }
        }
    }
}
