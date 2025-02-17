using Scalar.AspNetCore;

namespace Users.Infrastructure
{
    public static class ScalarUI
    {
        public static IServiceCollection AddScalar(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddOpenApi();
            return services;
        }

        public static void UseScalar(this WebApplication app)
        {
            app.MapOpenApi();
            app.MapScalarApiReference(x =>
            {
                x.WithTitle("Users Api");
                x.WithTheme(ScalarTheme.BluePlanet);
                x.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.Http);
            });
        }
    }
}