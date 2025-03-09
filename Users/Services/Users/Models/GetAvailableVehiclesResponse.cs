using Users.Services.Vehicles.Models;

namespace Users.Services.Users.Models;

public class GetAvailableVehiclesResponse(string name, Vehicle vehicle)
{
    public string Response => $"{name} drivers a {vehicle.Make} {vehicle.Model}";
}
