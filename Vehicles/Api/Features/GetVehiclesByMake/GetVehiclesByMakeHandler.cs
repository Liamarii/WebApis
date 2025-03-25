using MediatR;
using Vehicles.Data.Repositories;
using Vehicles.Dtos;

namespace Vehicles.Api.Features.GetVehiclesByMake;

public class GetVehiclesByMakeHandler(IVehiclesRepository vehiclesRepository) : IRequestHandler<GetVehiclesByMakeRequest, GetVehiclesByMakeResponse>
{
    public async Task<GetVehiclesByMakeResponse> Handle(GetVehiclesByMakeRequest getVehiclesRequest, CancellationToken cancellationToken)
    {
        var vehicles = await vehiclesRepository.GetVehiclesByMakeAsync(getVehiclesRequest.Make, cancellationToken);
        return new()
        {
            Vehicles = vehicles.Select(v => new VehicleDto().FromVehicle(v))
        };
    }
}
