using System.Text.Json.Serialization;
using Vehicles.Models;

namespace Vehicles.Api.Features.GetVehicles;

public class GetVehiclesResponse
{
    [JsonPropertyName("vehicles")]
    public required IEnumerable<Vehicle> Vehicles { get; set; }
}
