using DAL;
using Metier;

namespace Presentation;

public class GestionAnimal
{
    public void Ajouter()
    {
        Console.WriteLine("=== AJOUT D'UN ANIMAL ===");
        
        Console.Write("Date d'entrée (format: yyyy-MM-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime dateEntree))
        {
            Console.WriteLine("Date invalide!");
            return;
        }

        Console.Write("Séquence (5 chiffres): ");
        string sequence = Console.ReadLine() ?? "";
        if (sequence.Length != 5 || !sequence.All(char.IsDigit))
        {
            Console.WriteLine("Séquence invalide! Doit contenir exactement 5 chiffres.");
            return;
        }

        string identifiant = dateEntree.ToString("yyMMdd") + sequence;

        Console.Write("Nom: ");
        string nom = Console.ReadLine() ?? "";

        Console.Write("Type (chat/chien): ");
        string type = Console.ReadLine() ?? "";
        if (type != "chat" && type != "chien")
        {
            Console.WriteLine("Type invalide!");
            return;
        }

        Console.Write("Sexe (M/F): ");
        string sexe = Console.ReadLine() ?? "";
        if (sexe != "M" && sexe != "F")
        {
            Console.WriteLine("Sexe invalide!");
            return;
        }

        Console.Write("Date de naissance (format: yyyy-MM-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime dateNaissance))
        {
            Console.WriteLine("Date invalide!");
            return;
        }

        Console.Write("Stérilisé (true/false): ");
        if (!bool.TryParse(Console.ReadLine(), out bool sterilise))
        {
            Console.WriteLine("Valeur invalide!");
            return;
        }

        DateTime? dateSterilisation = null;
        if (sterilise)
        {
            Console.Write("Date de stérilisation (format: yyyy-MM-dd): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime dateSter))
            {
                dateSterilisation = dateSter;
            }
        }

        Console.Write("Description (optionnel): ");
        string? description = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(description)) description = null;

        Console.Write("Particularités (optionnel): ");
        string? particularites = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(particularites)) particularites = null;

        var animal = new Animal(identifiant, nom, type, sexe, dateNaissance, sterilise)
        {
            DateSterilisation = dateSterilisation,
            Description = description,
            Particularite = particularites
        };

        Console.Write("Couleurs (séparées par des virgules): ");
        string? couleursStr = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(couleursStr))
        {
            animal.Couleurs = couleursStr.Split(',').Select(c => c.Trim()).ToList();
        }

        try
        {
            AnimalDAO.Ajouter(animal);
            Console.WriteLine($"Animal ajouté avec succès! ID: {identifiant}");
            
            Console.WriteLine("\nCréation de l'entrée au refuge...");
            Console.Write("Raison d'entrée (1=abandon, 2=errant, 3=deces_proprietaire, 4=saisie, 5=retour_adoption, 6=retour_famille_accueil): ");
            string? raisonChoix = Console.ReadLine();
            string raisonEntree = raisonChoix switch
            {
                "1" => "abandon",
                "2" => "errant",
                "3" => "deces_proprietaire",
                "4" => "saisie",
                "5" => "retour_adoption",
                "6" => "retour_famille_accueil",
                _ => "abandon"
            };
            
            Console.Write("Identifiant du contact pour l'entrée (optionnel, laissez vide si aucun): ");
            string? contactIdStr = Console.ReadLine();
            Contact? contactEntree = null;
            
            if (!string.IsNullOrWhiteSpace(contactIdStr) && int.TryParse(contactIdStr, out int contactId))
            {
                contactEntree = ContactDAO.Consulter(contactId);
                if (contactEntree == null)
                {
                    Console.WriteLine("Contact non trouvé, entrée créée sans contact.");
                }
            }
            
            var entree = new Entree(raisonEntree, dateEntree, animal, contactEntree ?? new Contact());
            EntreeDAO.Ajouter(entree);
            Console.WriteLine("Entrée au refuge créée avec succès!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'ajout: {ex.Message}");
        }
    }

