using ProtoBuf;
using System.Text.Json.Serialization;

namespace Users.Services.Vehicles.Models;

[ProtoContract]
public class GetVehiclesByMakeResponse
{
    [JsonPropertyName("vehicles")]
    [ProtoMember(1)]
    public required IEnumerable<Vehicle> Vehicles { get; set; }
}