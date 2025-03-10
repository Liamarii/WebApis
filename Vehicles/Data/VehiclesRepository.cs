using Vehicles.Models;

namespace Vehicles.Data;

public interface IVehiclesRepository
{
    public Task<List<Vehicle>> GetVehicles();
}

public class VehiclesRepository : IVehiclesRepository
{        
    public async Task<List<Vehicle>> GetVehicles()
    {
        return await StubbedVehicleData();
    }

    private static async Task<List<Vehicle>> StubbedVehicleData()
    {
        await Task.Delay(TimeSpan.FromSeconds(3));

        return [
        new Vehicle(){ Make = "Hyundai", Model = "Zebra"},
        new Vehicle() { Make = "Volvo", Model = "Skunk" },
        new Vehicle() { Make = "Kia", Model = "Chimp" },
        new Vehicle() { Make = "Toyota", Model = "Ape" }];
    }
}
