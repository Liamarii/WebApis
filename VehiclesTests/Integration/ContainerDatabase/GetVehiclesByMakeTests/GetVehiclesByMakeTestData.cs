﻿namespace VehiclesTests.Integration.ContainerDatabase.GetVehiclesByMakeTests
{
    internal static class GetVehiclesByMakeTestData
    {
        public const string CreateTableSql = @"
        CREATE TABLE IF NOT EXISTS vehicles (
        id SERIAL PRIMARY KEY,
        vin VARCHAR(17) UNIQUE NOT NULL,
        make TEXT NOT NULL,
        model TEXT NOT NULL,
        year INT NOT NULL);";

        public const string InsertDataSql = @"
        INSERT INTO vehicles (vin, make, model, year)
        VALUES
        ('1HGCM82633A123456', 'Honda', 'Egg', 2020),
        ('1FADP3F24JL123457', 'Ford', 'Fastcar', 2020),
        ('3N1AB7AP8HY123458', 'Nissan', 'Bagel', 2021),
        ('1C4RJFBG4FC123459', 'Jeep', 'Spongebob', 2022),
        ('5YJ3E1EA7KF123460', 'Tesla', 'Model Why', 2022)
        ON CONFLICT (vin) DO NOTHING;";
    }
}
