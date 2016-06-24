using PVDevelop.UCoach.Server.AuthService;
using PVDevelop.UCoach.Server.Mongo;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.UserManagement
{
    public class ExecutorContainer
    {
        private static readonly object _sync = new object();
        private static ExecutorContainer _instance;

        public static ExecutorContainer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_sync)
                    {
                        if (_instance == null)
                        {
                            _instance = new ExecutorContainer();
                        }
                    }
                }
                return _instance;
            }
        }

        public Container Container { get; private set; }

        private ExecutorContainer()
        {
            Container = new Container(x =>
            {
                x.AddRegistry<AuthService.Infrastructure.AuthServiceRegistry>();
                x.AddRegistry<Mongo.Infrastructure.MongoRegistry>();

                foreach (var type in 
                    Assembly.
                    GetExecutingAssembly().
                    GetTypes().
                    Where(t => t.GetInterface(typeof(IExecutor).Name) != null))
                {
                    x.For(typeof(IExecutor)).Use(type);
                }
            });
        }
    }
}
