using System;
using System.Linq;

namespace PVDevelop.UCoach.Server.UserManagement.Executor
{
    public class HelpExecutor
    {
        public void PrintHelp()
        {
            foreach(var executor in Initializer.Instance.GetAllInstances<IExecutor>())
            {
                var command =
                    executor.ArgumentNames == null ?
                    executor.Command :
                    string.Format("{0} {1}",
                    executor.Command,
                        string.Join(
                            " ",
                            executor.ArgumentNames.Select(name => string.Format("<{0}>", name))));
                var str = string.Format(
                    "{0}: {1}", 
                    command,
                    executor.Description);

                Console.WriteLine(str);
            }
        }
    }
}
