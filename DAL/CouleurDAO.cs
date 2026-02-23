using Npgsql;

namespace DAL;

public static class CouleurDAO
{
    public static List<string> ListerToutes()
    {
        var liste = new List<string>();
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        using var cmd = new NpgsqlCommand("SELECT * FROM couleur_lister_toutes()", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            liste.Add(reader.GetString(0));
        return liste;
    }
}
