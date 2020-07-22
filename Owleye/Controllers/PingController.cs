using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Owleye.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PingController : ControllerBase
    {
        private readonly ILogger _log;

        public PingController(ILogger<PingController> logger)
        {
            _log = logger;
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            _log.LogInformation("d");
            return Ok();
        }
    }
}
