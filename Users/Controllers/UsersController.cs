using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Users.Exceptions;
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAvailableVehiclesResponse))]
    [ProducesResponseType(StatusCodes.Status502BadGateway, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<ActionResult<GetAvailableVehiclesResponse>> GetVehicleByUser([FromBody, Required] GetAvailableVehiclesRequest getVehicleByUserRequest)
    {
        try
        {
            GetAvailableVehiclesResponse response = await usersService.GetAvailableVehicles(getVehicleByUserRequest);
            return Ok(response);
        }

        catch (ServiceUnavailableException)
        {
            return StatusCode(StatusCodes.Status502BadGateway, "External service is unavailable.");
        }

        catch (Exception ex)
        {
            Logs.Add.ErrorLog($"Exception surfaced in: {this} method: {nameof(GetVehicleByUser)} of: {ex}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred while processing your request.");
        }
    }
}
