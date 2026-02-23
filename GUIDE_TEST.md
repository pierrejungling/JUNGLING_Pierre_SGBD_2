# Guide de test - Application Console Refuge d'Animaux

## Prérequis

1. **PostgreSQL installé et en cours d'exécution**
2. **Base de données et utilisateur créés** : Exécuter `creer_database_user.sql` (en tant que superutilisateur)
3. **Tables créées** : Exécuter `creertables_JUNGLING_Pierre.sql` (en tant que `refuge_animaux_user`)
4. **.NET 7.0 SDK installé**

## Installation de la base de données

### Étape 1 : Créer la base de données et l'utilisateur

Exécutez le script SQL pour créer la base de données et l'utilisateur :

```bash
psql -U postgres -f creer_database_user.sql
```

Ce script crée :
- L'utilisateur `refuge_animaux_user` avec le mot de passe `p@ssword`
- La base de données `refuge_animaux` avec cet utilisateur comme propriétaire
- Tous les privilèges nécessaires pour l'utilisateur

### Étape 2 : Créer les tables

Exécutez le script SQL pour créer toutes les tables :

```bash
psql -U refuge_animaux_user -d refuge_animaux -f creertables_JUNGLING_Pierre.sql
```

## Compilation et exécution

### Méthode 1 : Via Visual Studio / Rider / VS Code

1. Ouvrir le projet dans votre IDE
2. Restaurer les packages NuGet (automatique ou `dotnet restore`)
3. Compiler le projet (F5 ou `dotnet build`)
4. Exécuter (F5 ou `dotnet run`)

### Méthode 2 : Via ligne de commande

```bash
# Naviguer vers le dossier du projet
cd "/Users/pierrejungling/Documents/Bac Informatique/Bac 3/Projet SGBD/Travail/Code/ConsoleAPI/JUNGLING_Pierre_SGBD"

# Restaurer les packages
dotnet restore

# Compiler
dotnet build

# Exécuter
dotnet run
```

## Configuration de la connexion

L'application utilise le fichier `appsettings.json` pour la configuration de la base de données.

### Configuration initiale

1. **Copier le fichier exemple** :
   ```bash
   cp appsettings.example.json appsettings.json
   ```

2. **Éditer `appsettings.json`** avec vos paramètres de connexion :
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
   
   **Note** : Si vous avez modifié le mot de passe lors de la création de l'utilisateur, utilisez votre mot de passe personnalisé.

3. **Important** : Le fichier `appsettings.json` contient des informations sensibles. Ne le commitez pas dans Git.

L'application chargera automatiquement ces paramètres au démarrage et affichera directement le menu principal.

## Scénarios de test

### Test 1 : Créer les rôles de base

Avant de créer des contacts, insérez les rôles dans la base de données :

```sql
INSERT INTO ROLE (rol_nom) VALUES ('benevole');
INSERT INTO ROLE (rol_nom) VALUES ('adoptant');
INSERT INTO ROLE (rol_nom) VALUES ('candidat');
INSERT INTO ROLE (rol_nom) VALUES ('Famille_accueil');
```

### Test 2 : Ajouter un contact

1. Menu principal → `2` (Gestion des contacts)
2. `1` (Ajouter un contact)
3. Remplir les informations :
   - Nom : `Dupont`
   - Prénom : `Jean`
   - Registre national : `85.03.15-123.45` (format: yy.mm.dd-999.99)
   - Rue : `Rue de la Paix, 10`
   - Code postal : `4000`
   - Localité : `Liège`
   - GSM : `0478123456` (au moins un contact requis)
   - Email : `jean.dupont@email.com`
   - Rôles : `1,2` (IDs des rôles séparés par des virgules)

### Test 3 : Ajouter un animal

1. Menu principal → `1` (Gestion des animaux)
2. `1` (Ajouter un animal)
3. Remplir les informations :
   - Date d'entrée : `2026-01-15`
   - Séquence : `00001` (5 chiffres)
   - Nom : `Médor`
   - Type : `chien`
   - Sexe : `M`
   - Date de naissance : `2020-05-10`
   - Stérilisé : `true`
   - Date de stérilisation : `2021-06-15`
   - Description : `Chien très gentil`
   - Particularités : `Aucune`
   - Couleurs : `noir,blanc` (séparées par des virgules)
4. L'application créera automatiquement l'entrée au refuge
   - Raison : `1` (abandon)
   - Contact : ID du contact créé précédemment (ou laissez vide)

