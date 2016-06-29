using System;
using AutoMapper;
using PVDevelop.UCoach.Server.Core.Service;

namespace PVDevelop.UCoach.Server.Core.AutoMapper
{
    public class SportsmanConfirmationProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<CreateSportsmanConfirmationParams, Auth.WebDto.CreateUserParams>();
            CreateMap<CreateSportsmanConfirmationParams, ProduceConfirmationKeyParams>();
        }
    }
}
