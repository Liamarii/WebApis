using Polly;
using Users.Services.Users;
using Users.Services.Vehicles;

namespace Users.Infrastructure;

public static class Services
{
    public static IServiceCollection AddServices(this IServiceCollection services, string vehiclesServiceBase)
    {
        ArgumentNullException.ThrowIfNull(vehiclesServiceBase);
        services.AddHttpClient();
        services.AddSingleton<IVehicleService>(sp =>
        {
            var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = clientFactory.CreateClient(nameof(IVehicleService));
            httpClient.BaseAddress = new Uri(vehiclesServiceBase);
            var pipeline = sp.GetRequiredService<ResiliencePipeline<HttpResponseMessage>>();
            return new VehiclesService(httpClient, pipeline);
        });

        services.AddSingleton<IUsersService, UsersService>();
        return services;
    }
}
