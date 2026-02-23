using Npgsql;
using Metier;

namespace DAL;

public class AdoptionDAO
{
    // Note: Relation 1-1 avec Animal selon le diagramme
    public static void Ajouter(Adoption adoption)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        // Vérifier qu'il n'y a pas déjà une adoption pour cet animal (relation 1-1)
        using var cmdCheck = new NpgsqlCommand(
            "SELECT COUNT(*) FROM ADOPTION WHERE ani_identifiant = @ani_id",
            conn);
        cmdCheck.Parameters.AddWithValue("ani_id", adoption.AnimalAdopte.Identifiant);
        var count = Convert.ToInt32(cmdCheck.ExecuteScalar());
        
        if (count > 0)
        {
            throw new InvalidOperationException("Cet animal a déjà une adoption (relation 1-1).");
        }
        
        using var cmd = new NpgsqlCommand(
            @"INSERT INTO ADOPTION (statut, date_demande, ani_identifiant, adop_contact)
              VALUES (@statut, @date_demande, @ani_identifiant, @adop_contact)",
            conn);
        
        cmd.Parameters.AddWithValue("statut", adoption.Statut);
        cmd.Parameters.AddWithValue("date_demande", adoption.DateDemande);
        cmd.Parameters.AddWithValue("ani_identifiant", adoption.AnimalAdopte.Identifiant);
        cmd.Parameters.AddWithValue("adop_contact", adoption.Adoptant.Identifiant);
        
        cmd.ExecuteNonQuery();
    }

    public static void ModifierStatut(string animalId, string nouveauStatut)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        using var cmd = new NpgsqlCommand(
            @"UPDATE ADOPTION 
              SET statut = @statut
              WHERE ani_identifiant = @ani_id",
            conn);
        
        cmd.Parameters.AddWithValue("statut", nouveauStatut);
        cmd.Parameters.AddWithValue("ani_id", animalId);
        
        cmd.ExecuteNonQuery();
    }

    public static Adoption? ConsulterParAnimal(string animalId)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        using var cmd = new NpgsqlCommand(
            @"SELECT statut, date_demande, ani_identifiant, adop_contact
              FROM ADOPTION
              WHERE ani_identifiant = @id",
            conn);
        
        cmd.Parameters.AddWithValue("id", animalId);
        
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Adoption
            {
                Statut = reader.GetString(0),
                DateDemande = reader.GetDateTime(1),
                AnimalAdopte = AnimalDAO.Consulter(reader.GetString(2)) ?? new Animal { Identifiant = reader.GetString(2) },
                Adoptant = ContactDAO.Consulter(reader.GetInt32(3)) ?? new Contact { Identifiant = reader.GetInt32(3) }
            };
        }
        return null;
    }

    public static List<Adoption> ListerToutes()
    {
        var adoptions = new List<Adoption>();
        
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        using var cmd = new NpgsqlCommand(
            @"SELECT statut, date_demande, ani_identifiant, adop_contact
              FROM ADOPTION
              ORDER BY date_demande DESC",
            conn);
        
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var adoption = new Adoption
            {
                Statut = reader.GetString(0),
                DateDemande = reader.GetDateTime(1),
                AnimalAdopte = AnimalDAO.Consulter(reader.GetString(2)) ?? new Animal { Identifiant = reader.GetString(2) },
                Adoptant = ContactDAO.Consulter(reader.GetInt32(3)) ?? new Contact { Identifiant = reader.GetInt32(3) }
            };
            
            adoptions.Add(adoption);
        }
        
        return adoptions;
    }
}
