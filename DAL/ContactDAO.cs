using Npgsql;
using Metier;

namespace DAL;

public class ContactDAO
{
    public static void Ajouter(Contact contact)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        using var cmd = new NpgsqlCommand(
            @"INSERT INTO CONTACT (nom, prenom, rue, cp, localite, registre_national, GSM, telephone, email)
              VALUES (@nom, @prenom, @rue, @cp, @localite, @registre_national, @gsm, @telephone, @email)
              RETURNING contact_identifiant",
            conn);
        
        cmd.Parameters.AddWithValue("nom", contact.Nom);
        cmd.Parameters.AddWithValue("prenom", contact.Prenom);
        cmd.Parameters.AddWithValue("rue", contact.AdresseContact.Rue);
        cmd.Parameters.AddWithValue("cp", contact.AdresseContact.Cp);
        cmd.Parameters.AddWithValue("localite", contact.AdresseContact.Localite);
        cmd.Parameters.AddWithValue("registre_national", contact.RegistreNational);
        cmd.Parameters.AddWithValue("gsm", (object?)contact.Gsm ?? DBNull.Value);
        cmd.Parameters.AddWithValue("telephone", (object?)contact.Telephone ?? DBNull.Value);
        cmd.Parameters.AddWithValue("email", (object?)contact.Email ?? DBNull.Value);
        
        contact.Identifiant = (int)cmd.ExecuteScalar()!;
        
        // Ajouter les rôles
        foreach (var role in contact.Role)
        {
            AjouterRole(contact.Identifiant, role.IdRole);
        }
    }

    public static Contact? Consulter(int identifiant)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        using var cmd = new NpgsqlCommand(
            @"SELECT contact_identifiant, nom, prenom, rue, cp, localite, 
                     registre_national, GSM, telephone, email
              FROM CONTACT WHERE contact_identifiant = @id",
            conn);
        
        cmd.Parameters.AddWithValue("id", identifiant);
        
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            var contact = new Contact
            {
                Identifiant = reader.GetInt32(0),
                Nom = reader.GetString(1),
                Prenom = reader.GetString(2),
                AdresseContact = new Adresse
                {
                    Rue = reader.GetString(3),
                    Cp = reader.GetInt32(4),
                    Localite = reader.GetString(5)
                },
                RegistreNational = reader.GetString(6),
                Gsm = reader.IsDBNull(7) ? null : reader.GetString(7),
                Telephone = reader.IsDBNull(8) ? null : reader.GetString(8),
                Email = reader.IsDBNull(9) ? null : reader.GetString(9)
            };
            
            reader.Close();
            ChargerRoles(contact);
            return contact;
        }
        return null;
    }

    public static void Supprimer(int identifiant)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        using var cmd = new NpgsqlCommand("DELETE FROM CONTACT WHERE contact_identifiant = @id", conn);
        cmd.Parameters.AddWithValue("id", identifiant);
        cmd.ExecuteNonQuery();
    }

    public static void Modifier(Contact contact)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        using var cmd = new NpgsqlCommand(
            @"UPDATE CONTACT 
              SET nom = @nom, prenom = @prenom, rue = @rue, cp = @cp, 
                  localite = @localite, registre_national = @registre_national, 
                  GSM = @gsm, telephone = @telephone, email = @email
              WHERE contact_identifiant = @id",
            conn);
        
        cmd.Parameters.AddWithValue("id", contact.Identifiant);
        cmd.Parameters.AddWithValue("nom", contact.Nom);
        cmd.Parameters.AddWithValue("prenom", contact.Prenom);
        cmd.Parameters.AddWithValue("rue", contact.AdresseContact.Rue);
        cmd.Parameters.AddWithValue("cp", contact.AdresseContact.Cp);
        cmd.Parameters.AddWithValue("localite", contact.AdresseContact.Localite);
        cmd.Parameters.AddWithValue("registre_national", contact.RegistreNational);
        cmd.Parameters.AddWithValue("gsm", (object?)contact.Gsm ?? DBNull.Value);
        cmd.Parameters.AddWithValue("telephone", (object?)contact.Telephone ?? DBNull.Value);
        cmd.Parameters.AddWithValue("email", (object?)contact.Email ?? DBNull.Value);
        
        cmd.ExecuteNonQuery();
        
        // Mettre à jour les rôles
        SupprimerTousRoles(contact.Identifiant);
        foreach (var role in contact.Role)
        {
            AjouterRole(contact.Identifiant, role.IdRole);
        }
    }

    public static List<Contact> ListerTous()
    {
        var contacts = new List<Contact>();
        
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        using var cmd = new NpgsqlCommand(
            @"SELECT contact_identifiant, nom, prenom, rue, cp, localite, 
                     registre_national, GSM, telephone, email
              FROM CONTACT ORDER BY nom, prenom",
            conn);
        
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            contacts.Add(new Contact
            {
                Identifiant = reader.GetInt32(0),
                Nom = reader.GetString(1),
                Prenom = reader.GetString(2),
                AdresseContact = new Adresse
                {
                    Rue = reader.GetString(3),
                    Cp = reader.GetInt32(4),
                    Localite = reader.GetString(5)
                },
                RegistreNational = reader.GetString(6),
                Gsm = reader.IsDBNull(7) ? null : reader.GetString(7),
                Telephone = reader.IsDBNull(8) ? null : reader.GetString(8),
                Email = reader.IsDBNull(9) ? null : reader.GetString(9)
            });
        }
        
        return contacts;
    }

    private static void ChargerRoles(Contact contact)
    {
        contact.Role.Clear();
        
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        try
        {
            using var cmd = new NpgsqlCommand(
                @"SELECT r.rol_identifiant, r.rol_nom
                  FROM ROLE r
                  INNER JOIN PERSONNE_ROLE pr ON r.rol_identifiant = pr.rol_identifiant
                  WHERE pr.pers_identifiant = @id",
                conn);
            
            cmd.Parameters.AddWithValue("id", contact.Identifiant);
            
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                contact.Role.Add(new Role
                {
                    IdRole = reader.GetInt32(0),
                    NomRole = reader.GetString(1)
                });
            }
        }
        catch (NpgsqlException ex) when (ex.SqlState == "42P01")
        {
            // Table ROLE ou PERSONNE_ROLE n'existe pas - ignorer silencieusement
            // car cela peut arriver si les tables n'ont pas encore été créées
            // L'erreur sera capturée lors de ListerTous() dans GestionContact
        }
    }

    private static void AjouterRole(int contactId, int roleId)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        using var cmd = new NpgsqlCommand(
            "INSERT INTO PERSONNE_ROLE (pers_identifiant, rol_identifiant) VALUES (@contactId, @roleId)",
            conn);
        
        cmd.Parameters.AddWithValue("contactId", contactId);
        cmd.Parameters.AddWithValue("roleId", roleId);
        cmd.ExecuteNonQuery();
    }

    private static void SupprimerTousRoles(int contactId)
    {
        using var conn = ConnexionBD.GetConnection();
        conn.Open();
        
        using var cmd = new NpgsqlCommand(
            "DELETE FROM PERSONNE_ROLE WHERE pers_identifiant = @id",
            conn);
        
        cmd.Parameters.AddWithValue("id", contactId);
        cmd.ExecuteNonQuery();
    }
}
