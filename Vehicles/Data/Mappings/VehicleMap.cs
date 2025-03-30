using FluentNHibernate.Mapping;
using Vehicles.Infrastructure.Persistence.Entities;

namespace Vehicles.Data.Mappings
{
    public class VehicleMap : ClassMap<Vehicle>
    {
        public VehicleMap()
        {
            Table("vehicles");
            Id(x => x.Id).GeneratedBy.Identity(); 
            Map(x => x.Vin).Unique().Not.Nullable();
            Map(x => x.Make).Not.Nullable();
            Map(x => x.Model).Not.Nullable();
            Map(x => x.Year).Not.Nullable();
        }
    }
}
