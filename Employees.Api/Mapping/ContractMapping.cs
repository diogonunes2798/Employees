using Employees.Application.Models;
using Employees.Contracts.Requests;
using Employees.Contracts.Responses;

namespace Employees.Api.Mapping
{
    public static class ContractMapping
    {
        public static Employee MapToMovie(this CreateEmployeeRequest request)
        {
            return new Employee
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                DateOfBirth = request.DateOfBirth,
                YearsOfExperience = request.YearsOfExperience,
                Technologies = request.Technologies.ToList(),
            };
        }

        public static EmployeeResponse MapToResponse(this Employee employee)
        {
            return new EmployeeResponse
            {
                Id = employee.Id,
                Name = employee.Name,
                DateOfBirth = employee.DateOfBirth,
                YearsOfExperience = employee.YearsOfExperience,
                Technologies = employee.Technologies
            };
        }

        public static EmployeesResponse MapToResponse(this IEnumerable<Employee> employees)
        {
            return new EmployeesResponse
            {
                Items = employees.Select(MapToResponse)
            };
        }

        public static Employee MapToEmployee(this UpdateEmployeeRequest request, Guid id)
        {
            return new Employee
            {
                Id = id,
                Name = request.Name,
                DateOfBirth = request.DateOfBirth,
                YearsOfExperience = request.YearsOfExperience,
                Technologies = request.Technologies.ToList(),
            };
        }

        public static GetAllEmployeesOptions MapToOptions(this GetAllEmployeesRequest request)
        {
            return new GetAllEmployeesOptions
            {
                Name = request.Name,
                DateOfBirth = request.DateOfBirth,
                YearsOfExperience = request.YearsOfExperience,
                SortField = request.SortBy?.Trim('+','-'),
                SortOrder = request.SortBy is null ? SortOrder.Unsorted :
                    request.SortBy.StartsWith('-') ? SortOrder.Descending : SortOrder.Ascending
            };
        }
    }
}
