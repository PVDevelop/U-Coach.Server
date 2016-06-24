using System;

namespace PVDevelop.UCoach.Server.Mapper
{
    public class MapperImpl : IMapper
    {
        private readonly AutoMapper.IMapper _mapper;

        public MapperImpl(Action<AutoMapper.IMapperConfiguration> callback)
        {
            _mapper = new AutoMapper.MapperConfiguration(callback).CreateMapper();
        }

        public TDest Map<TDest>(object source)
        {
            return _mapper.Map<TDest>(source);
        }
    }
}
