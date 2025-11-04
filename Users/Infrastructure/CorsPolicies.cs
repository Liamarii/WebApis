namespace Users.Infrastructure;

public static class CorsPolicies
{
    public static IServiceCollection AddCorsPolicies(this IServiceCollection services, IConfiguration configuration)
    {
        var angularUIOrigin = configuration.GetSection("Cors")["AngularOrigin"] ?? throw new InvalidOperationException("Cors:AngularOrigin");

        services.AddCors(options =>
        {
            options.AddPolicy("StopBlockingMyAngularUI",
            policy => policy
                .WithOrigins(angularUIOrigin)
                .WithMethods("POST")
                .AllowAnyHeader()
                .SetPreflightMaxAge(TimeSpan.FromMinutes(5)));
        });

        return services;
    }

    public static WebApplication UseCorsPolicies(this WebApplication app)
    {
        app.UseCors("StopBlockingMyAngularUI");
        return app;
    }
}
