using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Application.Models
{
    public class GetAllEmployeesOptions
    {
        public string? Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? YearsOfExperience { get; set; }
        public string? SortField { get; set; }
        public SortOrder? SortOrder { get; set; }
    }

    public enum SortOrder
    {
        Unsorted,
        Ascending, 
        Descending 
    }
}
