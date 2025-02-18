using MediatR;

namespace Vehicles.Features.GetVehicles
{
    public class GetVehiclesHandler : IRequestHandler<GetVehiclesRequest, GetVehiclesResponse>
    {
        public Task<GetVehiclesResponse> Handle(GetVehiclesRequest getVehiclesRequest, CancellationToken cancellationToken)
        {
            GetVehiclesResponse getVehiclesResponse = new()
            {
                Vehicles =[
                    new(getVehiclesRequest.Make, "Zebra"),
                    new(getVehiclesRequest.Make, "Skunk"),
                    new(getVehiclesRequest.Make, "Chimp")]
            };

            return Task.FromResult(getVehiclesResponse);
        }
    }
}
