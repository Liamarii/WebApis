using Users.Services.Users;
using Users.Services.Vehicles;

namespace Users.Infrastructure;

public static class Services
{
    public static IServiceCollection AddServices(this IServiceCollection services, string vehiclesServiceBase)
    {
        ArgumentNullException.ThrowIfNull(vehiclesServiceBase);

        services.AddHttpClient<IVehicleService, VehiclesService>(client => client.BaseAddress = new Uri(vehiclesServiceBase));

        services.AddSingleton<IUsersService, UsersService>();

        return services;
    }
}
