using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Users.Services.Vehicles;
using WireMock.Server;
using WireMock.Settings;

namespace UsersTests.Integration.Api.WireMockTests;

public class WireMockFixture : IDisposable
{
    public WireMockServer Server { get; }
    public string BaseUrl => Server.Urls[0];

    public WireMockFixture()
    {
        Server = WireMockServer.Start(new WireMockServerSettings { Port = 0 });
    }

    public HttpClient CreateClient<TProgram>(WebApplicationFactory<TProgram> factory) where TProgram : class
    {
        return factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IVehiclesService));
                if (descriptor != null) services.Remove(descriptor);

                services.AddHttpClient<IVehiclesService, VehiclesService>(c =>
                {
                    c.BaseAddress = new Uri(BaseUrl);
                });
            });
        }).CreateClient();
    }

    public void Dispose() => Server.Stop();
}