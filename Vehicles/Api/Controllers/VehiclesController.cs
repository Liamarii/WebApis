using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Vehicles.Api.Features.GetVehicles;
using Vehicles.Api.Features.GetVehiclesByMake;

namespace Vehicles.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehiclesController(IMediator mediator) : ControllerBase
{
    [HttpGet("GetVehicles")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<ActionResult<GetVehiclesResponse>> GetVehicles()
    {
        return await mediator.Send(new GetVehiclesRequest());
    }

    [HttpPost("GetVehiclesByMake")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<ActionResult<GetVehiclesByMakeResponse>> GetVehiclesByMake([FromBody, Required] GetVehiclesByMakeRequest getVehiclesByMakeRequest)
    {
        return await mediator.Send(getVehiclesByMakeRequest);
    }
}
