using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Vehicles.Api.Features;
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
    [Produces("application/json", "application/x-protobuf")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<ActionResult> GetVehiclesByMake([FromBody, Required] GetVehiclesByMakeRequest getVehiclesByMakeRequest, [FromHeader] string accept = "application/json")
    {
        GetVehiclesByMakeResponse response = await mediator.Send(getVehiclesByMakeRequest);

        return accept == "application/x-protobuf" ? 
            File(ProtobufHelper.SerialiseToProtobuf(response), "application/x-protobuf") : 
            Ok(response);
    }
}
