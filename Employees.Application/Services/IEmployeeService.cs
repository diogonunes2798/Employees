using Employees.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Application.Services
{
    public interface IEmployeeService
    {
        Task<bool> CreateAsync(Employee employee, CancellationToken token = default);
        Task<Employee?> GetByIdAsync(Guid id, CancellationToken token = default);
        Task<IEnumerable<Employee>> GetAllAsync(GetAllEmployeesOptions options, CancellationToken token = default);
        Task<Employee?> UpdateAsync(Employee employee, CancellationToken token = default);
        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
    }
}
