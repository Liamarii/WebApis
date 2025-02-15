using Microsoft.AspNetCore.Mvc;

namespace Vehicles.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Vehicles : ControllerBase
    { 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public ActionResult<string> Get()
        {
            return Ok("Car 1");
        }
    }
}
