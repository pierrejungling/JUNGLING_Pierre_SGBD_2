using DAL;
using Metier;

namespace Presentation;

public class GestionContact
{
    public void Ajouter()
    {
        Console.WriteLine("=== AJOUT D'UN CONTACT ===");
        
        Console.Write("Nom: ");
        string nom = Console.ReadLine() ?? "";
        if (nom.Length < 2)
        {
            Console.WriteLine("Le nom doit contenir au moins 2 caractères!");
            return;
        }

        Console.Write("Prénom: ");
        string prenom = Console.ReadLine() ?? "";
        if (prenom.Length < 2)
        {
            Console.WriteLine("Le prénom doit contenir au moins 2 caractères!");
            return;
        }

        Console.Write("Registre national (format: yy.mm.dd-999.99): ");
        string registreNational = Console.ReadLine() ?? "";

        Console.Write("Rue: ");
        string rue = Console.ReadLine() ?? "";

        Console.Write("Code postal: ");
        if (!int.TryParse(Console.ReadLine(), out int codePostal))
        {
            Console.WriteLine("Code postal invalide!");
            return;
        }

        Console.Write("Localité: ");
        string localite = Console.ReadLine() ?? "";

        Console.Write("GSM (optionnel): ");
        string? gsm = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(gsm)) gsm = null;

