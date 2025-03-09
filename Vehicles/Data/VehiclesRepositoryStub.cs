using Vehicles.Models;

namespace Vehicles.Data;

public interface IVehiclesRepositoryStub
{
    public Task<List<Vehicle>> GetVehicleDataAsync();
}

public class VehiclesRepositoryStub : IVehiclesRepositoryStub
{        
    public async Task<List<Vehicle>> GetVehicleDataAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(3));
        
        return [
        new Vehicle("Hyundai", "Zebra"),
        new Vehicle("Volvo", "Skunk"),
        new Vehicle("Kia", "Chimp"),
        new Vehicle("Toyota", "Ape")];
    }
}
