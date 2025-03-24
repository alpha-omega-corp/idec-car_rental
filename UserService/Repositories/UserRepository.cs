using System.Data;
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
        using var cmd = database.Request()
            .CreateCommand($"SELECT * FROM users WHERE email = '{email}'");
        
        return cmd.ExecuteReader().Cast<User>().First();
    }
}