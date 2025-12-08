using Employees.Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Application.Validators
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator() 
        {

            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .Must(date => date.Year > 1925 && date.Year <= DateTime.UtcNow.Year);

            RuleFor(x => x.YearsOfExperience)
                .InclusiveBetween(0, 60);

        }
    }
}
