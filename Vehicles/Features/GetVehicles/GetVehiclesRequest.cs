using MediatR;

namespace Vehicles.Features.GetVehicles
{
    public class GetVehiclesRequest : IRequest<GetVehiclesResponse>
    {
        public required string Make { get; set; }
    }
}
