using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;

namespace PVDevelop.UCoach.Server.Mapper
{
    public class MapperRegistry : Registry
    {
        public MapperRegistry()
        {
            For<IMapper>().Use<MapperImpl>();
        }
    }
}
