using System.Text.Json.Serialization;
using Vehicles.Models;

namespace Vehicles.Api.Features.GetVehiclesByMake;

public class GetVehiclesByMakeResponse
{
    [JsonPropertyName("vehicles")]
    public required IEnumerable<Vehicle> Vehicles { get; set; }
}
