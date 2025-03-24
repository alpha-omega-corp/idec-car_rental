using Dapper;
using CarRental.Database;
using CarRental.Domain;

namespace CarRental.Repositories;

public class UserRepository(Connection database)
{

    public async void Create(User user)
    {
        try
        {
            await using var cmd = database.Request()
                .CreateCommand($"INSERT INTO users (name, email, password) VALUES ('{user.Name}', '{user.Email}', '{BCrypt.Net.BCrypt.HashPassword(user.Password)}')");
            
            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public User GetOne(string email)
    {
        return database.Request().OpenConnection().QuerySingle<User>(sql:
            $"SELECT * FROM users WHERE email = '{email}';");
    }
}