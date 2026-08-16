using AutoMapper;
using EmployeeManagement.Application.DTOs.Authentication;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Mappings
{
    public  class AuthProfile : Profile
    {
        public AuthProfile() 
        {
            CreateMap<RegisterRequestDto, ApplicationUser>()
                    .ForMember(dest => dest.UserName,
                               opt => opt.MapFrom(src => src.Email));

        }
    }
}
