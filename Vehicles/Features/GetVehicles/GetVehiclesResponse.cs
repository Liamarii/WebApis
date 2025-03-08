using System.Text.Json.Serialization;

namespace Vehicles.Features.GetVehicles
{
    public class GetVehiclesResponse
    {
        [JsonPropertyName("vehicles")]
        public required IEnumerable<Vehicle> Vehicles { get; set; }
    }
}
