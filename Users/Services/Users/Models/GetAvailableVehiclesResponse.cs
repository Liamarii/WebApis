using Users.Services.Vehicles.Models;

namespace Users.Services.Users.Models;

public class GetAvailableVehiclesResponse(string name, Vehicle vehicle)
{
    public string Response => $"{name} drives a {vehicle.Make} {vehicle.Model}";
}
