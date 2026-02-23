# Travail 2 – Partie 2 : Application fenêtrée + procédures stockées

## Réalisations

### 1. Procédures stockées (PL/pgSQL)

- **Script** : `procedures_stockees_refuge.sql`
- À exécuter **après** `creertables_JUNGLING_Pierre.sql` sur la base PostgreSQL.
- Toutes les opérations métier passent par des fonctions stockées (animal_inserer, contact_consulter, adoption_ajouter, etc.). La couche DAL n’exécute plus de SQL en dur, uniquement des appels à ces procédures (conformément au chapitre 11 et 12 des notes de cours).

**Exécution** (depuis le dossier du projet) :
```bash
psql -U refuge_animaux_user -d refuge_animaux -f procedures_stockees_refuge.sql
```

### 2. Couche d’accès aux données (DAL)

- Les fichiers dans `DAL/` appellent exclusivement les procédures stockées (ex. `SELECT animal_inserer(...)`, `SELECT * FROM animal_consulter(@id)`).
- Aucune requête SQL directe dans le C#.

### 3. Application fenêtrée WPF

- **Point d’entrée** : `App.xaml` / `App.xaml.cs` (chargement de `appsettings.json`, test de connexion, démarrage de la fenêtre principale).
- **Fenêtre principale** : `Presentation/MainWindow.xaml` – menu avec 7 actions + Quitter.
- **Fenêtres** (couche présentation = fenêtres, comme demandé) :
  - Gestion des animaux
  - Gestion des contacts
  - Gestion des adoptions
  - Gestion des familles d’accueil
  - Gestion des vaccinations
  - Gestion des entrées/sorties
  - Liste des animaux présents au refuge

Conception inspirée des notes de cours WPF (Chapitre 13) : XAML pour la vue, code-behind pour les actions, liaison aux données via `ItemsSource` et utilisation du `DataContext` là où c’est pertinent.

### 4. Lancement

- **Sur Windows** : `dotnet run` ou exécuter l’exe dans `bin/Debug/net7.0-windows/`.
- **Sur macOS/Linux** : la compilation est possible avec `EnableWindowsTargeting` ; l’exécution WPF nécessite Windows.

### 5. Ancienne application console

- Le fichier `Presentation/Program.cs` est **exclu** du build pour que l’entrée soit l’application WPF. Les classes `GestionAnimal`, `GestionContact`, etc. restent dans le projet (réutilisables si besoin pour des tests ou une version console séparée).
