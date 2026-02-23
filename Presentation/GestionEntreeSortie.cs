using DAL;
using Metier;
using Npgsql;

namespace Presentation;

public class GestionEntreeSortie
{
    public void AjouterEntree()
    {
        Console.WriteLine("=== AJOUT D'UNE ENTRÉE AU REFUGE ===");
        
        Console.Write("Identifiant de l'animal: ");
        string animalId = Console.ReadLine() ?? "";
        var animal = AnimalDAO.Consulter(animalId);
        if (animal == null)
        {
            Console.WriteLine("Animal non trouvé!");
            return;
        }

        Console.WriteLine("Raisons disponibles:");
        Console.WriteLine("1. abandon");
        Console.WriteLine("2. errant");
        Console.WriteLine("3. deces_proprietaire");
        Console.WriteLine("4. saisie");
        Console.WriteLine("5. retour_adoption");
        Console.WriteLine("6. retour_famille_accueil");
        Console.Write("Raison (1-6): ");
        string? raisonChoix = Console.ReadLine();
        string raison = raisonChoix switch
        {
            "1" => "abandon",
            "2" => "errant",
            "3" => "deces_proprietaire",
            "4" => "saisie",
            "5" => "retour_adoption",
            "6" => "retour_famille_accueil",
            _ => "abandon"
        };

        Console.Write("Date d'entrée (format: yyyy-MM-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime dateEntree))
        {
            Console.WriteLine("Date invalide!");
            return;
        }

        Console.Write("Identifiant du contact (optionnel, laissez vide si aucun): ");
        string? contactIdStr = Console.ReadLine();
        Contact? contact = null;
        
        if (!string.IsNullOrWhiteSpace(contactIdStr) && int.TryParse(contactIdStr, out int contactId))
        {
            contact = ContactDAO.Consulter(contactId);
            if (contact == null)
            {
                Console.WriteLine("Contact non trouvé, entrée créée sans contact.");
            }
        }

        var entree = new Entree(raison, dateEntree, animal, contact ?? new Contact());

        try
        {
            EntreeDAO.Ajouter(entree);
            Console.WriteLine("Entrée ajoutée avec succès!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'ajout: {ex.Message}");
        }
    }

    public void AjouterSortie()
    {
        Console.WriteLine("=== AJOUT D'UNE SORTIE DU REFUGE ===");
        
        Console.Write("Identifiant de l'animal: ");
        string animalId = Console.ReadLine() ?? "";
        var animal = AnimalDAO.Consulter(animalId);
        if (animal == null)
        {
            Console.WriteLine("Animal non trouvé!");
            return;
        }

        Console.WriteLine("Raisons disponibles:");
        Console.WriteLine("1. adoption");
        Console.WriteLine("2. retour_proprietaire");
        Console.WriteLine("3. deces_animal");
        Console.WriteLine("4. famille_accueil");
        Console.Write("Raison (1-4): ");
        string? raisonChoix = Console.ReadLine();
        string raison = raisonChoix switch
        {
            "1" => "adoption",
            "2" => "retour_proprietaire",
            "3" => "deces_animal",
            "4" => "famille_accueil",
            _ => "adoption"
        };

        Console.Write("Date de sortie (format: yyyy-MM-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime dateSortie))
        {
            Console.WriteLine("Date invalide!");
            return;
        }

        DateTime? dateDecesPourProcedure = null;
        if (raison == "deces_animal")
        {
            Console.Write("Mettre à jour la date de décès de l'animal? (oui/non): ");
            string? confirmer = Console.ReadLine();
            if (confirmer?.ToLower() == "oui")
                dateDecesPourProcedure = dateSortie;
        }

        Console.Write("Identifiant du contact (optionnel, laissez vide si aucun): ");
        string? contactIdStr = Console.ReadLine();
        Contact? contact = null;
        
        if (!string.IsNullOrWhiteSpace(contactIdStr) && int.TryParse(contactIdStr, out int contactId))
        {
            contact = ContactDAO.Consulter(contactId);
            if (contact == null)
            {
                Console.WriteLine("Contact non trouvé, sortie créée sans contact.");
            }
        }

        var sortie = new Sortie(raison, dateSortie, animal, contact ?? new Contact());

        try
        {
            SortieDAO.Ajouter(sortie, dateDecesPourProcedure);
            Console.WriteLine("Sortie ajoutée avec succès!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'ajout: {ex.Message}");
        }
    }

    public void ListerEntrees()
    {
        Console.WriteLine("=== LISTE DES ENTRÉES D'UN ANIMAL ===");
        Console.Write("Identifiant de l'animal: ");
        string animalId = Console.ReadLine() ?? "";

        var animal = AnimalDAO.Consulter(animalId);
        if (animal == null)
        {
            Console.WriteLine("Animal non trouvé!");
            return;
        }

        var entrees = EntreeDAO.ListerParAnimal(animalId);
        
        if (entrees.Count == 0)
        {
            Console.WriteLine($"Aucune entrée enregistrée pour {animal.Nom}.");
            return;
        }

        Console.WriteLine($"\nEntrées de {animal.Nom}:");
        foreach (var entree in entrees)
        {
            Console.WriteLine($"- {entree.DateEntree:dd/MM/yyyy} - {entree.Raison}");
            if (entree.Contact.Identifiant > 0)
            {
                Console.WriteLine($"  Contact: {entree.Contact.Prenom} {entree.Contact.Nom}");
            }
        }
    }

    public void ListerSorties()
    {
        Console.WriteLine("=== LISTE DES SORTIES D'UN ANIMAL ===");
        Console.Write("Identifiant de l'animal: ");
        string animalId = Console.ReadLine() ?? "";

        var animal = AnimalDAO.Consulter(animalId);
        if (animal == null)
        {
            Console.WriteLine("Animal non trouvé!");
            return;
        }

        var sorties = SortieDAO.ListerParAnimal(animalId);
        
        if (sorties.Count == 0)
        {
            Console.WriteLine($"Aucune sortie enregistrée pour {animal.Nom}.");
            return;
        }

        Console.WriteLine($"\nSorties de {animal.Nom}:");
        foreach (var sortie in sorties)
        {
            Console.WriteLine($"- {sortie.DateSortie:dd/MM/yyyy} - {sortie.Raison}");
            if (sortie.Contact.Identifiant > 0)
            {
                Console.WriteLine($"  Contact: {sortie.Contact.Prenom} {sortie.Contact.Nom}");
            }
        }
    }
}
