using MediatR;
using Vehicles.Data;
using Vehicles.Models;

namespace Vehicles.Api.Features.GetVehiclesByMake;

public class GetVehiclesByMakeHandler(IVehiclesRepository vehiclesRepositoryStub) : IRequestHandler<GetVehiclesByMakeRequest, GetVehiclesByMakeResponse>
{
    public async Task<GetVehiclesByMakeResponse> Handle(GetVehiclesByMakeRequest getVehiclesRequest, CancellationToken cancellationToken)
    {
        List<Vehicle> result = await vehiclesRepositoryStub.GetVehicles(cancellationToken);

        return new()
        {
            Vehicles = result.Where(x => x.Make == getVehiclesRequest.Make)
        };
    }
}
