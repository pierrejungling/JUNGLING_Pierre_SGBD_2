using Npgsql;
using Metier;

namespace DAL;

public class SortieDAO
{
    /// <param name="dateDecesAnimal">Si raison = 'deces_animal', passer la date de décès pour mettre à jour ANIMAL.date_deces (déclencheur BDD). Sinon null.</param>
    public static void Ajouter(Sortie sortie, DateTime? dateDecesAnimal = null)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        using var cmd = new NpgsqlCommand(
            "SELECT sortie_ajouter(@raison, @date_sortie, @ani_identifiant, @sortie_contact, @date_deces_animal)",
            conn);
        cmd.Parameters.AddWithValue("raison", sortie.Raison);
        cmd.Parameters.AddWithValue("date_sortie", sortie.DateSortie);
        cmd.Parameters.AddWithValue("ani_identifiant", sortie.Animal.Identifiant);
        cmd.Parameters.AddWithValue("sortie_contact", (object?)sortie.Contact?.Identifiant ?? DBNull.Value);
        cmd.Parameters.AddWithValue("date_deces_animal", (object?)dateDecesAnimal ?? DBNull.Value);
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
