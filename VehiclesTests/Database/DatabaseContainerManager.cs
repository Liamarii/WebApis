using Testcontainers.PostgreSql;

namespace VehiclesTests.Database;

public class DatabaseContainerManager : IAsyncDisposable
{
    private readonly PostgreSqlContainer _postgresContainer;
    private bool _disposed;

    //TODO: Maybe this should be more generic as it's specific to pg currently.

    public DatabaseContainerManager()
    {
        _postgresContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15")
            .WithDatabase("testdb")
            .WithUsername("testuser")
            .WithPassword("testpassword")
            .WithName("VehicleTests")
            .Build();
    }

    public async Task StartAsync() => await _postgresContainer.StartAsync();

    public string GetConnectionString() => _postgresContainer.GetConnectionString();

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        if (_postgresContainer != null)
        {
            await _postgresContainer.StopAsync();
        }

        _disposed = true;

        GC.SuppressFinalize(this);
    }
}
