using EmployeeManagement.Domain.Enums;

namespace EmployeeManagement.Application.DTOs.Employees
{
    public class EmployeeSelfUpdateDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }
    }
}