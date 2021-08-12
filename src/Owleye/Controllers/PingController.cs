using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Owleye.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PingController : ControllerBase
    {

        [HttpGet]
        [SwaggerOperation(
            Summary = "Ping endpoint for availability status ",
            Description = "api availability check",
            OperationId = "Ping.Get",
            Tags = new[] { "HealthCheckEndpoints" })
        ]
        public IActionResult Get()
        {
            return Ok(":)");
        }
    }
}
