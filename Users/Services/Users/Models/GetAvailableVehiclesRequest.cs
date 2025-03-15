using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Users.Services.Users.Models;

public class GetAvailableVehiclesRequest
{
    private string _name = string.Empty;

    [Required]
    public required string Name {
        get => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_name);
        init => _name = value;
    }
}