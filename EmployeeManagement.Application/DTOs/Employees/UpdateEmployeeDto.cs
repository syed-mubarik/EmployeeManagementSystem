//using System.ComponentModel.DataAnnotations;
 using EmployeeManagement.Domain.Enums;

namespace EmployeeManagement.Application.DTOs.Employees
{
    public class UpdateEmployeeDto
    {
       
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public Gender Gender {  get; set; }
        public DateTime JoiningDate { get; set; }
        public decimal Salary { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
    }
}