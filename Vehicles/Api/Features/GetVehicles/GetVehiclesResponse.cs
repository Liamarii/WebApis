using System.Text.Json.Serialization;
using Vehicles.Dtos;

namespace Vehicles.Api.Features.GetVehicles;

public class GetVehiclesResponse
{
    [JsonPropertyName("vehicles")]
    public required IEnumerable<VehicleDto> Vehicles { get; set; }
}
