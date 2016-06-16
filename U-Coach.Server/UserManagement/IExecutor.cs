using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.UserManagement
{
    public interface IExecutor
    {
        void Setup(string[] arguments);
        void Execute();
    }
}
