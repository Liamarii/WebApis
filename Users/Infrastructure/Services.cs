using Polly.Registry;
using Users.Services.Users;
using Users.Services.Vehicles;

namespace Users.Infrastructure;

public static class Services
{
    public static IServiceCollection AddServices(this IServiceCollection services, string vehiclesServiceBase)
    {
        ArgumentNullException.ThrowIfNull(vehiclesServiceBase);

        services.AddHttpClient(nameof(IVehicleService), client => client.BaseAddress = new Uri(vehiclesServiceBase));
        services.AddScoped<IVehicleService, VehiclesService>(sp =>
        {
            var resiliencePipelineProvider = sp.GetRequiredService<ResiliencePipelineProvider<string>>();
            var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(IVehicleService));
            return new VehiclesService(httpClient, resiliencePipelineProvider);
        });

        services.AddScoped<IUsersService, UsersService>();
        return services;
    }
}
