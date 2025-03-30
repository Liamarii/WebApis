namespace Vehicles.Infrastructure.Configurations
{
    public static class ConfigManager
    {
        private static readonly string? _environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        private static readonly Lazy<IConfiguration> _configuration = new(BuildConfiguration);

        private static IConfiguration BuildConfiguration()
        {
            if (string.IsNullOrWhiteSpace(_environment))
            {
                throw new InvalidOperationException("Environment variable ASPNETCORE_ENVIRONMENT is not set.");
            }

            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{_environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public static IServiceCollection AddConfigurationMapping<T>(this IServiceCollection services, string section) where T : class
        {
            return services.Configure<T>(_configuration.Value.GetSection(section));
        }
    }
}