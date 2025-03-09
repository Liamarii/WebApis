using System.ComponentModel.DataAnnotations;

namespace Users.Services.Users.Models;

public class GetAvailableVehiclesRequest
{
    [Required]
    public required string Name { get; init; }
}
