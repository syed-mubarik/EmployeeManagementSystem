using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Domain.Entities
{
    public class Designation : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Navigation Property
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
