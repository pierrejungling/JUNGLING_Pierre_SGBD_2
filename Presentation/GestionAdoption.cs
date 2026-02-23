using DAL;
using Metier;

namespace Presentation;

public class GestionAdoption
{
    public void Lister()
    {
        Console.WriteLine("=== LISTE DES ADOPTIONS ===");
        var adoptions = AdoptionDAO.ListerToutes();
        
        if (adoptions.Count == 0)
        {
            Console.WriteLine("Aucune adoption enregistrée.");
            return;
        }

        foreach (var adoption in adoptions)
        {
            Console.WriteLine($"\n- {adoption.AnimalAdopte.Nom} adopté par {adoption.Adoptant.Prenom} {adoption.Adoptant.Nom}");
            Console.WriteLine($"  Date de demande: {adoption.DateDemande:dd/MM/yyyy}");
            Console.WriteLine($"  Statut: {adoption.Statut}");
        }
        
        Console.WriteLine($"\nTotal: {adoptions.Count} adoption(s)");
    }

    public void Ajouter()
    {
        Console.WriteLine("=== AJOUT D'UNE ADOPTION ===");
        
        Console.Write("Identifiant de l'animal: ");
        string animalId = Console.ReadLine() ?? "";
        var animal = AnimalDAO.Consulter(animalId);
        if (animal == null)
        {
            Console.WriteLine("Animal non trouvé!");
            return;
        }

        // Vérifier qu'il n'y a pas déjà une adoption (relation 1-1)
        var adoptionExistante = AdoptionDAO.ConsulterParAnimal(animalId);
        if (adoptionExistante != null)
        {
            Console.WriteLine($"Cet animal a déjà une adoption (relation 1-1). Statut actuel: {adoptionExistante.Statut}");
            return;
        }

        Console.Write("Identifiant du contact (adoptant): ");
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

        Console.Write("Date de demande (format: yyyy-MM-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime dateDemande))
        {
            Console.WriteLine("Date invalide!");
            return;
        }

        Console.WriteLine("Statuts disponibles:");
        Console.WriteLine("1. demande");
        Console.WriteLine("2. acceptee");
        Console.WriteLine("3. rejet_environnement");
        Console.WriteLine("4. rejet_comportement");
        Console.Write("Statut (1-4): ");
        string? statutChoix = Console.ReadLine();
        string statut = statutChoix switch
        {
            "1" => "demande",
            "2" => "acceptee",
            "3" => "rejet_environnement",
            "4" => "rejet_comportement",
            _ => "demande"
        };

        var adoption = new Adoption(statut, dateDemande, animal, contact);

        try
        {
            AdoptionDAO.Ajouter(adoption);
            Console.WriteLine("Adoption ajoutée avec succès!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'ajout: {ex.Message}");
        }
    }

    public void ModifierStatut()
    {
        Console.WriteLine("=== MODIFICATION DU STATUT D'UNE ADOPTION ===");
        
        Console.Write("Identifiant de l'animal: ");
        string animalId = Console.ReadLine() ?? "";

        var adoption = AdoptionDAO.ConsulterParAnimal(animalId);
        if (adoption == null)
        {
            Console.WriteLine("Aucune adoption trouvée pour cet animal!");
            return;
        }

        Console.WriteLine($"Statut actuel: {adoption.Statut}");
        Console.WriteLine("Nouveaux statuts disponibles:");
        Console.WriteLine("1. demande");
        Console.WriteLine("2. acceptee");
        Console.WriteLine("3. rejet_environnement");
        Console.WriteLine("4. rejet_comportement");
        Console.Write("Nouveau statut (1-4): ");
        string? statutChoix = Console.ReadLine();
        string nouveauStatut = statutChoix switch
        {
            "1" => "demande",
            "2" => "acceptee",
            "3" => "rejet_environnement",
            "4" => "rejet_comportement",
            _ => "demande"
        };

        try
        {
            AdoptionDAO.ModifierStatut(animalId, nouveauStatut);
            Console.WriteLine("Statut modifié avec succès!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la modification: {ex.Message}");
        }
    }
}
