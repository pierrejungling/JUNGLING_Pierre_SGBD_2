using Npgsql;
using Metier;

namespace DAL;

public class SortieDAO
{
    public static void Ajouter(Sortie sortie)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        using var cmd = new NpgsqlCommand(
            @"INSERT INTO ANI_SORTIE (raison, date_sortie, ani_identifiant, sortie_contact)
              VALUES (@raison, @date_sortie, @ani_identifiant, @sortie_contact)",
            conn);
        
        cmd.Parameters.AddWithValue("raison", sortie.Raison);
        cmd.Parameters.AddWithValue("date_sortie", sortie.DateSortie);
        cmd.Parameters.AddWithValue("ani_identifiant", sortie.Animal.Identifiant);
        cmd.Parameters.AddWithValue("sortie_contact", sortie.Contact.Identifiant);
        
        cmd.ExecuteNonQuery();
    }

    public static List<Sortie> ListerParAnimal(string animalId)
    {
        var sorties = new List<Sortie>();
        
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        using var cmd = new NpgsqlCommand(
            @"SELECT raison, date_sortie, ani_identifiant, sortie_contact
              FROM ANI_SORTIE
              WHERE ani_identifiant = @id
              ORDER BY date_sortie DESC",
            conn);
        
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
            {
                sortie.Contact = ContactDAO.Consulter(reader.GetInt32(3)) ?? new Contact { Identifiant = reader.GetInt32(3) };
            }
            
            sorties.Add(sortie);
        }
        
        return sorties;
    }
}
