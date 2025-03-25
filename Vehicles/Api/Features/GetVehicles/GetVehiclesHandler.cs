using MediatR;
using Vehicles.Data.Repositories;
using Vehicles.Dtos;

namespace Vehicles.Api.Features.GetVehicles;

public class GetVehiclesHandler(IVehiclesRepository vehiclesRepository) : IRequestHandler<GetVehiclesRequest, GetVehiclesResponse>
{
    public async Task<GetVehiclesResponse> Handle(GetVehiclesRequest getVehiclesRequest, CancellationToken cancellationToken)
    {
        var vehicles = await vehiclesRepository.GetVehiclesAsync(cancellationToken);
        return new()
        {
            Vehicles = vehicles
            .Select(v => new VehicleDto().FromVehicle(v))
        };
    }
}
