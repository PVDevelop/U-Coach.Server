using System;
using AutoMapper;

namespace PVDevelop.UCoach.Server.Mapper
{
    public static class MapperHelper
    {
        public static TDest Map<TSource, TDest>(
            TSource source)
        {
            var mapper =
                new MapperConfiguration(
                    x => x.CreateMap<TSource, TDest>())
                .CreateMapper();

            return mapper.Map<TDest>(source);
        }
    }
}
