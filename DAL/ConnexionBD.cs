using Npgsql;

namespace DAL;

public static class ConnexionBD
{
    private static string? _connectionString;

    public static void SetConnectionString(string host, string database, string username, string password, int port = 5432)
    {
        _connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";
    }

    public static string GetConnectionString()
    {
        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new InvalidOperationException("La chaîne de connexion n'a pas été initialisée. Utilisez SetConnectionString() d'abord.");
        }
        return _connectionString;
    }

    public static NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(GetConnectionString());
    }
}
