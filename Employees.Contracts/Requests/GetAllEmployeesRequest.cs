using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Contracts.Requests
{
    public class GetAllEmployeesRequest
    {
        public  string? Name { get; init; }
        public  DateTime? DateOfBirth { get; init; }
        public  int? YearsOfExperience { get; init; }
        public string? SortBy { get; init; }
    }
}
