# Comment lancer l'application

## Prérequis

1. ✅ PostgreSQL est démarré
2. ✅ La base de données `refuge_animaux` existe
3. ✅ Les tables ont été créées (exécuter `creertables_JUNGLING_Pierre.sql`)
4. ✅ Le fichier `appsettings.json` est configuré avec vos identifiants

## Méthode 1 : Via VS Code (Recommandé)

### Option A : Avec le débogueur (F5)

1. Ouvrez VS Code dans le dossier du projet
2. Appuyez sur **F5** ou cliquez sur **Run > Start Debugging**
3. L'application s'exécutera dans un terminal intégré
4. Le menu principal s'affichera automatiquement

### Option B : Via la palette de commandes

1. Appuyez sur **Ctrl+Shift+P** (ou **Cmd+Shift+P** sur Mac)
2. Tapez "Debug: Start Debugging"
3. Sélectionnez ".NET Core Launch (console)"
4. L'application démarre

## Méthode 2 : Via le Terminal intégré de VS Code

1. Ouvrez un terminal dans VS Code : **Terminal > New Terminal** (ou **Ctrl+`**)
2. Naviguez vers le dossier du projet (si nécessaire) :
   ```bash
   cd "/Users/pierrejungling/Documents/Bac Informatique/Bac 3/Projet SGBD/Travail/Code/ConsoleAPI/JUNGLING_Pierre_SGBD"
   ```
3. Exécutez l'application :
   ```bash
   dotnet run
   ```

## Méthode 3 : Via un Terminal externe

1. Ouvrez Terminal.app (Mac) ou votre terminal préféré
2. Naviguez vers le dossier du projet :
   ```bash
   cd "/Users/pierrejungling/Documents/Bac Informatique/Bac 3/Projet SGBD/Travail/Code/ConsoleAPI/JUNGLING_Pierre_SGBD"
   ```
3. Exécutez l'application :
   ```bash
   dotnet run
   ```

## Méthode 4 : Compiler puis exécuter

1. Compilez le projet :
   ```bash
   dotnet build
   ```
2. Exécutez l'application compilée :
   ```bash
   dotnet bin/Debug/net7.0/RefugeAnimaux.dll
   ```

## Vérification avant le lancement

### 1. Vérifier que PostgreSQL est démarré

```bash
# Sur macOS avec Homebrew
brew services list | grep postgresql

# Si ce n'est pas démarré :
brew services start postgresql
```

### 2. Vérifier que la base de données existe

```bash
psql -U refuge_animaux_user -d refuge_animaux -c "\dt"
```

Cette commande liste toutes les tables. Vous devriez voir au moins la table `ROLE`.

### 3. Vérifier le fichier appsettings.json

Assurez-vous que le fichier `appsettings.json` contient vos identifiants corrects :

```json
{
  "Database": {
    "Host": "localhost",
    "Port": 5432,
    "Database": "refuge_animaux",
    "Username": "refuge_animaux_user",
    "Password": "p@ssword"
  }
}
```

## Résolution de problèmes

### Erreur : "relation ROLE does not exist"

**Solution** : Exécutez le script SQL pour créer les tables :

```bash
psql -U refuge_animaux_user -d refuge_animaux -f creertables_JUNGLING_Pierre.sql
```

### Erreur : "password authentication failed"

**Solution** : Vérifiez que le mot de passe dans `appsettings.json` correspond au mot de passe de l'utilisateur `refuge_animaux_user` dans PostgreSQL.

### Erreur : "error: 0x80070057" lors de la saisie

**Solution** : Utilisez un terminal intégré au lieu de la console de débogage. Le fichier `.vscode/launch.json` est déjà configuré pour cela. Relancez avec F5.

### L'application ne démarre pas

1. Vérifiez que .NET 7.0 SDK est installé :
   ```bash
   dotnet --version
   ```
   Devrait afficher `7.0.x` ou supérieur.

2. Restaurez les packages :
   ```bash
   dotnet restore
   ```

3. Nettoyez et reconstruisez :
   ```bash
   dotnet clean
   dotnet build
   ```

## Utilisation de l'application

Une fois l'application lancée :

1. L'application charge automatiquement la configuration depuis `appsettings.json`
2. Le menu principal s'affiche directement
3. Entrez le numéro de l'option souhaitée (1, 2, 3, etc.)
4. Suivez les instructions à l'écran

## Exemple de session

```
=== MENU PRINCIPAL ===
1. Gestion des animaux
2. Gestion des contacts
...
0. Quitter

Votre choix: 1

=== GESTION DES ANIMAUX ===
1. Ajouter un animal
...
```
