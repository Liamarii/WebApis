using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Testcontainers.PostgreSql;

namespace VehiclesTests.Integration.Infrastructure;

public class WebApplicationFactoryWithFakeDatabase<TStartClass> : WebApplicationFactory<TStartClass>, IAsyncDisposable where TStartClass : class
{
    private static PostgreSqlContainer? _container;
    private static bool _containerStarted;
    private static readonly SemaphoreSlim _semaphore = new(1, 1);

    public WebApplicationFactoryWithFakeDatabase()
    {
        _container ??= new PostgreSqlBuilder()
            .WithImage("postgres:15")
            .WithDatabase("testdb")
            .WithUsername("testuser")
            .WithPassword("testpassword")
            .WithCleanUp(true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
            .Build();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        if (_container == null) throw new InvalidOperationException("Container is not running.");

        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
        Environment.SetEnvironmentVariable("TEST_CREDENTIALS", $"{_container.GetConnectionString()};Port={_container.GetMappedPublicPort(5432)}");

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

        await _semaphore.WaitAsync();

        try
        {
            if (!_containerStarted)
            {
                await _container.StartAsync();
                _containerStarted = true;
            }
        }
        finally
        {
            _semaphore.Release();
        }

        if (sqlQueries != null)
            await RunDatabaseQueryAsync(sqlQueries);

        return CreateClient();
    }

    private static async Task RunDatabaseQueryAsync(params string[] sqlQueries)
    {
        if (_container == null) throw new InvalidOperationException("Container is not running.");

        var connStr = _container.GetConnectionString();
        await using var conn = new NpgsqlConnection(connStr);
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