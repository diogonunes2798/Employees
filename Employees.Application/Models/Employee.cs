using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Application.Models
{
    public class Employee
    {
        public required Guid Id { get; init; }
        public required string Name { get; set; }
        public required DateTime DateOfBirth { get; set; }

        //private DateTime _dateOfBirth;

        //public DateTime DateOfBirth
        //{
        //    get => _dateOfBirth;
        //    init => _dateOfBirth = value.Date;
        //}

        public required int YearsOfExperience { get; set; }
        public required List<string> Technologies { get; init; } = new();
    }
}
