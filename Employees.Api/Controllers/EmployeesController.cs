using Employees.Api.Mapping;
using Employees.Application.Services;
using Employees.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Api.Controllers
{
   
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest request, CancellationToken token)
        {
            var employee = request.MapToMovie();

            await _employeeService.CreateAsync(employee, token);

            return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken token)
        {
            var employee = await _employeeService.GetByIdAsync(id, token);

            if (employee is null)
                return NotFound();

            return Ok(employee.MapToResponse());
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllEmployeesRequest request, CancellationToken token)
        {
            var options = request.MapToOptions();

            var employees = await _employeeService.GetAllAsync(options, token);

            return Ok(employees.MapToResponse());
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateEmployeeRequest request, CancellationToken token)
        {
            var employee = request.MapToEmployee(id);

            var updatedEmployee = await _employeeService.UpdateAsync(employee, token);

            if (updatedEmployee is null)
                return NotFound();

            return Ok(employee.MapToResponse());
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            var deleted = await _employeeService.DeleteByIdAsync(id, token);

            if (!deleted)
                return NotFound();

            return Ok();
        }
    }
}
