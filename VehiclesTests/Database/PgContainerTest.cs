using Npgsql;
using Testcontainers.PostgreSql;

namespace VehiclesTests.Database
{
    [TestFixture]
    public class PgContainerTest
    {
        private PostgreSqlContainer? _postgresContainer;
        private const string _createVehiclesTableQuery = @"
                CREATE TABLE IF NOT EXISTS vehicles (
                    id SERIAL PRIMARY KEY,
                    make VARCHAR(100),
                    model VARCHAR(100),
                    year INT
                );
            ";
        private const string _insertVehicleQuery = @"
                INSERT INTO vehicles (make, model, year) 
                VALUES 
                    ('Hyundai', 'Pancake', 2021),
                    ('Bmw','Jellyfish', 2050);
            ";

        private async Task CreatePostgresDatabase()
        {
            _postgresContainer = new PostgreSqlBuilder()
                .WithImage("postgres:15")
                .WithDatabase("testdb")
                .WithUsername("testuser")
                .WithPassword("testpassword")
                .WithName("VehicleTests")
                .Build();

            await _postgresContainer.StartAsync();
        }

        private async Task AddTable(string query)
        {
            ArgumentNullException.ThrowIfNull(_postgresContainer, nameof(_postgresContainer));
            string connectionString = _postgresContainer.GetConnectionString();
            await using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            await using var cmd = new NpgsqlCommand(query, connection);
            await cmd.ExecuteNonQueryAsync();
        }

        private async Task AddVehicleData(string insertVehicleQuery)
        {
            ArgumentNullException.ThrowIfNull(_postgresContainer, nameof(_postgresContainer));
            string connectionString = _postgresContainer.GetConnectionString();
            await using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            await using var cmd = new NpgsqlCommand(insertVehicleQuery, connection);
            await cmd.ExecuteNonQueryAsync();
        }

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            await CreatePostgresDatabase();
            await AddTable(_createVehiclesTableQuery);
            await AddVehicleData(_insertVehicleQuery);
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown() => await _postgresContainer?.StopAsync()!; //I'm ignoring it might be null as I want it to only execute if not null.

        [Test]
        public async Task TableContainsData()
        {
            ArgumentNullException.ThrowIfNull(_postgresContainer, nameof(_postgresContainer));
            string connectionString = _postgresContainer.GetConnectionString();
            await using NpgsqlConnection connection = new(connectionString);
            await connection.OpenAsync();

            string selectVehicleQuery = "SELECT COUNT(1) FROM vehicles";
            await using var cmd = new NpgsqlCommand(selectVehicleQuery, connection);
            long count = (long) await cmd.ExecuteScalarAsync();
            Assert.That(count, Is.EqualTo(2));
        }
    }
}