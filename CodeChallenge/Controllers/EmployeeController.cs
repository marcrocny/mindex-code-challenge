using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            _logger.LogDebug("Received employee create request for '{FirstName} {LastName}'", employee.FirstName, employee.LastName);

            _employeeService.Create(employee);

            return CreatedAtRoute("getEmployeeById", new { id = employee.EmployeeId }, employee);
        }

        [HttpGet("{id}", Name = "getEmployeeById")]
        public IActionResult GetEmployeeById(string id)
        {
            _logger.LogDebug("Received employee get request for '{Id}'", id);

            var employee = _employeeService.GetById(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(string id, [FromBody]Employee employeeInfo)
        {
            _logger.LogDebug("Recieved employee update request for '{Id}'", id);

            employeeInfo.EmployeeId = id;
            var updated = _employeeService.Update(employeeInfo);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }
    }
}
