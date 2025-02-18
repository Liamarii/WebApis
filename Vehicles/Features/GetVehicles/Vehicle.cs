namespace Vehicles.Features.GetVehicles
{
    public readonly struct Vehicle(string make, string model)
    {
        public string Make { get; init; } = make;

        public string Model { get; init; } = model;
    }
}
