using Users.Infrastructure;

namespace Users;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var config = builder.Configuration;

        _ = builder.Services
            .AddCorsPolicies(config.GetSection("Cors")["AngularOrigin"])
            .AddScalar()
            .AddServices(config.GetSection("Services")["VehiclesService"])
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