using Vehicles.Data;
using Vehicles.Enums;

namespace Vehicles;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(Program).Assembly));
        builder.Services.AddControllers();
        builder.Services.AddSingleton<IVehiclesRepository, VehiclesRepository>();

        string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        bool unknownEnvironment = Enum.TryParse(environment, true, out EnvironmentTypes environmentType) == false;
        if (unknownEnvironment)
        {
            throw new InvalidOperationException($"Unknown environment: {environment}.");
        }

        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = config.GetConnectionString("DefaultConnection");

        var app = builder.Build();
        app.UseHttpsRedirection();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(x => x.EnableTryItOutByDefault());
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}