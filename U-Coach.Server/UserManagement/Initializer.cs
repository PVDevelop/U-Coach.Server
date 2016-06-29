using StructureMap;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PVDevelop.UCoach.Server.Mapper;
using PVDevelop.UCoach.Server.Auth.StructureMap;
using PVDevelop.UCoach.Server.Mongo.StructureMap;
using PVDevelop.UCoach.Server.Auth.AutoMapper;
using PVDevelop.UCoach.Server.Core.AutoMapper;
using PVDevelop.UCoach.Server.Core.StructureMap;

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
            _container = new Container(x =>
            {
                x.AddRegistry<AuthRegistry>();
                x.AddRegistry<SportsmanConfirmationRegistry>();
                x.AddRegistry<MongoRegistry>();
                x.For<IMapper>().Add(() => new MapperImpl(SetupMapperConfiguration));

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

        private void SetupMapperConfiguration(AutoMapper.IMapperConfiguration cfg)
        {
            cfg.AddProfile<UserProfile>();
            cfg.AddProfile<SportsmanConfirmationProfile>();
        }
    }
}
