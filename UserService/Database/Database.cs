using CarRental.Domain;
using Npgsql;

namespace CarRental.Database;

public class Connection
{
    private readonly NpgsqlDataSource _dbConnection;

    public Connection()
    {
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = Configuration.GetEnvironment("DB_HOST"),
            Port = int.Parse(Configuration.GetEnvironment("DB_PORT")),
            Username = Configuration.GetEnvironment("DB_USER"),
            Password = Configuration.GetEnvironment("DB_PASSWORD"),
            Database = Configuration.GetEnvironment("DB_NAME")
        };

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.ConnectionString);
        
        dataSourceBuilder.MapComposite<User>();
        dataSourceBuilder.EnableDynamicJson();
        _dbConnection = dataSourceBuilder.Build();
    }

    public NpgsqlDataSource Request()
    {
        return _dbConnection;
    }
}