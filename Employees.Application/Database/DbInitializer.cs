using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Application.Database
{
    public class DbInitializer
    {

        private readonly IDbConnectionFactory _dbConnectionFactory;

        public DbInitializer(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task InitializeAsync()
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();

            await connection.ExecuteAsync("""
                CREATE TABLE IF NOT EXISTS employees (
                    id TEXT PRIMARY KEY,
                    name TEXT NOT NULL,
                    dateofbirth TEXT NOT NULL,
                    yearsofexperience INTEGER NOT NULL
                );
            """);

            await connection.ExecuteAsync("""
                CREATE TABLE IF NOT EXISTS technologies (
                    employeeId TEXT NOT NULL,
                    name TEXT NOT NULL,
                    FOREIGN KEY (employeeId) REFERENCES employees(id)
                );
            """);

        }
    }
}
