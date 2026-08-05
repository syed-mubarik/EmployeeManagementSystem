
namespace EmployeeManagement.Application.DTOs.Designation
{
    public  class UpdateDesignationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
