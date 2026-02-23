using System.IO;
using System.Text.Json;
using System.Windows;
using DAL;
using Npgsql;

namespace RefugeAnimaux;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
        if (!File.Exists(configPath))
            configPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

        if (!File.Exists(configPath))
        {
            MessageBox.Show(
                "Le fichier appsettings.json est introuvable. Créez-le avec la section Database (Host, Port, Database, Username, Password).",
                "Configuration manquante",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            Shutdown(1);
            return;
        }

        try
        {
            string jsonContent = File.ReadAllText(configPath);
            var config = JsonSerializer.Deserialize<JsonElement>(jsonContent);
            if (!config.TryGetProperty("Database", out var dbConfig))
            {
                MessageBox.Show("La section 'Database' est manquante dans appsettings.json.", "Configuration", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown(1);
                return;
            }

            string host = dbConfig.TryGetProperty("Host", out var h) ? h.GetString() ?? "localhost" : "localhost";
            int port = dbConfig.TryGetProperty("Port", out var p) ? p.GetInt32() : 5432;
            string database = dbConfig.TryGetProperty("Database", out var d) ? d.GetString() ?? "refuge_animaux" : "refuge_animaux";
            string username = dbConfig.TryGetProperty("Username", out var u) ? u.GetString() ?? "refuge_animaux_user" : "refuge_animaux_user";
            string password = dbConfig.TryGetProperty("Password", out var pw) ? pw.GetString() ?? "" : "";

            ConnexionBD.SetConnectionString(host, database, username, password, port);

            using var testConn = ConnexionBD.GetConnection();
            testConn.Open();
        }
        catch (NpgsqlException ex)
        {
            MessageBox.Show(
                $"Erreur de connexion PostgreSQL: {ex.Message}\n\nVérifiez que PostgreSQL est démarré, que la base existe et que appsettings.json est correct.",
                "Connexion base de données",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            Shutdown(1);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur au démarrage: {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown(1);
        }
    }
}
