using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Vehicles.Api.Features.GetVehicles;
using Vehicles.Api.Features.GetVehiclesByMake;
using Vehicles.Infrastructure.Serialisation;

namespace Vehicles.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehiclesController(IMediator mediator) : ControllerBase
{
    private const string _defaultResponseType = "application/x-protobuf";

    [HttpGet("GetVehicles")]
    [Produces("application/json", _defaultResponseType)]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<ActionResult> GetVehicles(CancellationToken cancellationToken, [FromHeader] string accept = _defaultResponseType)
    {
        GetVehiclesResponse response = await mediator.Send(new GetVehiclesRequest(), cancellationToken);
        return accept == _defaultResponseType ?
            File(ProtobufHelper.SerialiseToProtobuf(response), _defaultResponseType) :
            Ok(response);
    }

    [HttpPost("GetVehiclesByMake")]
    [Produces("application/json", _defaultResponseType)]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<ActionResult> GetVehiclesByMake([FromBody, Required] GetVehiclesByMakeRequest getVehiclesByMakeRequest, CancellationToken cancellationToken, [FromHeader] string accept = _defaultResponseType)
    {
        GetVehiclesByMakeResponse response = await mediator.Send(getVehiclesByMakeRequest, cancellationToken);
        return accept == _defaultResponseType ?
            File(ProtobufHelper.SerialiseToProtobuf(response), _defaultResponseType) :
            Ok(response);
    }
}
