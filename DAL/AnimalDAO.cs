using Npgsql;
using NpgsqlTypes;
using Metier;

namespace DAL;

public class AnimalDAO
{
    public static void Ajouter(Animal animal)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand(
            "SELECT animal_inserer(@id, @nom, @type, @sexe, @particularites, @date_deces, @description, @date_sterilisation, @sterilise, @date_naissance)",
            conn);
        cmd.Parameters.AddWithValue("id", animal.Identifiant);
        cmd.Parameters.AddWithValue("nom", animal.Nom);
        cmd.Parameters.AddWithValue("type", animal.Type);
        cmd.Parameters.AddWithValue("sexe", animal.Sexe);
        cmd.Parameters.AddWithValue("particularites", (object?)animal.Particularite ?? DBNull.Value);
        AddDateParam(cmd, "date_deces", animal.DateDeces);
        cmd.Parameters.AddWithValue("description", (object?)animal.Description ?? DBNull.Value);
        AddDateParam(cmd, "date_sterilisation", animal.DateSterilisation);
        cmd.Parameters.AddWithValue("sterilise", animal.Sterilise);
        AddDateParam(cmd, "date_naissance", animal.DateNaissance);
        cmd.ExecuteNonQuery();

        foreach (var couleur in animal.Couleurs)
            AjouterCouleur(animal.Identifiant, couleur);
        foreach (var compatibilite in animal.Compatibilites)
            AjouterCompatibilite(animal.Identifiant, compatibilite);
    }

    public static string ProchainIdentifiant(DateTime dateEntree)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT animal_prochain_identifiant(@date)", conn);
        AddDateParam(cmd, "date", dateEntree);
        return (string)cmd.ExecuteScalar()!;
    }

    public static Animal? Consulter(string identifiant)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();

        using var cmd = new NpgsqlCommand("SELECT * FROM animal_consulter(@id)", conn);
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
        using var cmd = new NpgsqlCommand("SELECT animal_supprimer(@id)", conn);
        cmd.Parameters.AddWithValue("id", identifiant);
        cmd.ExecuteNonQuery();
    }

    public static void ModifierDescription(string identifiant, string? description)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT animal_modifier_description(@id, @desc)", conn);
        cmd.Parameters.AddWithValue("id", identifiant);
        cmd.Parameters.AddWithValue("desc", (object?)description ?? DBNull.Value);
        cmd.ExecuteNonQuery();
    }

    public static void ModifierParticularites(string identifiant, string? particularites)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT animal_modifier_particularites(@id, @part)", conn);
        cmd.Parameters.AddWithValue("id", identifiant);
        cmd.Parameters.AddWithValue("part", (object?)particularites ?? DBNull.Value);
        cmd.ExecuteNonQuery();
    }

    public static void ModifierDateDeces(string identifiant, DateTime? dateDeces)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT animal_modifier_date_deces(@id, @date)", conn);
        cmd.Parameters.AddWithValue("id", identifiant);
        cmd.Parameters.AddWithValue("date", (object?)dateDeces ?? DBNull.Value);
        cmd.ExecuteNonQuery();
    }

    public static void SupprimerCompatibilite(string animalId, string typeCompatibilite)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT animal_supprimer_compatibilite(@ani_id, @type)", conn);
        cmd.Parameters.AddWithValue("ani_id", animalId);
        cmd.Parameters.AddWithValue("type", typeCompatibilite);
        cmd.ExecuteNonQuery();
    }

    public static void SupprimerCouleur(string animalId, string nomCouleur)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT animal_supprimer_couleur(@ani_id, @nom)", conn);
        cmd.Parameters.AddWithValue("ani_id", animalId);
        cmd.Parameters.AddWithValue("nom", nomCouleur);
        cmd.ExecuteNonQuery();
    }

    public static List<Animal> ListerTous()
    {
        var animaux = new List<Animal>();
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM animal_lister_tous()", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            animaux.Add(new Animal
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
            });
        }
        reader.Close();
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
        using var cmd = new NpgsqlCommand("SELECT * FROM animal_lister_au_refuge()", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            animaux.Add(new Animal
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
            });
        }
        reader.Close();
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
        using var cmd = new NpgsqlCommand("SELECT * FROM animal_charger_couleurs(@id)", conn);
        cmd.Parameters.AddWithValue("id", animal.Identifiant);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            animal.Couleurs.Add(reader.GetString(0));
    }

    private static void ChargerCompatibilites(Animal animal)
    {
        animal.Compatibilites.Clear();
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM animal_charger_compatibilites(@id)", conn);
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

    public static void AjouterCouleurPourAnimal(string animalId, string couleurNom) => AjouterCouleur(animalId, couleurNom);

    public static void AjouterCompatibilitePourAnimal(string animalId, Compatibilite compatibilite) => AjouterCompatibilite(animalId, compatibilite);

    private static void AjouterCouleur(string animalId, string couleurNom)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT animal_ajouter_couleur(@ani_id, @nom)", conn);
        cmd.Parameters.AddWithValue("ani_id", animalId);
        cmd.Parameters.AddWithValue("nom", couleurNom);
        cmd.ExecuteNonQuery();
    }

    private static void AjouterCompatibilite(string animalId, Compatibilite compatibilite)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT animal_ajouter_compatibilite(@ani_id, @type, @valeur, @description)", conn);
        cmd.Parameters.AddWithValue("ani_id", animalId);
        cmd.Parameters.AddWithValue("type", compatibilite.Type);
        cmd.Parameters.AddWithValue("valeur", compatibilite.Valeur);
        cmd.Parameters.AddWithValue("description", (object?)compatibilite.Description ?? DBNull.Value);
        cmd.ExecuteNonQuery();
    }

    private static void AddDateParam(NpgsqlCommand cmd, string name, DateTime? value)
    {
        var p = new NpgsqlParameter(name, NpgsqlDbType.Date);
        p.Value = value.HasValue ? (object)value.Value.Date : DBNull.Value;
        cmd.Parameters.Add(p);
    }
}
