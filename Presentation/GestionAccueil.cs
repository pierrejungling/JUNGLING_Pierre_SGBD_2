using DAL;
using Metier;

namespace Presentation;

public class GestionAccueil
{
    public void ListerParAnimal()
    {
        Console.WriteLine("=== FAMILLES D'ACCUEIL PAR ANIMAL ===");
        Console.Write("Identifiant de l'animal: ");
        string animalId = Console.ReadLine() ?? "";

        var animal = AnimalDAO.Consulter(animalId);
        if (animal == null)
        {
            Console.WriteLine("Animal non trouvé!");
            return;
        }

        var accueils = AccueilDAO.ListerParAnimal(animalId);
        
        if (accueils.Count == 0)
        {
            Console.WriteLine($"Aucune famille d'accueil pour {animal.Nom}.");
            return;
        }

        Console.WriteLine($"\nFamilles d'accueil pour {animal.Nom}:");
        foreach (var accueil in accueils)
        {
            string dateFinStr = accueil.DateFin.HasValue ? accueil.DateFin.Value.ToString("dd/MM/yyyy") : "En cours";
            Console.WriteLine($"- {accueil.FamilleAccueil.Prenom} {accueil.FamilleAccueil.Nom}");
            Console.WriteLine($"  Du {accueil.DateDebut:dd/MM/yyyy} au {dateFinStr}");
        }
    }

    public void ListerParFamille()
    {
        Console.WriteLine("=== ANIMAUX ACCUEILLIS PAR UNE FAMILLE ===");
        Console.Write("Identifiant du contact (famille d'accueil): ");
        if (!int.TryParse(Console.ReadLine(), out int contactId))
        {
            Console.WriteLine("ID invalide!");
            return;
        }

        var contact = ContactDAO.Consulter(contactId);
        if (contact == null)
        {
            Console.WriteLine("Contact non trouvé!");
            return;
        }

        var accueils = AccueilDAO.ListerParFamille(contactId);
        
        if (accueils.Count == 0)
        {
            Console.WriteLine($"Aucun animal accueilli par {contact.Prenom} {contact.Nom}.");
            return;
        }

        Console.WriteLine($"\nAnimaux accueillis par {contact.Prenom} {contact.Nom}:");
        foreach (var accueil in accueils)
        {
            string dateFinStr = accueil.DateFin.HasValue ? accueil.DateFin.Value.ToString("dd/MM/yyyy") : "En cours";
            Console.WriteLine($"- {accueil.AnimalAccueilli.Nom} ({accueil.AnimalAccueilli.Type})");
            Console.WriteLine($"  Du {accueil.DateDebut:dd/MM/yyyy} au {dateFinStr}");
        }
    }

    public void Ajouter()
    {
        Console.WriteLine("=== AJOUT D'UNE FAMILLE D'ACCUEIL ===");
        
        Console.Write("Identifiant de l'animal: ");
        string animalId = Console.ReadLine() ?? "";
        var animal = AnimalDAO.Consulter(animalId);
        if (animal == null)
        {
            Console.WriteLine("Animal non trouvé!");
            return;
        }

        Console.Write("Identifiant du contact (famille d'accueil): ");
        if (!int.TryParse(Console.ReadLine(), out int contactId))
        {
            Console.WriteLine("ID invalide!");
            return;
        }
        var contact = ContactDAO.Consulter(contactId);
        if (contact == null)
        {
            Console.WriteLine("Contact non trouvé!");
            return;
        }

        Console.Write("Date de début (format: yyyy-MM-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime dateDebut))
        {
            Console.WriteLine("Date invalide!");
            return;
        }

        Console.Write("Date de fin (format: yyyy-MM-dd, laissez vide si en cours): ");
        string? dateFinStr = Console.ReadLine();
        DateTime? dateFin = null;
        if (!string.IsNullOrWhiteSpace(dateFinStr))
        {
            if (DateTime.TryParse(dateFinStr, out DateTime df))
            {
                dateFin = df;
            }
        }

        if (dateFin.HasValue && dateFin.Value < dateDebut)
        {
            Console.WriteLine("La date de fin doit être supérieure ou égale à la date de début!");
            return;
        }

        var accueil = new Accueil(dateDebut, dateFin, animal, contact);

        try
        {
            AccueilDAO.Ajouter(accueil);
            Console.WriteLine("Famille d'accueil ajoutée avec succès!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'ajout: {ex.Message}");
        }
    }
}
