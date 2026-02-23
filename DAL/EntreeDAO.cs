using Npgsql;
using Metier;

namespace DAL;

public class EntreeDAO
{
    public static void Ajouter(Entree entree)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        using var cmd = new NpgsqlCommand(
            "SELECT entree_ajouter(@raison, @date_entree, @ani_identifiant, @entree_contact)",
            conn);
        cmd.Parameters.AddWithValue("raison", entree.Raison);
        cmd.Parameters.AddWithValue("date_entree", entree.DateEntree);
        cmd.Parameters.AddWithValue("ani_identifiant", entree.Animal.Identifiant);
        cmd.Parameters.AddWithValue("entree_contact", (object?)entree.Contact?.Identifiant ?? DBNull.Value);
        cmd.ExecuteNonQuery();
    }

    public static List<Entree> ListerParAnimal(string animalId)
    {
        var entrees = new List<Entree>();
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM entree_lister_par_animal(@id)", conn);
        cmd.Parameters.AddWithValue("id", animalId);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var entree = new Entree
            {
                Raison = reader.GetString(0),
                DateEntree = reader.GetDateTime(1),
                Animal = new Animal { Identifiant = reader.GetString(2) }
            };
            if (!reader.IsDBNull(3))
                entree.Contact = ContactDAO.Consulter(reader.GetInt32(3)) ?? new Contact { Identifiant = reader.GetInt32(3) };
            entrees.Add(entree);
        }
        return entrees;
    }
}
