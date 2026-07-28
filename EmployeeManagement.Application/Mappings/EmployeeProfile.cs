using AutoMapper;
using EmployeeManagement.Application.DTOs.Employees;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Mappings
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>()
    .ForMember(dest => dest.FullName,
        opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))

    .ForMember(dest => dest.DepartmentName,
        opt => opt.MapFrom(src => src.Department.Name))

    .ForMember(dest => dest.DesignationName,
        opt => opt.MapFrom(src => src.Designation.Name));

            CreateMap<Employee, EmployeeDetailDto>()
              .ForMember(dest => dest.DepartmentName,
                    opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.DesignationName,
                    opt => opt.MapFrom(src => src.Designation.Name));

            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<UpdateEmployeeDto, Employee>();
        }

    }
}