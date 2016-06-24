using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace PVDevelop.UCoach.Server.AuthService.Infrastructure
{
    public class AuthServiceMappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<User, MongoUser>();
            CreateMap<MongoUser, User>();
        }
    }
}
