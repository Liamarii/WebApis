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

        var app = builder.Build();

        app.UseHttpsRedirection();

        if (app.Environment.IsDevelopment())
        {
            app.UseScalar();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}