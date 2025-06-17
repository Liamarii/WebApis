using Polly;
using Users.Infrastructure;

namespace Users;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1102:Make class static", Justification = "Not static for the WabApplicationFactory tests")]
public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        _ = builder.Services
            .AddCorsPolicies(builder.Configuration)
            .AddScalar()
            .AddServices()
            .AddRateLimiting(window: TimeSpan.FromSeconds(10), permitLimit: 3)
            .AddResiliencePipeline("defaultPipeline", (ResiliencePipelineBuilder<HttpResponseMessage> rpb) => ResiliencePipelineProvider.ConfigureDefaultPipeline(rpb))
            .AddControllers();

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseCorsPolicies();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseScalar();
        }

        app.UseAuthorization();
        app.MapControllers();
        app.UseRateLimiter();
        app.Run();
    }
}