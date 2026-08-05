using EmployeeManagement.Application.Common.Pagination;

namespace EmployeeManagement.Application.Common.Pagination
{
   public class DesignationQueryParameters : QueryParameters
    {
        public string? SearchTerm {  get; set; }

        public string? SortBy { get; set; }

        public bool Descending { get; set; } = false;
    }
}
