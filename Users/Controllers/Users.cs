using Microsoft.AspNetCore.Mvc;
using Users.Infrastructure;

namespace Users.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Users : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public ActionResult<string> Get()
        {
            Logs.Add.InfoLog("This is an info log");
            Logs.Add.ErrorLog("This is an error log");
            return Ok("User 1");
        }
    }
}
