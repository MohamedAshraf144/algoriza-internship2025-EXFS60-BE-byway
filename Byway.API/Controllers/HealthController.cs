using Microsoft.AspNetCore.Mvc;

namespace Byway.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new 
            { 
                status = "Healthy",
                timestamp = DateTime.UtcNow,
                version = "1.0.0",
                environment = "Production"
            });
        }
    }
}
