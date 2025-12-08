using Dapper;
using Employees.Application.Database;
using Employees.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Application.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public EmployeeRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<bool> CreateAsync(Employee employee, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition("""
                INSERT INTO employees (id, name, dateofbirth, yearsofexperience)
                VALUES (@Id, @Name, @DateOfBirth, @YearsOfExperience)
                """, employee, cancellationToken: token));

            if (result > 0 && employee.Technologies.Any())
            {
                foreach (var tech in employee.Technologies)
                {
                    await connection.ExecuteAsync(new CommandDefinition("""
                        INSERT INTO technologies (employeeId, name)
                        VALUES (@EmployeeId, @Name)
                        """, new { EmployeeId = employee.Id, Name = tech }, cancellationToken: token
                        ));
                }
            }

            transaction.Commit();

            return result > 0;
        }
        
        public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            await connection.ExecuteAsync(new CommandDefinition("""
                DELETE FROM technologies WHERE employeeId = @id
                """, new { id }, cancellationToken: token));

            var result = await connection.ExecuteAsync(new CommandDefinition("""
                DELETE FROM employees WHERE id = @id
                """, new { id }, cancellationToken: token));

            transaction.Commit();

            return result > 0;
        }

        public async Task<bool> ExistsAsync(string name, DateTime dateOfBirth, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

            var exists = await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
                SELECT COUNT(1)
                FROM employees
                WHERE name = @name AND dateofbirth = @dateOfBirth
                """, new { name, dateOfBirth }, cancellationToken: token));

            return exists;
        }


        public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

            return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
                SELECT COUNT(1) FROM employees WHERE id = @id
                """, new { id }, cancellationToken: token));
        }

        public async Task<IEnumerable<Employee>> GetAllAsync(GetAllEmployeesOptions options, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

            var orderClause = string.Empty;
            if (options.SortField is not null)
            {
                orderClause = $"ORDER BY e.{options.SortField} {(options.SortOrder == SortOrder.Ascending ? "ASC" : "DESC")}";
            }


            var result = await connection.QueryAsync(new CommandDefinition($"""
                SELECT e.*, GROUP_CONCAT(t.name, ',') AS technologies
                FROM employees e
                LEFT JOIN technologies t ON e.id = t.employeeId
                WHERE (@name IS NULL OR e.name LIKE ('%' || @name || '%'))
                  AND (@dateOfBirth IS NULL OR e.dateofbirth = @dateOfBirth)
                  AND (@yearsOfExperience IS NULL OR e.yearsofexperience = @yearsOfExperience)
                GROUP BY e.id 
                {orderClause}
                """, new
                {
                    name = options.Name,
                    dateOfBirth = options.DateOfBirth?.ToString("yyyy-MM-dd"),
                    yearsOfExperience = options.YearsOfExperience
                }, cancellationToken: token));

            return result.Select(x => new Employee
            {
                Id = Guid.Parse((string)x.id),
                Name = x.name,
                DateOfBirth = DateTime.Parse(x.dateofbirth),
                YearsOfExperience = (int)x.yearsofexperience,
                Technologies = string.IsNullOrEmpty(x.technologies)
                    ? new List<string>()
                    : ((string)x.technologies).Split(',').ToList()
            });
        }

        public async Task<Employee?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

            var employeeData = await connection.QuerySingleOrDefaultAsync<dynamic>(new CommandDefinition("""
                        SELECT * FROM employees WHERE id = @id
                        """, new { id }, cancellationToken: token));

            if (employeeData is null)
                return null;

            var technologies = await connection.QueryAsync<string>(new CommandDefinition("""
                                    SELECT name FROM technologies WHERE employeeId = @id
                                    """, new { id }, cancellationToken: token));

            return new Employee
            {
                Id = Guid.Parse((string)employeeData.id),
                Name = employeeData.name,
                DateOfBirth = DateTime.Parse((string)employeeData.dateofbirth),
                YearsOfExperience = (int)employeeData.yearsofexperience,
                Technologies = technologies.ToList()
            };
        }

        public async Task<bool> UpdateAsync(Employee employee, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            await connection.ExecuteAsync(new CommandDefinition("""
                DELETE FROM technologies WHERE employeeId = @id
                """, new { id = employee.Id }, cancellationToken: token, transaction: transaction));

            foreach (var tech in employee.Technologies)
            {
                await connection.ExecuteAsync(new CommandDefinition("""
                    INSERT INTO technologies (employeeId, name)
                    VALUES (@EmployeeId, @Name)
                    """, new { EmployeeId = employee.Id, Name = tech }, cancellationToken: token));
            }

            var result = await connection.ExecuteAsync(new CommandDefinition("""
                UPDATE employees
                SET name = @Name,
                dateofbirth = @DateOfBirth,
                yearsofexperience = @YearsOfExperience
                WHERE id = @Id
                """, employee, cancellationToken: token));

            transaction.Commit();

            return result > 0;
        }
    }
}
