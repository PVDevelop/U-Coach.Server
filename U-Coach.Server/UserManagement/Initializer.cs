using StructureMap;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PVDevelop.UCoach.Server.AuthService.Infrastructure;
using PVDevelop.UCoach.Server.Mongo.Infrastructure;
using PVDevelop.UCoach.Server.Mapper;

namespace PVDevelop.UCoach.Server.UserManagement
{
    public class Initializer
    {
        private static readonly object _sync = new object();
        private static Initializer _instance;

        public static Initializer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_sync)
                    {
                        if (_instance == null)
                        {
                            _instance = new Initializer();
                        }
                    }
                }
                return _instance;
            }
        }

        private readonly Container _container;

        private Initializer()
        {
            AutoMapper.Mapper.AddProfile<AuthServiceMappingProfile>();

            _container = new Container(x =>
            {
                x.AddRegistry<AuthServiceRegistry>();
                x.AddRegistry<MongoRegistry>();
                x.AddRegistry<MapperRegistry>();

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

        public IEnumerable<T> GetAllInstances<T>()
        {
            return _container.GetAllInstances<T>();
        }
    }
}
