using Npgsql;
using Metier;

namespace DAL;

public class AccueilDAO
{
    public static void Ajouter(Accueil accueil)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        using var cmd = new NpgsqlCommand(
            @"INSERT INTO FAMILLE_ACCUEIL (date_debut, date_fin, fa_ani_identifiant, fa_contact)
              VALUES (@date_debut, @date_fin, @fa_ani_identifiant, @fa_contact)",
            conn);
        
        cmd.Parameters.AddWithValue("date_debut", accueil.DateDebut);
        cmd.Parameters.AddWithValue("date_fin", (object?)accueil.DateFin ?? DBNull.Value);
        cmd.Parameters.AddWithValue("fa_ani_identifiant", accueil.AnimalAccueilli.Identifiant);
        cmd.Parameters.AddWithValue("fa_contact", accueil.FamilleAccueil.Identifiant);
        
        cmd.ExecuteNonQuery();
    }

    public static List<Accueil> ListerParAnimal(string animalId)
    {
        var accueils = new List<Accueil>();
        
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        using var cmd = new NpgsqlCommand(
            @"SELECT date_debut, date_fin, fa_ani_identifiant, fa_contact
              FROM FAMILLE_ACCUEIL
              WHERE fa_ani_identifiant = @id
              ORDER BY date_debut DESC",
            conn);
        
        cmd.Parameters.AddWithValue("id", animalId);
        
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var accueil = new Accueil
            {
                DateDebut = reader.GetDateTime(0),
                DateFin = reader.IsDBNull(1) ? null : reader.GetDateTime(1),
                AnimalAccueilli = new Animal { Identifiant = reader.GetString(2) },
                FamilleAccueil = ContactDAO.Consulter(reader.GetInt32(3)) ?? new Contact { Identifiant = reader.GetInt32(3) }
            };
            
            accueils.Add(accueil);
        }
        
        return accueils;
    }

    public static List<Accueil> ListerParFamille(int contactId)
    {
        var accueils = new List<Accueil>();
        
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        using var cmd = new NpgsqlCommand(
            @"SELECT date_debut, date_fin, fa_ani_identifiant, fa_contact
              FROM FAMILLE_ACCUEIL
              WHERE fa_contact = @id
              ORDER BY date_debut DESC",
            conn);
        
        cmd.Parameters.AddWithValue("id", contactId);
        
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var accueil = new Accueil
            {
                DateDebut = reader.GetDateTime(0),
                DateFin = reader.IsDBNull(1) ? null : reader.GetDateTime(1),
                AnimalAccueilli = AnimalDAO.Consulter(reader.GetString(2)) ?? new Animal { Identifiant = reader.GetString(2) },
                FamilleAccueil = ContactDAO.Consulter(reader.GetInt32(3)) ?? new Contact { Identifiant = reader.GetInt32(3) }
            };
            
            accueils.Add(accueil);
        }
        
        return accueils;
    }
}
