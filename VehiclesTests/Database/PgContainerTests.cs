using Npgsql;

namespace VehiclesTests.Database;

[TestFixture]
public class PgContainerTests
{
    private DatabaseContainerManager? _dbManager;

    [OneTimeSetUp]
    public async Task Setup()
    {
        _dbManager = new DatabaseContainerManager();
        await _dbManager.StartAsync();
        await DatabaseSeeder.SeedAsync(_dbManager.GetConnectionString());
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        if (_dbManager is not null)
        {
            await _dbManager.DisposeAsync();
        }
    }

    [Test]
    public async Task TableContainsData()
    {
        //Arrange
        int expectedRows = 2;
        string query = $"SELECT COUNT(1) FROM VEHICLES";

        //Act
        var response = await RunQuery(query);
        var result = response == null ? 0L : (long) response;

        //Assert
        Assert.That(result, Is.EqualTo(expectedRows));
    }

    private async Task<object?> RunQuery(string query)
    {
        ArgumentNullException.ThrowIfNull(_dbManager, nameof(_dbManager));
        string connectionString = _dbManager.GetConnectionString();
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await using var cmd = new NpgsqlCommand(query, connection);
        return await cmd.ExecuteScalarAsync();
    }
}
