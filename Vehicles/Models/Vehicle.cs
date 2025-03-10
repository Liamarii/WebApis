using ProtoBuf;
using System.Text.Json.Serialization;

namespace Vehicles.Models;

[ProtoContract]
public class Vehicle
{
    [ProtoMember(1)]
    [JsonPropertyName("make")]
    public required string Make { get; init; }

    [ProtoMember(2)]
    [JsonPropertyName("model")]
    public required string Model { get; init; }
}
