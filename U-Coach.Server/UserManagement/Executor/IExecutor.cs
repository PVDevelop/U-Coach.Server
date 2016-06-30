namespace PVDevelop.UCoach.Server.UserManagement.Executor
{
    public interface IExecutor
    {
        string Command { get; }
        string[] ArgumentNames { get; }
        string Description { get; }

        void Setup(string[] arguments);
        void Execute();
        string GetSuccessString();
    }
}
