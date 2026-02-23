using Npgsql;
using Metier;

namespace DAL;

public class AnimalDAO
{
    public static void Ajouter(Animal animal)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        using var cmd = new NpgsqlCommand(
            @"INSERT INTO ANIMAL (identifiant, nom, type, sexe, particularites, date_deces, 
                                  description, date_sterilisation, sterilise, date_naissance)
              VALUES (@id, @nom, @type, @sexe, @particularites, @date_deces, 
                      @description, @date_sterilisation, @sterilise, @date_naissance)",
            conn);
        
        cmd.Parameters.AddWithValue("id", animal.Identifiant);
        cmd.Parameters.AddWithValue("nom", animal.Nom);
        cmd.Parameters.AddWithValue("type", animal.Type);
        cmd.Parameters.AddWithValue("sexe", animal.Sexe);
        cmd.Parameters.AddWithValue("particularites", (object?)animal.Particularite ?? DBNull.Value);
        cmd.Parameters.AddWithValue("date_deces", (object?)animal.DateDeces ?? DBNull.Value);
        cmd.Parameters.AddWithValue("description", (object?)animal.Description ?? DBNull.Value);
        cmd.Parameters.AddWithValue("date_sterilisation", (object?)animal.DateSterilisation ?? DBNull.Value);
        cmd.Parameters.AddWithValue("sterilise", animal.Sterilise);
        cmd.Parameters.AddWithValue("date_naissance", animal.DateNaissance);
        
        cmd.ExecuteNonQuery();
        
        // Ajouter les couleurs
        foreach (var couleur in animal.Couleurs)
        {
            AjouterCouleur(animal.Identifiant, couleur);
        }
        
        // Ajouter les compatibilités
        foreach (var compatibilite in animal.Compatibilites)
        {
            AjouterCompatibilite(animal.Identifiant, compatibilite);
        }
    }

    public static Animal? Consulter(string identifiant)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        using var cmd = new NpgsqlCommand(
            @"SELECT identifiant, nom, type, sexe, particularites, date_deces, 
                     description, date_sterilisation, sterilise, date_naissance
              FROM ANIMAL WHERE identifiant = @id",
            conn);
        
        cmd.Parameters.AddWithValue("id", identifiant);
        
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            var animal = new Animal
            {
                Identifiant = reader.GetString(0),
                Nom = reader.GetString(1),
                Type = reader.GetString(2),
                Sexe = reader.GetString(3),
                Particularite = reader.IsDBNull(4) ? null : reader.GetString(4),
                DateDeces = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                Description = reader.IsDBNull(6) ? null : reader.GetString(6),
                DateSterilisation = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                Sterilise = reader.GetBoolean(8),
                DateNaissance = reader.GetDateTime(9)
            };
            
            reader.Close();
            ChargerCouleurs(animal);
            ChargerCompatibilites(animal);
            return animal;
        }
        return null;
    }

    public static void Supprimer(string identifiant)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        using var cmd = new NpgsqlCommand("DELETE FROM ANIMAL WHERE identifiant = @id", conn);
        cmd.Parameters.AddWithValue("id", identifiant);
        cmd.ExecuteNonQuery();
    }

    public static List<Animal> ListerTous()
    {
        var animaux = new List<Animal>();
        
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        using var cmd = new NpgsqlCommand(
            @"SELECT identifiant, nom, type, sexe, particularites, date_deces, 
                     description, date_sterilisation, sterilise, date_naissance
              FROM ANIMAL ORDER BY nom",
            conn);
        
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var animal = new Animal
            {
                Identifiant = reader.GetString(0),
                Nom = reader.GetString(1),
                Type = reader.GetString(2),
                Sexe = reader.GetString(3),
                Particularite = reader.IsDBNull(4) ? null : reader.GetString(4),
                DateDeces = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                Description = reader.IsDBNull(6) ? null : reader.GetString(6),
                DateSterilisation = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                Sterilise = reader.GetBoolean(8),
                DateNaissance = reader.GetDateTime(9)
            };
            
            animaux.Add(animal);
        }
        
        // Charger les couleurs et compatibilités pour chaque animal
        foreach (var animal in animaux)
        {
            ChargerCouleurs(animal);
            ChargerCompatibilites(animal);
        }
        
        return animaux;
    }

    public static List<Animal> ListerAnimauxAuRefuge()
    {
        var animaux = new List<Animal>();
        
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        using var cmd = new NpgsqlCommand(
            @"SELECT DISTINCT a.identifiant, a.nom, a.type, a.sexe, a.particularites, 
                     a.date_deces, a.description, a.date_sterilisation, a.sterilise, a.date_naissance
              FROM ANIMAL a
              WHERE a.date_deces IS NULL
              AND (
                  NOT EXISTS (SELECT 1 FROM ANI_SORTIE s WHERE s.ani_identifiant = a.identifiant)
                  OR EXISTS (
                      SELECT 1 FROM ANI_ENTREE e
                      WHERE e.ani_identifiant = a.identifiant
                      AND e.date_entree > COALESCE(
                          (SELECT MAX(s2.date_sortie) FROM ANI_SORTIE s2 WHERE s2.ani_identifiant = a.identifiant),
                          '1900-01-01'::date
                      )
                  )
              )
              ORDER BY a.nom",
            conn);
        
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var animal = new Animal
            {
                Identifiant = reader.GetString(0),
                Nom = reader.GetString(1),
                Type = reader.GetString(2),
                Sexe = reader.GetString(3),
                Particularite = reader.IsDBNull(4) ? null : reader.GetString(4),
                DateDeces = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                Description = reader.IsDBNull(6) ? null : reader.GetString(6),
                DateSterilisation = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                Sterilise = reader.GetBoolean(8),
                DateNaissance = reader.GetDateTime(9)
            };
            
            animaux.Add(animal);
        }
        
        foreach (var animal in animaux)
        {
            ChargerCouleurs(animal);
            ChargerCompatibilites(animal);
        }
        
        return animaux;
    }

    private static void ChargerCouleurs(Animal animal)
    {
        animal.Couleurs.Clear();
        
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        using var cmd = new NpgsqlCommand(
            @"SELECT c.nom_couleur
              FROM COULEUR c
              INNER JOIN ANIMAL_COULEUR ac ON c.col_identifiant = ac.col_identifiant
              WHERE ac.ani_identifiant = @id",
            conn);
        
        cmd.Parameters.AddWithValue("id", animal.Identifiant);
        
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            animal.Couleurs.Add(reader.GetString(0));
        }
    }

    private static void ChargerCompatibilites(Animal animal)
    {
        animal.Compatibilites.Clear();
        
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        using var cmd = new NpgsqlCommand(
            @"SELECT ac.valeur, ac.description, c.type
              FROM ANI_COMPATIBILITE ac
              INNER JOIN COMPATIBILITE c ON ac.comp_identifiant = c.identifiant
              WHERE ac.ani_identifiant = @id",
            conn);
        
        cmd.Parameters.AddWithValue("id", animal.Identifiant);
        
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            animal.Compatibilites.Add(new Compatibilite
            {
                Valeur = reader.GetBoolean(0),
                Description = reader.IsDBNull(1) ? null : reader.GetString(1),
                Type = reader.GetString(2)
            });
        }
    }

    private static void AjouterCouleur(string animalId, string couleurNom)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        // Chercher ou créer la couleur
        using var cmdFind = new NpgsqlCommand(
            "SELECT col_identifiant FROM COULEUR WHERE nom_couleur = @nom",
            conn);
        cmdFind.Parameters.AddWithValue("nom", couleurNom);
        var couleurId = cmdFind.ExecuteScalar();
        
        if (couleurId == null)
        {
            using var cmdInsert = new NpgsqlCommand(
                "INSERT INTO COULEUR (nom_couleur) VALUES (@nom) RETURNING col_identifiant",
                conn);
            cmdInsert.Parameters.AddWithValue("nom", couleurNom);
            couleurId = cmdInsert.ExecuteScalar();
        }
        
        // Lier l'animal à la couleur
        using var cmdLink = new NpgsqlCommand(
            "INSERT INTO ANIMAL_COULEUR (col_identifiant, ani_identifiant) VALUES (@couleurId, @animalId) ON CONFLICT DO NOTHING",
            conn);
        cmdLink.Parameters.AddWithValue("couleurId", (int)couleurId);
        cmdLink.Parameters.AddWithValue("animalId", animalId);
        cmdLink.ExecuteNonQuery();
    }

    private static void AjouterCompatibilite(string animalId, Compatibilite compatibilite)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        // Chercher ou créer le type de compatibilité
        using var cmdFind = new NpgsqlCommand(
            "SELECT identifiant FROM COMPATIBILITE WHERE type = @type",
            conn);
        cmdFind.Parameters.AddWithValue("type", compatibilite.Type);
        var compId = cmdFind.ExecuteScalar();
        
        if (compId == null)
        {
            using var cmdInsert = new NpgsqlCommand(
                "INSERT INTO COMPATIBILITE (type) VALUES (@type) RETURNING identifiant",
                conn);
            cmdInsert.Parameters.AddWithValue("type", compatibilite.Type);
            compId = cmdInsert.ExecuteScalar();
        }
        
        // Lier l'animal à la compatibilité
        using var cmdLink = new NpgsqlCommand(
            @"INSERT INTO ANI_COMPATIBILITE (valeur, description, comp_identifiant, ani_identifiant) 
              VALUES (@valeur, @description, @compId, @animalId) ON CONFLICT DO NOTHING",
            conn);
        cmdLink.Parameters.AddWithValue("valeur", compatibilite.Valeur);
        cmdLink.Parameters.AddWithValue("description", (object?)compatibilite.Description ?? DBNull.Value);
        cmdLink.Parameters.AddWithValue("compId", (int)compId);
        cmdLink.Parameters.AddWithValue("animalId", animalId);
        cmdLink.ExecuteNonQuery();
    }
}
