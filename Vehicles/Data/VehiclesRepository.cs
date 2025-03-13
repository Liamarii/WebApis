using Vehicles.Models;

namespace Vehicles.Data;

public interface IVehiclesRepository
{
    public Task<List<Vehicle>> GetVehicles(CancellationToken cancellationToken);
}

public class VehiclesRepository : IVehiclesRepository
{        
    public async Task<List<Vehicle>> GetVehicles(CancellationToken cancellationToken)
    {
        return await StubbedVehicleData(cancellationToken);
    }

    private static async Task<List<Vehicle>> StubbedVehicleData(CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);

        return [
        new Vehicle(){ Make = "Hyundai", Model = "Zebra"},
        new Vehicle() { Make = "Volvo", Model = "Skunk" },
        new Vehicle() { Make = "Kia", Model = "Chimp" },
        new Vehicle() { Make = "Toyota", Model = "Ape" }];
    }
}
