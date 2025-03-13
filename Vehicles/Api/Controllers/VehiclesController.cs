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
    [Produces("application/json", "application/x-protobuf")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<ActionResult> GetVehicles(CancellationToken cancellationToken, [FromHeader] string accept = "application/x-protobuf")
    {
        GetVehiclesResponse response = await mediator.Send(new GetVehiclesRequest(), cancellationToken);
        return accept == "application/x-protobuf" ?
            File(ProtobufHelper.SerialiseToProtobuf(response), "application/x-protobuf") :
            Ok(response);
    }

    [HttpPost("GetVehiclesByMake")]
    [Produces("application/json", "application/x-protobuf")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<ActionResult> GetVehiclesByMake([FromBody, Required] GetVehiclesByMakeRequest getVehiclesByMakeRequest, CancellationToken cancellationToken, [FromHeader] string accept = "application/x-protobuf")
    {
        GetVehiclesByMakeResponse response = await mediator.Send(getVehiclesByMakeRequest, cancellationToken);
        return accept == "application/x-protobuf" ?
            File(ProtobufHelper.SerialiseToProtobuf(response), "application/x-protobuf") :
            Ok(response);
    }
}
