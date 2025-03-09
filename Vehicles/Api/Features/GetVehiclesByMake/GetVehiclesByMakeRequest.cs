using MediatR;

namespace Vehicles.Api.Features.GetVehiclesByMake;

public class GetVehiclesByMakeRequest : IRequest<GetVehiclesByMakeResponse>
{
    public required string Make { get; set; }
}
