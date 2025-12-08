using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Contracts.Responses
{
    public class ValidationFailureResponse
    {
        public required IEnumerable<ValidationResponse> Erros { get; init; }
    }

    public class ValidationResponse
    {
        public required string PropertyName { get; init; }
        public required string Message { get; init; }
    }
}
