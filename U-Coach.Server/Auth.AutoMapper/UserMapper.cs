using AutoMapper;
using PVDevelop.UCoach.Server.Auth.Domain;
using PVDevelop.UCoach.Server.Auth.Mongo;

namespace PVDevelop.UCoach.Server.Auth.AutoMapper
{
    public class UserProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<User, MongoUser>();
            CreateMap<MongoUser, User>();
        }
    }
}