    public void Consulter()
    {
        Console.WriteLine("=== CONSULTATION D'UN ANIMAL ===");
        Console.Write("Identifiant de l'animal: ");
        string identifiant = Console.ReadLine() ?? "";

        var animal = AnimalDAO.Consulter(identifiant);
        if (animal == null)
        {
            Console.WriteLine("Animal non trouvé!");
            return;
        }

        Console.WriteLine($"\n=== INFORMATIONS SUR L'ANIMAL ===");
        Console.WriteLine($"ID: {animal.Identifiant}");
        Console.WriteLine($"Nom: {animal.Nom}");
        Console.WriteLine($"Type: {animal.Type}");
        Console.WriteLine($"Sexe: {animal.Sexe}");
        Console.WriteLine($"Date de naissance: {animal.DateNaissance:dd/MM/yyyy}");
        Console.WriteLine($"Stérilisé: {(animal.Sterilise ? "Oui" : "Non")}");
        if (animal.DateSterilisation.HasValue)
            Console.WriteLine($"Date de stérilisation: {animal.DateSterilisation.Value:dd/MM/yyyy}");
        if (animal.DateDeces.HasValue)
            Console.WriteLine($"Date de décès: {animal.DateDeces.Value:dd/MM/yyyy}");
        if (!string.IsNullOrEmpty(animal.Description))
            Console.WriteLine($"Description: {animal.Description}");
        if (!string.IsNullOrEmpty(animal.Particularite))
            Console.WriteLine($"Particularités: {animal.Particularite}");
        
        Console.WriteLine($"\nCouleurs: {string.Join(", ", animal.Couleurs)}");
        
        Console.WriteLine("\nCompatibilités:");
        foreach (var comp in animal.Compatibilites)
        {
            Console.WriteLine($"  - {comp}");
        }
    }

    public void Supprimer()
    {
        Console.WriteLine("=== SUPPRESSION D'UN ANIMAL ===");
        Console.Write("Identifiant de l'animal: ");
        string identifiant = Console.ReadLine() ?? "";

        var animal = AnimalDAO.Consulter(identifiant);
        if (animal == null)
        {
            Console.WriteLine("Animal non trouvé!");
            return;
        }

        Console.Write($"Êtes-vous sûr de vouloir supprimer {animal.Nom}? (oui/non): ");
        string confirmation = Console.ReadLine() ?? "";
        
        if (confirmation.ToLower() != "oui")
        {
            Console.WriteLine("Suppression annulée.");
            return;
        }

        try
        {
            AnimalDAO.Supprimer(identifiant);
            Console.WriteLine("Animal supprimé avec succès!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la suppression: {ex.Message}");
        }
    }

    public void AjouterInformation()
    {
        Console.WriteLine("=== AJOUT D'INFORMATION SUR UN ANIMAL ===");
        Console.Write("Identifiant de l'animal: ");
        string identifiant = Console.ReadLine() ?? "";

        var animal = AnimalDAO.Consulter(identifiant);
        if (animal == null)
        {
            Console.WriteLine("Animal non trouvé!");
            return;
        }

        Console.WriteLine("\n1. Ajouter une compatibilité");
        Console.WriteLine("2. Modifier la description");
        Console.WriteLine("3. Modifier les particularités");
        Console.WriteLine("4. Ajouter une couleur");
        Console.Write("Votre choix: ");
        string choix = Console.ReadLine() ?? "";

        switch (choix)
        {
            case "1":
                AjouterCompatibilite(animal);
                break;
            case "2":
                ModifierDescription(animal);
                break;
            case "3":
                ModifierParticularites(animal);
                break;
            case "4":
                AjouterCouleur(animal);
                break;
            default:
                Console.WriteLine("Choix invalide!");
                break;
        }
    }

