using Npgsql;
using Metier;

namespace DAL;

public class RoleDAO
{
    public static List<Role> ListerTous()
    {
        var roles = new List<Role>();
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        try
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM role_lister_tous()", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                roles.Add(new Role
                {
                    IdRole = reader.GetInt32(0),
                    NomRole = reader.GetString(1)
                });
            }
        }
        catch (NpgsqlException ex) when (ex.SqlState == "42P01")
        {
            throw new InvalidOperationException(
                "La table ROLE n'existe pas dans la base de données.\n" +
                "Vérifiez que le script SQL de création des tables a été exécuté complètement.",
                ex);
        }
        return roles;
    }
}
