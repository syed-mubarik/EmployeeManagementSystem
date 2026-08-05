using AutoMapper;
using EmployeeManagement.Application.DTOs.Designation;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Mappings
{
    public class DesignationProfile : Profile
    {
        public DesignationProfile()
        {
            // Entity → DTO
            CreateMap<Designation, DesignationDto>().ReverseMap();

            // Create
            CreateMap<CreateDesignationDto, Designation>().ReverseMap();

            // Update
            CreateMap<UpdateDesignationDto, Designation>().ReverseMap();
        }
    }
}