    private void AjouterCompatibilite(Animal animal)
    {
        Console.Write("Type de compatibilité (chat, chien, jeune enfant, enfant, jardin, poney): ");
        string type = Console.ReadLine() ?? "";

        Console.Write("Valeur (true/false): ");
        if (!bool.TryParse(Console.ReadLine(), out bool valeur))
        {
            Console.WriteLine("Valeur invalide!");
            return;
        }

        Console.Write("Description (optionnel): ");
        string? description = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(description)) description = null;

        var compatibilite = new Compatibilite(type, valeur, description);
        animal.Compatibilites.Add(compatibilite);
        
        try
        {
            AnimalDAO.AjouterCompatibilitePourAnimal(animal.Identifiant, compatibilite);
            Console.WriteLine("Compatibilité ajoutée avec succès!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur: {ex.Message}");
        }
    }

    private void ModifierDescription(Animal animal)
    {
        Console.Write("Nouvelle description: ");
        string? description = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(description)) description = null;
        AnimalDAO.ModifierDescription(animal.Identifiant, description);
        Console.WriteLine("Description modifiée avec succès!");
    }

    private void ModifierParticularites(Animal animal)
    {
        Console.Write("Nouvelles particularités: ");
        string? particularites = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(particularites)) particularites = null;
        AnimalDAO.ModifierParticularites(animal.Identifiant, particularites);
        Console.WriteLine("Particularités modifiées avec succès!");
    }

    private void AjouterCouleur(Animal animal)
    {
        Console.Write("Nom de la couleur: ");
        string couleur = Console.ReadLine() ?? "";
        
        if (string.IsNullOrWhiteSpace(couleur))
        {
            Console.WriteLine("Couleur invalide!");
            return;
        }

        try
        {
            AnimalDAO.AjouterCouleurPourAnimal(animal.Identifiant, couleur);
            Console.WriteLine("Couleur ajoutée avec succès!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur: {ex.Message}");
        }
    }

    public void SupprimerInformation()
    {
        Console.WriteLine("=== SUPPRESSION D'INFORMATION SUR UN ANIMAL ===");
        Console.Write("Identifiant de l'animal: ");
        string identifiant = Console.ReadLine() ?? "";

        var animal = AnimalDAO.Consulter(identifiant);
        if (animal == null)
        {
            Console.WriteLine("Animal non trouvé!");
            return;
        }

        Console.WriteLine("\n1. Supprimer une compatibilité");
        Console.WriteLine("2. Supprimer une couleur");
        Console.Write("Votre choix: ");
        string choix = Console.ReadLine() ?? "";

        switch (choix)
        {
            case "1":
                SupprimerCompatibilite(animal);
                break;
            case "2":
                SupprimerCouleur(animal);
                break;
            default:
                Console.WriteLine("Choix invalide!");
                break;
        }
    }

    private void SupprimerCompatibilite(Animal animal)
    {
        Console.WriteLine("\nCompatibilités de l'animal:");
        for (int i = 0; i < animal.Compatibilites.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {animal.Compatibilites[i]}");
        }

        Console.Write("Numéro de la compatibilité à supprimer: ");
        if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > animal.Compatibilites.Count)
        {
            Console.WriteLine("Numéro invalide!");
            return;
        }

        var compatibilite = animal.Compatibilites[index - 1];

        try
        {
            AnimalDAO.SupprimerCompatibilite(animal.Identifiant, compatibilite.Type);
            Console.WriteLine("Compatibilité supprimée avec succès!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur: {ex.Message}");
        }
    }

    private void SupprimerCouleur(Animal animal)
    {
        Console.WriteLine("\nCouleurs de l'animal:");
        for (int i = 0; i < animal.Couleurs.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {animal.Couleurs[i]}");
        }

        Console.Write("Numéro de la couleur à supprimer: ");
        if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > animal.Couleurs.Count)
        {
            Console.WriteLine("Numéro invalide!");
            return;
        }

        var couleur = animal.Couleurs[index - 1];

        try
        {
            AnimalDAO.SupprimerCouleur(animal.Identifiant, couleur);
            Console.WriteLine("Couleur supprimée avec succès!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur: {ex.Message}");
        }
    }
}
