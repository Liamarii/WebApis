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
    [EndpointDescription("Uses protobuf rest request")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<ActionResult<GetAvailableVehiclesResponse>> GetVehicleByUser([FromBody, Required] GetAvailableVehiclesRequest getVehicleByUserRequest)
    {
        try
        {
            GetAvailableVehiclesResponse response = await usersService.GetAvailableVehicles(getVehicleByUserRequest);
            return Ok(response);
        }
        catch (HttpRequestException ex)
        {
            Logs.Add.InfoLog($"External service is unavailable, {ex}");
            return StatusCode(StatusCodes.Status502BadGateway, "External service is unavailable.");
        }

        catch (Exception ex)
        {
            Logs.Add.ErrorLog($"Unhandled exception {ex}");
            return StatusCode(500, "An internal server error occurred.");
        }
    }
}
