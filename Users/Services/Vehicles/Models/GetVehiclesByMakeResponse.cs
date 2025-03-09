using System.Text.Json.Serialization;

namespace Users.Services.Vehicles.Models;

public readonly struct GetVehiclesByMakeResponse
{
    [JsonPropertyName("vehicles")]
    public required IEnumerable<Vehicle> Vehicles { get; init; }
}