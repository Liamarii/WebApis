namespace Vehicles.Infrastructure.Persistence.Entities;

public class Vehicle
{
    public virtual int Id { get; set; }
    public virtual string Vin { get; set; } = string.Empty;
    public virtual string Make { get; set; } = string.Empty;
    public virtual string Model { get; set; } = string.Empty;
    public virtual int Year { get; set; }
}
