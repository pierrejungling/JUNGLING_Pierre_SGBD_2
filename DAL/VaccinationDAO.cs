using Npgsql;
using Metier;

namespace DAL;

public class VaccinationDAO
{
    public static void Ajouter(Vaccination vaccination)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        // Chercher ou créer le vaccin
        using var cmdFind = new NpgsqlCommand(
            "SELECT identifiant FROM VACCIN WHERE nom = @nom",
            conn);
        cmdFind.Parameters.AddWithValue("nom", vaccination.Vaccin.Nom);
        var vaccinId = cmdFind.ExecuteScalar();
        
        if (vaccinId == null)
        {
            using var cmdInsert = new NpgsqlCommand(
                "INSERT INTO VACCIN (nom) VALUES (@nom) RETURNING identifiant",
                conn);
            cmdInsert.Parameters.AddWithValue("nom", vaccination.Vaccin.Nom);
            vaccinId = cmdInsert.ExecuteScalar();
        }
        
        using var cmd = new NpgsqlCommand(
            @"INSERT INTO VACCINATION (vaccination_date, vac_animal, id_vaccin)
              VALUES (@date, @animal, @vaccin)",
            conn);
        
        cmd.Parameters.AddWithValue("date", vaccination.DateVaccination);
        cmd.Parameters.AddWithValue("animal", vaccination.Animal.Identifiant);
        cmd.Parameters.AddWithValue("vaccin", (int)vaccinId);
        
        cmd.ExecuteNonQuery();
    }

    public static List<Vaccination> ListerParAnimal(string animalId)
    {
        var vaccinations = new List<Vaccination>();
        
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        using var cmd = new NpgsqlCommand(
            @"SELECT vaccination_date, vac_animal, v.nom
              FROM VACCINATION vac
              INNER JOIN VACCIN v ON vac.id_vaccin = v.identifiant
              WHERE vac.vac_animal = @id
              ORDER BY vaccination_date DESC",
            conn);
        
        cmd.Parameters.AddWithValue("id", animalId);
        
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var vaccination = new Vaccination
            {
                DateVaccination = reader.GetDateTime(0),
                Animal = new Animal { Identifiant = reader.GetString(1) },
                Vaccin = new Vaccin { Nom = reader.GetString(2) }
            };
            
            vaccinations.Add(vaccination);
        }
        
        return vaccinations;
    }
}
