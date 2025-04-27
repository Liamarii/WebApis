using Npgsql;

namespace VehiclesTests.Database;

public static class DatabaseSeeder
{
    private const string TableName = "vehicles";

    private const string CreateVehiclesTableQuery = @$"
        CREATE TABLE IF NOT EXISTS {TableName} (
            id SERIAL PRIMARY KEY,
            make VARCHAR(100),
            model VARCHAR(100),
            year INT
        );";

    private const string InsertVehicleQuery = @$"
        INSERT INTO {TableName} (make, model, year) 
        VALUES ('Hyundai', 'Pancake', 2021),('Bmw', 'Jellyfish', 2050);";

    public static async Task SeedAsync(string connectionString)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        await using var createCmd = new NpgsqlCommand(CreateVehiclesTableQuery, connection);
        await createCmd.ExecuteNonQueryAsync();

        await using var insertCmd = new NpgsqlCommand(InsertVehicleQuery, connection);
        await insertCmd.ExecuteNonQueryAsync();
    }
}
