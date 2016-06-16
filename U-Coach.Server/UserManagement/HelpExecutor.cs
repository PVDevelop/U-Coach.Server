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
            foreach(var executor in ExecutorContainer.Instance.Container.GetAllInstances<IExecutor>())
            {
                var str = string.Format(
                    "{0} {1}\t\t{2}", 
                    executor.Command, 
                    string.Join(
                        " ", 
                        executor.ArgumentsNames.Select(name => string.Format("<{0}>", name))),
                    executor.Description);

                Console.WriteLine(str);
            }
        }
    }
}
