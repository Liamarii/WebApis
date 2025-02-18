namespace Vehicles.Features.GetVehicles
{
    public class GetVehiclesResponse
    {
        public required IEnumerable<Vehicle> Vehicles { get; set; }
    }
}
