using ExpenseTracker.Services.Mongo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("api/v1")]
    [Authorize]
    public class HealthController : ControllerBase
    {

        private readonly IMongoDBClientService _mongoDBClientService;
        public HealthController(IMongoDBClientService mongoDBClientService)
        {
            _mongoDBClientService = mongoDBClientService;
        }

        [HttpGet]
        [Route("health")]
        public IActionResult Health()
        {
            var healthStatus = _mongoDBClientService.HealthCheck();
            return Ok(new { status = healthStatus });
            //manaje network errors
            //user auth errors
            //too many open connections
        }
    }
}
