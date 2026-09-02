using EmployeeManagement.Application.Common.Pagination;
using EmployeeManagement.Application.DTOs.Department;
using EmployeeManagement.Application.DTOs.Employees;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Interfaces.Services
{
    public interface IEmployeeService
    {
       // Task<IEnumerable<EmployeeDto>> GetAllAsync();
        
        Task<EmployeeDetailDto?> GetByIdAsync(int id);
        Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto);
        Task UpdateAsync(UpdateEmployeeDto dto);  // Admin / HR full update

        Task DeleteAsync(int id);
        Task<PagedResult<EmployeeDto>> GetEmployeesAsync(EmployeeQueryParameters queryParameters);
        
        // method that returns the entity specifically for authorization
        Task<Employee?> GetEmployeeForAuthorizationAsync(int id);
        EmployeeDetailDto MapToDetailDto(Employee employee);
        Task UpdateSelfAsync(EmployeeSelfUpdateDto dto);  // Employee limited self-update
    }
}