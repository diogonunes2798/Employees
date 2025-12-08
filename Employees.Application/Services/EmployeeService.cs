using Employees.Application.Exceptions;
using Employees.Application.Models;
using Employees.Application.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IValidator<Employee> _employeeValidator;
        private readonly IValidator<GetAllEmployeesOptions> _optionsValidator;

        public EmployeeService(IEmployeeRepository employeeRepository, IValidator<Employee> employeeValidator, IValidator<GetAllEmployeesOptions> optionsValidator)
        {
            _employeeRepository = employeeRepository;
            _employeeValidator = employeeValidator;
            _optionsValidator = optionsValidator;
        }

        public async Task<bool> CreateAsync(Employee employee, CancellationToken token = default)
        {
            var exists = await _employeeRepository.ExistsAsync(employee.Name, employee.DateOfBirth, token);

            if (exists)
                throw new ConflictException("An employee with the same name and date of birth already exists.");

            await _employeeValidator.ValidateAndThrowAsync(employee, cancellationToken: token);

            return await _employeeRepository.CreateAsync(employee, token);
        }

        public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            return _employeeRepository.DeleteByIdAsync(id, token);
        }

        public async Task<IEnumerable<Employee>> GetAllAsync(GetAllEmployeesOptions options, CancellationToken token = default)
        {
            await _optionsValidator.ValidateAndThrowAsync(options, token);

            return await _employeeRepository.GetAllAsync(options, token);
        }

        public Task<Employee?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return _employeeRepository.GetByIdAsync(id, token);
        }

        public async Task<Employee?> UpdateAsync(Employee employee, CancellationToken token = default)
        {
            await _employeeValidator.ValidateAndThrowAsync(employee, cancellationToken: token);

            var employeeExists = await _employeeRepository.ExistsByIdAsync(employee.Id, token);
            if (!employeeExists)
                return null;

            await _employeeRepository.UpdateAsync(employee, token);
            return employee;
        }
    }
}
