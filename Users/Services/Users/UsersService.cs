using Users.Services.Users.Models;
using Users.Services.Vehicles;
using Users.Services.Vehicles.Models;

namespace Users.Services.Users;

public interface IUsersService
{
    public Task<GetAvailableVehiclesResponse> GetAvailableVehicles(GetAvailableVehiclesRequest request, CancellationToken cancellationToken);
}

public class UsersService(IVehicleService vehicleService) : IUsersService
{
    public async Task<GetAvailableVehiclesResponse> GetAvailableVehicles(GetAvailableVehiclesRequest request, CancellationToken cancellationToken)
    {
        string vehicleMake = DetermineVehicleMake();
        GetVehiclesByMakeResponse getVehiclesByMakeResponse = await vehicleService.GetVehiclesByMake(new GetVehiclesByMakeRequest { Make = vehicleMake }, cancellationToken);
        return new GetAvailableVehiclesResponse(request.Name, getVehiclesByMakeResponse.Vehicles.Single());
    }

    private static string DetermineVehicleMake()
    {
        string[] makes = ["Honda", "Ford", "Nissan", "Jeep","Tesla"];
        int randomMakesIndex = new Random().Next(0, makes.Length - 1);
        return makes[randomMakesIndex];
    }
}