        Console.Write("Téléphone (optionnel): ");
        string? telephone = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(telephone)) telephone = null;

        Console.Write("Email (optionnel): ");
        string? email = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(email)) email = null;

        if (gsm == null && telephone == null && email == null)
        {
            Console.WriteLine("Au moins un moyen de contact (GSM, téléphone ou email) doit être fourni!");
            return;
        }

        var roles = RoleDAO.ListerTous();
        Console.WriteLine("\nRôles disponibles:");
        foreach (var role in roles)
        {
            Console.WriteLine($"{role.IdRole}. {role.NomRole}");
        }

        Console.Write("IDs des rôles (séparés par des virgules): ");
        string? rolesStr = Console.ReadLine();
        var contactRoles = new List<Role>();
        
        if (!string.IsNullOrWhiteSpace(rolesStr))
        {
            var roleIds = rolesStr.Split(',');
            foreach (var roleIdStr in roleIds)
            {
                if (int.TryParse(roleIdStr.Trim(), out int roleId))
                {
                    var role = roles.FirstOrDefault(r => r.IdRole == roleId);
                    if (role != null)
                        contactRoles.Add(role);
                }
            }
        }

        var adresse = new Adresse(rue, codePostal, localite);
        var contact = new Contact(0, nom, prenom, adresse, registreNational, gsm, telephone, email)
        {
            Role = contactRoles
        };

        try
        {
            ContactDAO.Ajouter(contact);
            Console.WriteLine($"Contact ajouté avec succès! ID: {contact.Identifiant}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'ajout: {ex.Message}");
        }
    }

    public void Consulter()
    {
        Console.WriteLine("=== CONSULTATION D'UN CONTACT ===");
        Console.Write("Identifiant du contact: ");
        if (!int.TryParse(Console.ReadLine(), out int identifiant))
        {
            Console.WriteLine("ID invalide!");
            return;
        }

        var contact = ContactDAO.Consulter(identifiant);
        if (contact == null)
        {
            Console.WriteLine("Contact non trouvé!");
            return;
        }

        Console.WriteLine($"\n=== INFORMATIONS SUR LE CONTACT ===");
        Console.WriteLine($"ID: {contact.Identifiant}");
        Console.WriteLine($"Nom: {contact.Nom}");
        Console.WriteLine($"Prénom: {contact.Prenom}");
        Console.WriteLine($"Registre national: {contact.RegistreNational}");
        Console.WriteLine($"Adresse: {contact.AdresseContact}");
        if (!string.IsNullOrEmpty(contact.Gsm))
            Console.WriteLine($"GSM: {contact.Gsm}");
        if (!string.IsNullOrEmpty(contact.Telephone))
            Console.WriteLine($"Téléphone: {contact.Telephone}");
        if (!string.IsNullOrEmpty(contact.Email))
            Console.WriteLine($"Email: {contact.Email}");
        Console.WriteLine($"Rôles: {string.Join(", ", contact.Role.Select(r => r.NomRole))}");
    }

    public void Modifier()
    {
        Console.WriteLine("=== MODIFICATION D'UN CONTACT ===");
        Console.Write("Identifiant du contact: ");
        if (!int.TryParse(Console.ReadLine(), out int identifiant))
        {
            Console.WriteLine("ID invalide!");
            return;
        }

        var contact = ContactDAO.Consulter(identifiant);
        if (contact == null)
        {
            Console.WriteLine("Contact non trouvé!");
            return;
        }

        Console.WriteLine($"\nContact actuel: {contact.Prenom} {contact.Nom}");

        Console.Write("Nouveau nom (laissez vide pour ne pas modifier): ");
        string? nouveauNom = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(nouveauNom))
            contact.Nom = nouveauNom;

        Console.Write("Nouveau prénom (laissez vide pour ne pas modifier): ");
        string? nouveauPrenom = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(nouveauPrenom))
            contact.Prenom = nouveauPrenom;

        Console.Write("Nouvelle rue (laissez vide pour ne pas modifier): ");
        string? nouvelleRue = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(nouvelleRue))
            contact.AdresseContact.Rue = nouvelleRue;

        Console.Write("Nouveau code postal (laissez vide pour ne pas modifier): ");
        string? nouveauCPStr = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(nouveauCPStr) && int.TryParse(nouveauCPStr, out int nouveauCP))
            contact.AdresseContact.Cp = nouveauCP;

        Console.Write("Nouvelle localité (laissez vide pour ne pas modifier): ");
        string? nouvelleLocalite = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(nouvelleLocalite))
            contact.AdresseContact.Localite = nouvelleLocalite;

        Console.Write("Nouveau GSM (laissez vide pour ne pas modifier): ");
        string? nouveauGSM = Console.ReadLine();
        contact.Gsm = string.IsNullOrWhiteSpace(nouveauGSM) ? contact.Gsm : nouveauGSM;

        Console.Write("Nouveau téléphone (laissez vide pour ne pas modifier): ");
        string? nouveauTelephone = Console.ReadLine();
        contact.Telephone = string.IsNullOrWhiteSpace(nouveauTelephone) ? contact.Telephone : nouveauTelephone;

        Console.Write("Nouvel email (laissez vide pour ne pas modifier): ");
        string? nouvelEmail = Console.ReadLine();
        contact.Email = string.IsNullOrWhiteSpace(nouvelEmail) ? contact.Email : nouvelEmail;

        if (contact.Gsm == null && contact.Telephone == null && contact.Email == null)
        {
            Console.WriteLine("Au moins un moyen de contact doit être fourni!");
            return;
        }

        try
        {
            ContactDAO.Modifier(contact);
            Console.WriteLine("Contact modifié avec succès!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la modification: {ex.Message}");
        }
    }

    public void Supprimer()
    {
        Console.WriteLine("=== SUPPRESSION D'UN CONTACT ===");
        Console.Write("Identifiant du contact: ");
        if (!int.TryParse(Console.ReadLine(), out int identifiant))
        {
            Console.WriteLine("ID invalide!");
            return;
        }

        var contact = ContactDAO.Consulter(identifiant);
        if (contact == null)
        {
            Console.WriteLine("Contact non trouvé!");
            return;
        }

        Console.Write($"Êtes-vous sûr de vouloir supprimer {contact.Prenom} {contact.Nom}? (oui/non): ");
        string confirmation = Console.ReadLine() ?? "";
        
        if (confirmation.ToLower() != "oui")
        {
            Console.WriteLine("Suppression annulée.");
            return;
        }

        try
        {
            ContactDAO.Supprimer(identifiant);
            Console.WriteLine("Contact supprimé avec succès!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la suppression: {ex.Message}");
        }
    }
}
