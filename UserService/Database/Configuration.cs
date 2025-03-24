namespace CarRental.Database;


public static class Configuration
{
    public static string GetEnvironment(string name)
    {
        return Environment.GetEnvironmentVariable(name) 
               ?? throw new Exception($"Environment variable {name} is not set");
    }
}