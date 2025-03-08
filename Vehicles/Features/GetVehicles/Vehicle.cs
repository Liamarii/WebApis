using System.Text.Json.Serialization;

namespace Vehicles.Features.GetVehicles
{
    public readonly struct Vehicle(string make, string model)
    {
        [JsonPropertyName("make")]
        public string Make { get; init; } = make;

        [JsonPropertyName("model")]
        public string Model { get; init; } = model;
    }
}
