using Microsoft.Extensions.Options;
using System.Reflection;
using Vehicles.Data.Repositories;
using Vehicles.Infrastructure.Configurations;
using Vehicles.Infrastructure.Persistence;

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
        builder.Services.AddConfigurationMapping<DatabaseConfig>("ConnectionStrings");
        builder.Services.AddSingleton<INHibernateDatabase, NHibernateDatabase>(x => new NHibernateDatabase(x.GetRequiredService<IOptions<DatabaseConfig>>(), Assembly.GetExecutingAssembly()));

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