using AutoMapper;
using EmployeeManagement.Application.DTOs.Department;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Mapping
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, DepartmentDto>();

            CreateMap<Department, DepartmentDetailDto>();

            CreateMap<CreateDepartmentDto, Department>();

            CreateMap<UpdateDepartmentDto, Department>();
        }
    }
}