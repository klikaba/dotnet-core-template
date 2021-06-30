using AutoMapper;
using Klika.AuthApi.Model.DTOs;
using Klika.AuthApi.Model.Entities;

namespace Klika.AuthApi.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ApplicationUserDTO, ApplicationUser>().ReverseMap();
        }
    }
}
