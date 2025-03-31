using Users.Infrastructure;

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

        var policyName = "StopBlockingMe";

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(policyName,
                policy => policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseCors(policyName);

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