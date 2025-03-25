using ProtoBuf;
using System.Text.Json.Serialization;
using Vehicles.Models;

namespace Vehicles.Dtos;

[ProtoContract]
public class VehicleDto
{
    [ProtoMember(1)]
    [JsonPropertyName("make")]
    public string Make { get; init; } = string.Empty;

    [ProtoMember(2)]
    [JsonPropertyName("model")]
    public string Model { get; init; } = string.Empty;

    public VehicleDto FromVehicle(Vehicle vehicle)
    {
        return new VehicleDto()
        {
            Make = vehicle.Make,
            Model = vehicle.Model
        };
    }
}
