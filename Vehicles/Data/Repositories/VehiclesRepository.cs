using Vehicles.Infrastructure;
using Vehicles.Models;

namespace Vehicles.Data.Repositories;

public interface IVehiclesRepository
{
    public Task<IList<Vehicle>> GetVehiclesAsync(CancellationToken cancellationToken);
    public Task<IList<Vehicle>> GetVehiclesByMakeAsync(string make, CancellationToken cancellationToken);
}

public class VehiclesRepository(INHibernateDatabase nHibernateDatabase) : IVehiclesRepository
{
    public async Task<IList<Vehicle>> GetVehiclesAsync(CancellationToken cancellationToken)
    {
        using NHibernate.ISession session = nHibernateDatabase.OpenSession();
        return await session.QueryOver<Vehicle>()
                            .ListAsync(cancellationToken);
    }

    public async Task<IList<Vehicle>> GetVehiclesByMakeAsync(string make, CancellationToken cancellationToken)
    {
        using NHibernate.ISession session = nHibernateDatabase.OpenSession();
        return await session.QueryOver<Vehicle>()
                            .Where(x => x.Make == make)
                            .ListAsync(cancellationToken);
    }
}

