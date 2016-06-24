using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.UserManagement
{
    public class HelpExecutor
    {
        public void PrintHelp()
        {
            foreach(var executor in Initializer.Instance.GetAllInstances<IExecutor>())
            {
                var command =
                    executor.ArgumentsNames == null ?
                    executor.Command :
                    string.Format("{0} {1}",
                    executor.Command,
                        string.Join(
                            " ",
                            executor.ArgumentsNames.Select(name => string.Format("<{0}>", name))));
                var str = string.Format(
                    "{0}: {1}", 
                    command,
                    executor.Description);

                Console.WriteLine(str);
            }
        }
    }
}
