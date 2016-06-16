using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.UserManagement
{
    public class HelpExecutor : IExecutor
    {
        public void Execute()
        {
            Console.WriteLine("c <login> <password>\t\tCreate new user");
        }

        public void Setup(string[] arguments)
        {
        }
    }
}
