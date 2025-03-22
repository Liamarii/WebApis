using Users.Infrastructure;
using Users.Infrastructure.FaultHandlers;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        _ = builder.Services
            .AddScalar()
            .AddServices()
            .AddRateLimiting()
            .AddFaultHandling(FaultHandler.ResiliencePipelines)
            .AddControllers();

        var app = builder.Build();

        app.UseHttpsRedirection();

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