using System;

namespace PVDevelop.UCoach.Server.Mapper
{
    public class MapperImpl : IMapper
    {
        public TDest Map<TDest>(object source)
        {
            return AutoMapper.Mapper.Map<TDest>(source);
        }
    }
}
