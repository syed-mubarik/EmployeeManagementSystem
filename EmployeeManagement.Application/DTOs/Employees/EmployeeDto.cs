namespace EmployeeManagement.Application.DTOs.Employees
{
    public class EmployeeDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public decimal Salary { get; set; }

        public string DepartmentName { get; set; } = string.Empty;

        public string DesignationName { get; set; } = string.Empty;
    }
}