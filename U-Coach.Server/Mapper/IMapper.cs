using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Mapper
{
    public interface IMapper
    {
        TDest Map<TDest>(object source);
    }
}
