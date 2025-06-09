using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Testcontainers.PostgreSql;

namespace VehiclesTests.Integration;

public class WebApplicationFactoryWithFakeDatabase<TStartup> : WebApplicationFactory<TStartup>, IAsyncDisposable where TStartup : class
{
    private static PostgreSqlContainer? _container;
    private static bool _containerStarted;
    private static readonly Lock _lock = new();

    public WebApplicationFactoryWithFakeDatabase()
    {
        _container ??= new PostgreSqlBuilder()
                .WithImage("postgres:15")
                .WithDatabase("testdb")
                .WithUsername("testuser")
                .WithPassword("testpassword")
                .WithName("integrationTests")
                .Build();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            if (_container != null)
            {
                var overrides = new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DefaultConnection"] = _container.GetConnectionString()
                };
                config.AddInMemoryCollection(overrides);
            }
        });

        return base.CreateHost(builder);
    }

    public async Task<HttpClient> InitializeAsync(params string[] sqlQueries)
    {
        if (_container == null) throw new InvalidOperationException("Container is not running.");

        lock (_lock)
        {
            if (!_containerStarted)
            {
                _container.StartAsync().GetAwaiter().GetResult();
                _containerStarted = true;
            }
        }

        if (sqlQueries != null)
            await RunDatabaseQueryAsync(sqlQueries);

        return CreateClient();
    }

    private static async Task RunDatabaseQueryAsync(params string[] sqlQueries)
    {
        if (_container == null) throw new InvalidOperationException("Container is not running.");

        await using var conn = new NpgsqlConnection(_container.GetConnectionString());
        await conn.OpenAsync();

        foreach (var sqlQuery in sqlQueries)
        {
            await using var cmd = new NpgsqlCommand(sqlQuery, conn);
            await cmd.ExecuteNonQueryAsync();
        }
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (_container != null && _containerStarted)
        {
            await _container.StopAsync();
            await _container.DisposeAsync();
            _containerStarted = false;
            _container = null;
        }

        GC.SuppressFinalize(this);
    }
}