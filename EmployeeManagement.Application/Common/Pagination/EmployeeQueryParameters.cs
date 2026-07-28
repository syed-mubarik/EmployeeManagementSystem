using EmployeeManagement.Application.Common.Pagination;

namespace EmployeeManagement.Application.Common.Pagination
{
    public class EmployeeQueryParameters : QueryParameters
    {
        public string? SearchTerm { get; set; }

        public string? SortBy { get; set; }

        public bool Descending { get; set; } = false;

        public int? DepartmentId { get; set; }

        public int? DesignationId { get; set; }
    }
}