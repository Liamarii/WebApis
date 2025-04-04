namespace Users.Infrastructure
{
    public static class CorsPolicies
    {
        public static IServiceCollection AddCorsPolicies(this IServiceCollection services, string? angularUIOrigin)
        {
            ArgumentNullException.ThrowIfNull(angularUIOrigin);

            services.AddCors(options =>
            {
                options.AddPolicy("StopBlockingMyAngularUI",
                policy => policy
                    .WithOrigins(angularUIOrigin)
                    .WithMethods("POST")
                    .AllowAnyHeader());
            });

            return services;
        }

        public static WebApplication UseCorsPolicies(this WebApplication app)
        {
            app.UseCors("StopBlockingMyAngularUI");
            return app;
        }
    }
}
