using Users.Infrastructure;

namespace Users;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var config = builder.Configuration;

        var angularOrigin = config.GetSection("Cors")["AngularOrigin"] ?? throw new InvalidOperationException("Cors:AngularOrigin");
        var vehiclesServiceBase = config.GetSection("Services")["VehiclesService"] ?? throw new InvalidOperationException("Services:VehiclesService");

        _ = builder.Services
            .AddCorsPolicies(angularOrigin)
            .AddScalar()
            .AddServices(vehiclesServiceBase)
            .AddRateLimiting(window: TimeSpan.FromSeconds(10), permitLimit: 3)
            .AddFaultHandling(FaultHandler.ResiliencePipelines);

        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
        builder.Services.AddControllers();

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseCorsPolicies();

        if (app.Environment.IsDevelopment())
        {
            app.UseScalar();
        }

        app.UseAuthorization();
        app.MapControllers();
        app.UseRateLimiter();
        app.Run();
    }
}