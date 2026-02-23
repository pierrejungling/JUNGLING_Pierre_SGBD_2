using Npgsql;
using NpgsqlTypes;
using Metier;

namespace DAL;

public class VaccinationDAO
{
    public static void Ajouter(Vaccination vaccination)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        using var cmd = new NpgsqlCommand(
            "SELECT vaccination_ajouter(@date, @animal, @nom_vaccin)",
            conn);
        var pDate = new NpgsqlParameter("date", NpgsqlDbType.Date);
        pDate.Value = vaccination.DateVaccination.Date;
        cmd.Parameters.Add(pDate);
        cmd.Parameters.AddWithValue("animal", vaccination.Animal.Identifiant);
        cmd.Parameters.AddWithValue("nom_vaccin", vaccination.Vaccin.Nom);
        cmd.ExecuteNonQuery();
    }

    public static List<Vaccination> ListerParAnimal(string animalId)
    {
        var vaccinations = new List<Vaccination>();
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM vaccination_lister_par_animal(@id)", conn);
        cmd.Parameters.AddWithValue("id", animalId);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            vaccinations.Add(new Vaccination
            {
                DateVaccination = reader.GetDateTime(0),
                Animal = new Animal { Identifiant = reader.GetString(1) },
                Vaccin = new Vaccin { Nom = reader.GetString(2) }
            });
        }
        return vaccinations;
    }
}
