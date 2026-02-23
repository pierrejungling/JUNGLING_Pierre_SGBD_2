using DAL;
using Metier;
using Npgsql;

namespace Presentation;

public class MenuPrincipal
{
    private readonly GestionAnimal gestionAnimal = new();
    private readonly GestionContact gestionContact = new();
    private readonly GestionAdoption gestionAdoption = new();
    private readonly GestionAccueil gestionAccueil = new();
    private readonly GestionVaccination gestionVaccination = new();
    private readonly GestionEntreeSortie gestionEntreeSortie = new();

    public void Afficher()
    {
        while (true)
        {
            Console.WriteLine("\n=== MENU PRINCIPAL ===");
            Console.WriteLine("1. Gestion des animaux");
            Console.WriteLine("2. Gestion des contacts");
            Console.WriteLine("3. Gestion des adoptions");
            Console.WriteLine("4. Gestion des familles d'accueil");
            Console.WriteLine("5. Gestion des vaccinations");
            Console.WriteLine("6. Gestion des entrées/sorties");
            Console.WriteLine("7. Lister les animaux présents au refuge");
            Console.WriteLine("0. Quitter");
            Console.Write("\nVotre choix: ");

            string? choix = null;
            try
            {
                choix = Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErreur lors de la lecture de l'entrée: {ex.Message}");
                Console.WriteLine($"Type: {ex.GetType().Name}");
                Console.WriteLine($"Code: 0x{ex.HResult:X8}");
                Console.WriteLine("\nConseil: Essayez de lancer l'application depuis un terminal (Terminal > New Terminal)");
                Console.WriteLine("au lieu de la console de débogage.");
                Console.WriteLine("\nAppuyez sur Entrée pour continuer...");
                try { Console.ReadLine(); } catch { }
                continue;
            }
            
            if (choix == null)
            {
                Console.WriteLine("\nAucune entrée reçue. Vérifiez que la console est correctement configurée.");
                Console.WriteLine("Essayez de lancer l'application depuis un terminal (Terminal > New Terminal).");
                continue;
            }
            
            Console.WriteLine();

            try
            {
                switch (choix)
                {
                    case "1":
                        try
                        {
                            MenuGestionAnimal();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\n*** ERREUR LORS DE L'APPEL AU MENU GESTION ANIMAUX ***");
                            Console.WriteLine($"Message: {ex.Message}");
                            Console.WriteLine($"Type: {ex.GetType().FullName}");
                            Console.WriteLine($"Code erreur: 0x{ex.HResult:X8}");
                            if (ex is NpgsqlException npgsqlEx)
                            {
                                Console.WriteLine($"Code SQL: {npgsqlEx.SqlState}");
                            }
                            if (ex.InnerException != null)
                            {
                                Console.WriteLine($"Erreur interne: {ex.InnerException.Message}");
                            }
                            Console.WriteLine($"\nStack trace:\n{ex.StackTrace}");
                            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                            try { Console.ReadKey(true); } catch { }
                        }
                        break;
                    case "2":
                        MenuGestionContact();
                        break;
                    case "3":
                        MenuGestionAdoption();
                        break;
                    case "4":
                        MenuGestionAccueil();
                        break;
                    case "5":
                        MenuGestionVaccination();
                        break;
                    case "6":
                        MenuGestionEntreeSortie();
                        break;
                    case "7":
                        ListerAnimauxAuRefuge();
                        break;
                    case "0":
                        Console.WriteLine("Au revoir!");
                        return;
                    default:
                        Console.WriteLine("Choix invalide!");
                        break;
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"\nErreur de connexion PostgreSQL: {ex.Message}");
                Console.WriteLine($"Code SQL: {ex.SqlState}");
                Console.WriteLine($"Code erreur: {ex.HResult:X8}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Détails: {ex.InnerException.Message}");
                }
                Console.WriteLine("\nVérifiez que la base de données est accessible.");
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                try { Console.ReadKey(true); } catch { }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErreur: {ex.Message}");
                Console.WriteLine($"Type: {ex.GetType().Name}");
                Console.WriteLine($"Code erreur: 0x{ex.HResult:X8}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Détails: {ex.InnerException.Message}");
                }
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                try { Console.ReadKey(true); } catch { }
            }
        }
    }

    private void MenuGestionAnimal()
    {
        while (true)
        {
            try
            {
                Console.WriteLine("\n=== GESTION DES ANIMAUX ===");
                Console.WriteLine("1. Ajouter un animal");
                Console.WriteLine("2. Consulter un animal");
                Console.WriteLine("3. Supprimer un animal");
                Console.WriteLine("4. Ajouter une information (compatibilité, description, particularité)");
                Console.WriteLine("5. Supprimer une information");
                Console.WriteLine("0. Retour");
                Console.Write("\nVotre choix: ");

                string? choix = Console.ReadLine();
                Console.WriteLine();

                switch (choix)
                {
                    case "1":
                        gestionAnimal.Ajouter();
                        break;
                    case "2":
                        gestionAnimal.Consulter();
                        break;
                    case "3":
                        gestionAnimal.Supprimer();
                        break;
                    case "4":
                        gestionAnimal.AjouterInformation();
                        break;
                    case "5":
                        gestionAnimal.SupprimerInformation();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Choix invalide!");
                        break;
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"\nErreur de connexion PostgreSQL: {ex.Message}");
                Console.WriteLine($"Code SQL: {ex.SqlState}");
                Console.WriteLine($"Code erreur: {ex.HResult:X8}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Détails: {ex.InnerException.Message}");
                }
                Console.WriteLine("\nVérifiez que la base de données est accessible.");
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                try { Console.ReadKey(true); } catch { }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErreur: {ex.Message}");
                Console.WriteLine($"Type: {ex.GetType().Name}");
                Console.WriteLine($"Code erreur: 0x{ex.HResult:X8}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Détails: {ex.InnerException.Message}");
                }
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                try { Console.ReadKey(true); } catch { }
            }
        }
    }

    private void MenuGestionContact()
    {
        while (true)
        {
            Console.WriteLine("\n=== GESTION DES CONTACTS ===");
            Console.WriteLine("1. Ajouter un contact");
            Console.WriteLine("2. Consulter un contact");
            Console.WriteLine("3. Modifier les coordonnées d'un contact");
            Console.WriteLine("4. Supprimer un contact");
            Console.WriteLine("0. Retour");
            Console.Write("\nVotre choix: ");

            string? choix = Console.ReadLine();
            Console.WriteLine();

            switch (choix)
            {
                case "1":
                    gestionContact.Ajouter();
                    break;
                case "2":
                    gestionContact.Consulter();
                    break;
                case "3":
                    gestionContact.Modifier();
                    break;
                case "4":
                    gestionContact.Supprimer();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Choix invalide!");
                    break;
            }
        }
    }

    private void MenuGestionAdoption()
    {
        while (true)
        {
            Console.WriteLine("\n=== GESTION DES ADOPTIONS ===");
            Console.WriteLine("1. Lister les adoptions et leur statut");
            Console.WriteLine("2. Ajouter une adoption");
            Console.WriteLine("3. Modifier le statut d'une adoption");
            Console.WriteLine("0. Retour");
            Console.Write("\nVotre choix: ");

            string? choix = Console.ReadLine();
            Console.WriteLine();

            switch (choix)
            {
                case "1":
                    gestionAdoption.Lister();
                    break;
                case "2":
                    gestionAdoption.Ajouter();
                    break;
                case "3":
                    gestionAdoption.ModifierStatut();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Choix invalide!");
                    break;
            }
        }
    }

    private void MenuGestionAccueil()
    {
        while (true)
        {
            Console.WriteLine("\n=== GESTION DES FAMILLES D'ACCUEIL ===");
            Console.WriteLine("1. Lister les familles d'accueil par où l'animal est passé");
            Console.WriteLine("2. Lister les animaux accueillis par une famille d'accueil");
            Console.WriteLine("3. Ajouter une nouvelle famille d'accueil à un animal");
            Console.WriteLine("0. Retour");
            Console.Write("\nVotre choix: ");

            string? choix = Console.ReadLine();
            Console.WriteLine();

            switch (choix)
            {
                case "1":
                    gestionAccueil.ListerParAnimal();
                    break;
                case "2":
                    gestionAccueil.ListerParFamille();
                    break;
                case "3":
                    gestionAccueil.Ajouter();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Choix invalide!");
                    break;
            }
        }
    }

    private void MenuGestionVaccination()
    {
        while (true)
        {
            Console.WriteLine("\n=== GESTION DES VACCINATIONS ===");
            Console.WriteLine("1. Ajouter un vaccin à un animal");
            Console.WriteLine("2. Consulter les vaccinations d'un animal");
            Console.WriteLine("0. Retour");
            Console.Write("\nVotre choix: ");

            string? choix = Console.ReadLine();
            Console.WriteLine();

            switch (choix)
            {
                case "1":
                    gestionVaccination.Ajouter();
                    break;
                case "2":
                    gestionVaccination.Consulter();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Choix invalide!");
                    break;
            }
        }
    }

    private void MenuGestionEntreeSortie()
    {
        while (true)
        {
            Console.WriteLine("\n=== GESTION DES ENTRÉES/SORTIES ===");
            Console.WriteLine("1. Ajouter une entrée au refuge");
            Console.WriteLine("2. Ajouter une sortie du refuge");
            Console.WriteLine("3. Lister les entrées d'un animal");
            Console.WriteLine("4. Lister les sorties d'un animal");
            Console.WriteLine("0. Retour");
            Console.Write("\nVotre choix: ");

            string? choix = Console.ReadLine();
            Console.WriteLine();

            switch (choix)
            {
                case "1":
                    gestionEntreeSortie.AjouterEntree();
                    break;
                case "2":
                    gestionEntreeSortie.AjouterSortie();
                    break;
                case "3":
                    gestionEntreeSortie.ListerEntrees();
                    break;
                case "4":
                    gestionEntreeSortie.ListerSorties();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Choix invalide!");
                    break;
            }
        }
    }

    private void ListerAnimauxAuRefuge()
    {
        try
        {
            Console.WriteLine("=== ANIMAUX PRÉSENTS AU REFUGE ===");
            var animaux = AnimalDAO.ListerAnimauxAuRefuge();
            
            if (animaux.Count == 0)
            {
                Console.WriteLine("Aucun animal présent au refuge.");
                return;
            }

            foreach (var animal in animaux)
            {
                Console.WriteLine($"- {animal.Nom} ({animal.Type}) - ID: {animal.Identifiant}");
            }
            
            Console.WriteLine($"\nTotal: {animaux.Count} animal(s)");
        }
        catch (NpgsqlException ex)
        {
            Console.WriteLine($"\nErreur de connexion PostgreSQL: {ex.Message}");
            Console.WriteLine($"Code SQL: {ex.SqlState}");
            Console.WriteLine($"Code erreur: {ex.HResult:X8}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Détails: {ex.InnerException.Message}");
            }
            Console.WriteLine("\nVérifiez que la base de données est accessible.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nErreur lors de la récupération des animaux: {ex.Message}");
            Console.WriteLine($"Type: {ex.GetType().Name}");
            Console.WriteLine($"Code erreur: 0x{ex.HResult:X8}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Détails: {ex.InnerException.Message}");
            }
        }
    }
}
