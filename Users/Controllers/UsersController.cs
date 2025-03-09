using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Users.Infrastructure;
using Users.Services.Users;
using Users.Services.Users.Models;

namespace Users.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(IUsersService usersService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<ActionResult<GetAvailableVehiclesResponse>> GetVehicleByUser([FromBody, Required] GetAvailableVehiclesRequest getVehicleByUserRequest)
    {
        Logs.Add.InfoLog("This is an info log");
        Logs.Add.ErrorLog("This is an error log");

        GetAvailableVehiclesResponse response = await usersService.GetAvailableVehicles(getVehicleByUserRequest);
        
        return Ok(response);
    }
}
