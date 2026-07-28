using EmployeeManagement.Domain.Common;
using EmployeeManagement.Domain.Enums;
using static System.Formats.Asn1.AsnWriter;

namespace EmployeeManagement.Domain.Entities
{
    public class Employee : BaseEntity
    {
        public string EmployeeCode { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public DateTime JoiningDate { get; set; }

        public decimal Salary { get; set; }
        public int DepartmentId { get; set; }

  // does not store data in the database.But when Entity Framework loads the employee, it can automatically give you the related department.
        public Department Department { get; set; } = null!;
        public int DesignationId { get; set; }
        public Designation Designation { get; set; } = null!;
        public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;
    }
}