using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Contracts.Responses
{
    public class EmployeesResponse 
    {
        public required IEnumerable<EmployeeResponse> Items { get; init; }
    }
}
