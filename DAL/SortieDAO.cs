using Npgsql;
using NpgsqlTypes;
using Metier;

namespace DAL;

public class SortieDAO
{
    public static void Ajouter(Sortie sortie, DateTime? dateDecesAnimal = null)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        using var cmd = new NpgsqlCommand(
            "SELECT sortie_ajouter(@raison, @date_sortie, @ani_identifiant, @sortie_contact, @date_deces_animal)",
            conn);
        cmd.Parameters.AddWithValue("raison", sortie.Raison);
        var pDateSortie = new NpgsqlParameter("date_sortie", NpgsqlDbType.Date);
        pDateSortie.Value = sortie.DateSortie.Date;
        cmd.Parameters.Add(pDateSortie);
        cmd.Parameters.AddWithValue("ani_identifiant", sortie.Animal.Identifiant);
        cmd.Parameters.AddWithValue("sortie_contact", sortie.Contact == null || sortie.Contact.Identifiant <= 0 ? DBNull.Value : sortie.Contact.Identifiant);
        var pDeces = new NpgsqlParameter("date_deces_animal", NpgsqlDbType.Date);
        pDeces.Value = dateDecesAnimal?.Date ?? (object)DBNull.Value;
        cmd.Parameters.Add(pDeces);
        cmd.ExecuteNonQuery();
    }

    public static List<Sortie> ListerParAnimal(string animalId)
    {
        var sorties = new List<Sortie>();
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM sortie_lister_par_animal(@id)", conn);
        cmd.Parameters.AddWithValue("id", animalId);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var sortie = new Sortie
            {
                Raison = reader.GetString(0),
                DateSortie = reader.GetDateTime(1),
                Animal = new Animal { Identifiant = reader.GetString(2) }
            };
            if (!reader.IsDBNull(3))
                sortie.Contact = ContactDAO.Consulter(reader.GetInt32(3)) ?? new Contact { Identifiant = reader.GetInt32(3) };
            sorties.Add(sortie);
        }
        return sorties;
    }
}
