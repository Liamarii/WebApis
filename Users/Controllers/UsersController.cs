using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
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
    [EnableRateLimiting("SlidingWindowPolicy")]
    [EndpointDescription("Returns a protocol buffered rest response from Vehicles")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAvailableVehiclesResponse))]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status502BadGateway, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<ActionResult<GetAvailableVehiclesResponse>> GetVehicleByUser([FromBody, Required] GetAvailableVehiclesRequest getVehicleByUserRequest)
    {
        try
        {
            GetAvailableVehiclesResponse response = await usersService.GetAvailableVehicles(getVehicleByUserRequest);
            return Ok(response);
        }

        catch (ServiceUnavailableException)
        {
            return Problem(
                statusCode: StatusCodes.Status502BadGateway,
                title: "Bad Gateway",
                detail: "External service is unavailable."
            );
        }

        catch (Exception ex)
        {
            Logs.Add.ErrorLog($"Exception surfaced in: {this} method: {nameof(GetVehicleByUser)} of: {ex}");
            
            return Problem(
               statusCode: StatusCodes.Status500InternalServerError,
               title: "Internal server error",
               detail: "An internal server error occurred while processing your request."
           );
        }
    }
}
