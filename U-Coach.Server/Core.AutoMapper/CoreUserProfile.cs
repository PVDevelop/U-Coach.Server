using System;
using AutoMapper;
using PVDevelop.UCoach.Server.Core.Service;

namespace PVDevelop.UCoach.Server.Core.AutoMapper
{
    public class CoreUserProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<CreateUCoachUserParams, Auth.WebDto.CreateUserParams>();
            CreateMap<CreateUCoachUserParams, ProduceConfirmationKeyParams>();
        }
    }
}
