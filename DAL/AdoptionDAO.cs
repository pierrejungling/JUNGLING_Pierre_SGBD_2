using Npgsql;
using Metier;

namespace DAL;

public class AdoptionDAO
{
    public static void Ajouter(Adoption adoption)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        using var cmd = new NpgsqlCommand(
            "SELECT adoption_ajouter(@statut, @date_demande, @ani_identifiant, @adop_contact)",
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
        using var cmd = new NpgsqlCommand("SELECT adoption_modifier_statut(@ani_id, @statut)", conn);
        cmd.Parameters.AddWithValue("ani_id", animalId);
        cmd.Parameters.AddWithValue("statut", nouveauStatut);
        cmd.ExecuteNonQuery();
    }

    public static Adoption? ConsulterParAnimal(string animalId)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM adoption_consulter_par_animal(@id)", conn);
        cmd.Parameters.AddWithValue("id", animalId);
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            var adoption = new Adoption
            {
                Statut = reader.GetString(0),
                DateDemande = reader.GetDateTime(1),
                AnimalAdopte = AnimalDAO.Consulter(reader.GetString(2)) ?? new Animal { Identifiant = reader.GetString(2) },
                Adoptant = ContactDAO.Consulter(reader.GetInt32(3)) ?? new Contact { Identifiant = reader.GetInt32(3) }
            };
            return adoption;
        }
        return null;
    }

    public static List<Adoption> ListerToutes()
    {
        var adoptions = new List<Adoption>();
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM adoption_lister_toutes()", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            adoptions.Add(new Adoption
            {
                Statut = reader.GetString(0),
                DateDemande = reader.GetDateTime(1),
                AnimalAdopte = AnimalDAO.Consulter(reader.GetString(2)) ?? new Animal { Identifiant = reader.GetString(2) },
                Adoptant = ContactDAO.Consulter(reader.GetInt32(3)) ?? new Contact { Identifiant = reader.GetInt32(3) }
            });
        }
        return adoptions;
    }
}
