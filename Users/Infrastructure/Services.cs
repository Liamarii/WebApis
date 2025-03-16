using Users.Services.Users;
using Users.Services.Vehicles;

namespace Users.Infrastructure;

public static class Services
{
    public const string baseAddress = "https://localhost:7264";

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddHttpClient<IVehicleService, VehiclesService>(client =>
        {
            client.BaseAddress = new Uri(baseAddress);
        });

        services.AddSingleton<IUsersService, UsersService>();

        return services;
    }
}
