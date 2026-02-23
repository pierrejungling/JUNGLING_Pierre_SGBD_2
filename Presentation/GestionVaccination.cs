using DAL;
using Metier;

namespace Presentation;

public class GestionVaccination
{
    public void Ajouter()
    {
        Console.WriteLine("=== AJOUT D'UN VACCIN À UN ANIMAL ===");
        
        Console.Write("Identifiant de l'animal: ");
        string animalId = Console.ReadLine() ?? "";
        var animal = AnimalDAO.Consulter(animalId);
        if (animal == null)
        {
            Console.WriteLine("Animal non trouvé!");
            return;
        }

        Console.Write("Nom du vaccin: ");
        string nomVaccin = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(nomVaccin))
        {
            Console.WriteLine("Nom du vaccin invalide!");
            return;
        }

        Console.Write("Date de vaccination (format: yyyy-MM-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime dateVaccination))
        {
            Console.WriteLine("Date invalide!");
            return;
        }

        if (dateVaccination < animal.DateNaissance)
        {
            Console.WriteLine("La date de vaccination doit être supérieure ou égale à la date de naissance!");
            return;
        }

        var vaccin = new Vaccin(nomVaccin);
        var vaccination = new Vaccination(dateVaccination, animal, vaccin);

        try
        {
            VaccinationDAO.Ajouter(vaccination);
            Console.WriteLine("Vaccination ajoutée avec succès!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'ajout: {ex.Message}");
        }
    }

    public void Consulter()
    {
        Console.WriteLine("=== CONSULTATION DES VACCINATIONS D'UN ANIMAL ===");
        Console.Write("Identifiant de l'animal: ");
        string animalId = Console.ReadLine() ?? "";

        var animal = AnimalDAO.Consulter(animalId);
        if (animal == null)
        {
            Console.WriteLine("Animal non trouvé!");
            return;
        }

        var vaccinations = VaccinationDAO.ListerParAnimal(animalId);
        
        if (vaccinations.Count == 0)
        {
            Console.WriteLine($"Aucune vaccination enregistrée pour {animal.Nom}.");
            return;
        }

        Console.WriteLine($"\nVaccinations de {animal.Nom}:");
        foreach (var vaccination in vaccinations)
        {
            Console.WriteLine($"- {vaccination.Vaccin.Nom} le {vaccination.DateVaccination:dd/MM/yyyy}");
        }
    }
}
