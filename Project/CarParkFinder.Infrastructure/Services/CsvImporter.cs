using System.Data;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using CarParkFinder.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class CsvImporter
{
    private readonly string _connectionString;

    public CsvImporter(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task ImportCarParkDataAsync(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));

        var records = csv.GetRecords<CarPark>().ToList(); // Convert CSV to List<CarPark>

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var bulkCopy = new SqlBulkCopy(connection)
        {
            DestinationTableName = "CarParks",
            BatchSize = 5000,  // Optimized batch size
            BulkCopyTimeout = 60
        };

        // Explicit column mappings
        bulkCopy.ColumnMappings.Add("car_park_no", "car_park_no");
        bulkCopy.ColumnMappings.Add("address", "address");
        bulkCopy.ColumnMappings.Add("x_coord", "x_coord");
        bulkCopy.ColumnMappings.Add("y_coord", "y_coord");
        bulkCopy.ColumnMappings.Add("car_park_type", "car_park_type");
        bulkCopy.ColumnMappings.Add("type_of_parking_system", "type_of_parking_system");
        bulkCopy.ColumnMappings.Add("short_term_parking", "short_term_parking");
        bulkCopy.ColumnMappings.Add("free_parking", "free_parking");
        bulkCopy.ColumnMappings.Add("night_parking", "night_parking");
        bulkCopy.ColumnMappings.Add("car_park_decks", "car_park_decks");
        bulkCopy.ColumnMappings.Add("gantry_height", "gantry_height");
        bulkCopy.ColumnMappings.Add("car_park_basement", "car_park_basement");

        var dataTable = ConvertToDataTable(records);
        await bulkCopy.WriteToServerAsync(dataTable); // Async bulk insert into MS SQL
    }

    private DataTable ConvertToDataTable(List<CarPark> records)
    {
        var dataTable = new DataTable();

        dataTable.Columns.Add("car_park_no", typeof(string));
        dataTable.Columns.Add("address", typeof(string));
        dataTable.Columns.Add("x_coord", typeof(string));
        dataTable.Columns.Add("y_coord", typeof(string));
        dataTable.Columns.Add("car_park_type", typeof(string));
        dataTable.Columns.Add("type_of_parking_system", typeof(string));
        dataTable.Columns.Add("short_term_parking", typeof(string));
        dataTable.Columns.Add("free_parking", typeof(string));
        dataTable.Columns.Add("night_parking", typeof(string));
        dataTable.Columns.Add("car_park_decks", typeof(string));
        dataTable.Columns.Add("gantry_height", typeof(string));
        dataTable.Columns.Add("car_park_basement", typeof(string));

        foreach (var record in records)
        {
            dataTable.Rows.Add(
                record.car_park_no, record.address, record.x_coord, record.y_coord,
                record.car_park_type, record.type_of_parking_system, record.short_term_parking,
                record.free_parking, record.night_parking, record.car_park_decks,
                record.gantry_height, record.car_park_basement
            );
        }

        return dataTable;
    }
}
