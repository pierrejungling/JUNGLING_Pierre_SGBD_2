using System.Text.Json;
using DAL;
using Metier;
using Npgsql;
using Presentation;

try
{
    // Chargement de la configuration depuis appsettings.json
    string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
    
    if (!File.Exists(configPath))
    {
        // Si le fichier n'existe pas dans le dossier d'exécution, chercher dans le dossier du projet
        configPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
    }
    
    if (!File.Exists(configPath))
    {
        throw new FileNotFoundException($"Le fichier de configuration appsettings.json est introuvable. Créez-le avec vos paramètres de connexion.");
    }

    string jsonContent = File.ReadAllText(configPath);
    JsonElement config;
    
    try
    {
        config = JsonSerializer.Deserialize<JsonElement>(jsonContent);
    }
    catch (JsonException ex)
    {
        throw new Exception($"Erreur lors de la lecture du fichier appsettings.json: {ex.Message}", ex);
    }
    
    if (!config.TryGetProperty("Database", out JsonElement dbConfig))
    {
        throw new Exception("La section 'Database' est manquante dans appsettings.json");
    }
    
    string host = dbConfig.TryGetProperty("Host", out JsonElement hostEl) ? hostEl.GetString() ?? "localhost" : "localhost";
    int port = dbConfig.TryGetProperty("Port", out JsonElement portEl) ? portEl.GetInt32() : 5432;
    string database = dbConfig.TryGetProperty("Database", out JsonElement dbEl) ? dbEl.GetString() ?? "refuge_animaux" : "refuge_animaux";
    string username = dbConfig.TryGetProperty("Username", out JsonElement userEl) ? userEl.GetString() ?? "refuge_animaux_user" : "refuge_animaux_user";
    string password = dbConfig.TryGetProperty("Password", out JsonElement passEl) ? passEl.GetString() ?? "" : "";

    ConnexionBD.SetConnectionString(host, database, username, password, port);
    
    // Test de la connexion
    try
    {
        using var testConn = ConnexionBD.GetConnection();
        testConn.Open();
        testConn.Close();
    }
    catch (NpgsqlException ex)
    {
        Console.WriteLine($"\nErreur de connexion PostgreSQL: {ex.Message}");
        Console.WriteLine($"Code d'erreur: {ex.SqlState}");
        Console.WriteLine("\nVérifiez que:");
        Console.WriteLine("1. PostgreSQL est démarré");
        Console.WriteLine("2. La base de données existe");
        Console.WriteLine("3. Les identifiants dans appsettings.json sont corrects");
        Console.WriteLine("4. Le port est correct (par défaut: 5432)");
        throw;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\nErreur inattendue lors de la connexion: {ex.Message}");
        Console.WriteLine($"Type: {ex.GetType().Name}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"Erreur interne: {ex.InnerException.Message}");
        }
        throw;
    }

    // Vérifier que la console est disponible et configurée correctement
    try
    {
        // Tester si nous pouvons lire depuis la console
        if (!Console.IsInputRedirected && Console.In == null)
        {
            throw new InvalidOperationException("La console d'entrée n'est pas disponible.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\nATTENTION: Problème avec la console d'entrée: {ex.Message}");
        Console.WriteLine("\nSOLUTION:");
        Console.WriteLine("1. Arrêtez cette application");
        Console.WriteLine("2. Ouvrez un terminal dans VS Code: Terminal > New Terminal (Ctrl+`)");
        Console.WriteLine("3. Exécutez l'application depuis le terminal avec: dotnet run");
        Console.WriteLine("\nOU");
        Console.WriteLine("Modifiez le fichier .vscode/launch.json pour utiliser un terminal intégré:");
        Console.WriteLine("Ajoutez \"console\": \"integratedTerminal\" dans la configuration de débogage.");
        Console.WriteLine("\nFermeture dans 5 secondes...");
        System.Threading.Thread.Sleep(5000);
        return;
    }

    // Menu principal
    MenuPrincipal menu = new MenuPrincipal();
    menu.Afficher();
}
catch (NpgsqlException ex)
{
    Console.WriteLine($"\nErreur de connexion PostgreSQL: {ex.Message}");
    Console.WriteLine($"Code d'erreur: {ex.SqlState}");
    Console.WriteLine($"Type: {ex.GetType().Name}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Erreur interne: {ex.InnerException.Message}");
    }
    Console.WriteLine("\nVérifiez que:");
    Console.WriteLine("1. PostgreSQL est démarré");
    Console.WriteLine("2. La base de données existe");
    Console.WriteLine("3. Les identifiants dans appsettings.json sont corrects");
    Console.WriteLine("4. Le port est correct (par défaut: 5432)");
    
    // Essayer de lire une touche, mais gérer le cas où la console n'est pas disponible (debug console)
    try
    {
        Console.WriteLine("\nAppuyez sur une touche pour quitter...");
        Console.ReadKey(true);
    }
    catch (InvalidOperationException)
    {
        // Si la console n'est pas disponible (debug console VS Code), attendre un peu puis quitter
        Console.WriteLine("\nFermeture automatique dans 3 secondes...");
        System.Threading.Thread.Sleep(3000);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"\nErreur: {ex.Message}");
    Console.WriteLine($"Type: {ex.GetType().Name}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Erreur interne: {ex.InnerException.Message}");
    }
    Console.WriteLine($"\nStack trace: {ex.StackTrace}");
    
    // Essayer de lire une touche, mais gérer le cas où la console n'est pas disponible (debug console)
    try
    {
        Console.WriteLine("\nAppuyez sur une touche pour quitter...");
        Console.ReadKey(true);
    }
    catch (InvalidOperationException)
    {
        // Si la console n'est pas disponible (debug console VS Code), attendre un peu puis quitter
        Console.WriteLine("\nFermeture automatique dans 3 secondes...");
        System.Threading.Thread.Sleep(3000);
    }
}
