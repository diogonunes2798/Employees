using Employees.Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Application.Validators
{
    public class GetAllEmployeesOptionValidator : AbstractValidator<GetAllEmployeesOptions>
    {
        private static readonly string[] AcceptableSortFields =
        {
            "Name", "YearsOfExperience"
        };

        public GetAllEmployeesOptionValidator()
        {

            RuleFor(x => x.DateOfBirth)
                   .Must(date => !date.HasValue || (date.Value.Year > 1925 && date.Value.Year <= DateTime.UtcNow.Year));

            RuleFor(x => x.YearsOfExperience)
                .InclusiveBetween(0, 60);

            RuleFor(x => x.SortField)
                .Must(x => x is null || AcceptableSortFields.Contains(x, StringComparer.OrdinalIgnoreCase))
                .WithMessage("You can only sort by 'Name' or 'YearOfEmperience'");
        }
    }
}
