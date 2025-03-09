using System.Text.Json.Serialization;

namespace Vehicles.Models;

public class Vehicle(string make, string model)
{
    [JsonPropertyName("make")]
    public string Make { get; init; } = make;

    [JsonPropertyName("model")]
    public string Model { get; init; } = model;
}
