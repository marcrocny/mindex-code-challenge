using CodeChallenge.Models;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/employee/{id}/compensation")]
    public class CompensationController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _compensationService = compensationService;
            _logger = logger;
        }

        [HttpPut()]
        public async Task<IActionResult> Create(string id, [FromBody] CompensationModel compensationModel)
        {
            _logger.LogDebug("Received compensation create request for '{EmployeeId}'", id);

            // should standardize error format
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (compensationModel.Validate().Any()) return BadRequest(compensationModel.Validate());
            
            var result = await _compensationService.AddOrUpdate(id, compensationModel);
            if (result == null)
                return NotFound();
            
            return Ok(CompensationModel.Map(result));
        }

        [HttpGet()]
        public async Task<IActionResult> List(string id)
        {
            _logger.LogDebug("Received compensation list request for '{EmployeeId}'", id);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _compensationService.ListByEmployeeId(id);
            if (result == null) return NotFound();

            return Ok(result.Select(c => CompensationModel.Map(c)));
        }
    }
}
