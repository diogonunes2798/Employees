using Employees.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Application.Repositories
{
    public interface IEmployeeRepository
    {
        Task<bool> CreateAsync(Employee employee, CancellationToken token = default);
        Task<Employee?> GetByIdAsync(Guid id, CancellationToken token = default);
        Task<IEnumerable<Employee>> GetAllAsync(GetAllEmployeesOptions options, CancellationToken token = default);
        Task<bool> UpdateAsync(Employee employee, CancellationToken token = default);
        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
        Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);
        Task<bool> ExistsAsync(string name, DateTime dateOfBirth, CancellationToken token = default);
    }
}