### Test 4 : Consulter un animal

1. Menu principal → `1` (Gestion des animaux)
2. `2` (Consulter un animal)
3. Entrer l'identifiant : `26011500001` (format: yymmdd + séquence)

### Test 5 : Ajouter une compatibilité

1. Menu principal → `1` (Gestion des animaux)
2. `4` (Ajouter une information)
3. `1` (Ajouter une compatibilité)
4. Entrer l'identifiant de l'animal
5. Type : `chien`
6. Valeur : `true`
7. Description : `S'entend bien avec les autres chiens`

### Test 6 : Ajouter une entrée

1. Menu principal → `6` (Gestion des entrées/sorties)
2. `1` (Ajouter une entrée au refuge)
3. Remplir les informations

### Test 7 : Ajouter une sortie

1. Menu principal → `6` (Gestion des entrées/sorties)
2. `2` (Ajouter une sortie du refuge)
3. Remplir les informations

### Test 8 : Ajouter une adoption

1. Menu principal → `3` (Gestion des adoptions)
2. `2` (Ajouter une adoption)
3. Remplir les informations :
   - Identifiant animal
   - Identifiant contact (adoptant)
   - Date de demande : `2026-01-20`
   - Statut : `1` (demande)

### Test 9 : Modifier le statut d'une adoption

1. Menu principal → `3` (Gestion des adoptions)
2. `3` (Modifier le statut d'une adoption)
3. Entrer les informations de l'adoption
4. Choisir le nouveau statut : `2` (acceptee)

### Test 10 : Ajouter une famille d'accueil

1. Menu principal → `4` (Gestion des familles d'accueil)
2. `3` (Ajouter une nouvelle famille d'accueil)
3. Remplir les informations :
   - Identifiant animal
   - Identifiant contact (famille d'accueil)
   - Date de début : `2026-01-25`
   - Date de fin : (laissez vide si en cours)

### Test 11 : Ajouter une vaccination

1. Menu principal → `5` (Gestion des vaccinations)
2. `1` (Ajouter un vaccin à un animal)
3. Remplir les informations :
   - Identifiant animal
   - Nom du vaccin : `Vaccin antirabique`
   - Date de vaccination : `2026-01-20`

### Test 12 : Lister les animaux au refuge

1. Menu principal → `7` (Lister les animaux présents au refuge)
2. Vérifier que les animaux sans sortie ou avec entrée récente sont listés

## Tests de validation des contraintes

### Test des contraintes d'intégrité

1. **Test format identifiant** : Essayer d'ajouter un animal avec un ID invalide (moins de 11 chiffres)
2. **Test date de naissance** : Essayer une date future → doit être rejetée
3. **Test stérilisation** : Ajouter un animal stérilisé sans date → doit être rejeté
4. **Test registre national** : Format invalide → doit être rejeté
5. **Test contact obligatoire** : Essayer d'ajouter un contact sans GSM, téléphone ni email → doit être rejeté
6. **Test entrée multiple** : Essayer d'ajouter deux entrées sans sortie → doit être rejeté
7. **Test adoption 1-1** : Essayer d'ajouter deux adoptions pour le même animal → doit être rejeté

## Vérification dans la base de données

Vous pouvez vérifier les données directement dans pgAdmin :

```sql
-- Voir tous les animaux
SELECT * FROM ANIMAL;

-- Voir tous les contacts
SELECT * FROM CONTACT;

-- Voir les entrées
SELECT * FROM ANI_ENTREE ORDER BY date_entree DESC;

-- Voir les sorties
SELECT * FROM ANI_SORTIE ORDER BY date_sortie DESC;

-- Voir les adoptions
SELECT * FROM ADOPTION;

-- Voir les familles d'accueil
SELECT * FROM FAMILLE_ACCUEIL;

-- Voir les vaccinations
SELECT * FROM VACCINATION;
```

## Dépannage

### Erreur de connexion
- Vérifiez que PostgreSQL est démarré
- Vérifiez le nom d'utilisateur et le mot de passe
- Vérifiez que la base de données existe

### Erreur "relation does not exist"
- Exécutez le script `creertables_JUNGLING_Pierre.sql` dans la base de données

### Erreur de contrainte
- Vérifiez que les données respectent les contraintes (format, valeurs autorisées, etc.)

### Erreur de compilation
- Vérifiez que .NET 8.0 SDK est installé : `dotnet --version`
- Restaurez les packages : `dotnet restore`
