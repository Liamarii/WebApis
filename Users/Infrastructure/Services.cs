using Users.Services.Users;
using Users.Services.Vehicles;

namespace Users.Infrastructure;

public static class Services
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddHttpClient<IVehiclesService, VehiclesService>((sp, client) => {
            var vehiclesService = sp
            .GetRequiredService<IConfiguration>()
            .GetSection("Services")["VehiclesService"] ?? throw new InvalidOperationException("Services:VehiclesService");
            
            client.BaseAddress = new Uri(vehiclesService);
        });

        services.AddScoped<IUsersService, UsersService>();

        return services;
    }
}