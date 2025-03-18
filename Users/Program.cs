using Users.Infrastructure;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;
        services.AddScalar();
        services.AddControllers();
        services.AddServices();
        services.AddRateLimiting();
        services.AddSingleton<IFaultHandlingPolicies>(provider => new FaultHandlingPolicies(3));
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