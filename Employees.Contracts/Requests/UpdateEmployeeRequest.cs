using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Contracts.Requests
{
    public class UpdateEmployeeRequest
    {
        public required string Name { get; init; }
        public required DateTime DateOfBirth { get; init; }
        public required int YearsOfExperience { get; init; }
        public required IEnumerable<string> Technologies { get; init; } = Enumerable.Empty<string>();
    }
}
