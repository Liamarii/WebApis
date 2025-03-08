using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Vehicles.Features.GetVehicles;

[Route("api/[controller]")]
[ApiController]
public class GetVehiclesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<ActionResult<GetVehiclesResponse>> GetVehicle([FromBody, Required] GetVehiclesRequest getVehiclesRequest, IMediator mediator)
    {
        return await mediator.Send(getVehiclesRequest);
    }
}
