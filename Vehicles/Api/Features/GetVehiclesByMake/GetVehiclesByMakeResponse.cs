using ProtoBuf;
using System.Text.Json.Serialization;
using Vehicles.Models;

namespace Vehicles.Api.Features.GetVehiclesByMake;

[ProtoContract]
public class GetVehiclesByMakeResponse
{
    [JsonPropertyName("vehicles")]
    [ProtoMember(1)]
    public required IEnumerable<Vehicle> Vehicles { get; set; }
}
