using Microsoft.AspNetCore.Mvc;

namespace Owleye.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PingController : ControllerBase
    {
        public PingController()
        {
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            return Ok();
        }
    }
}
