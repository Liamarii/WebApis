using MediatR;
using Vehicles.Data;
using Vehicles.Models;

namespace Vehicles.Api.Features.GetVehicles;

public class GetVehiclesHandler(IVehiclesRepository vehiclesRepositoryStub) : IRequestHandler<GetVehiclesRequest, GetVehiclesResponse>
{
    public async Task<GetVehiclesResponse> Handle(GetVehiclesRequest getVehiclesRequest, CancellationToken cancellationToken)
    {
        List<Vehicle> result = await vehiclesRepositoryStub.GetVehicles();

        return new()
        {
            Vehicles = result
        };
    }
}
