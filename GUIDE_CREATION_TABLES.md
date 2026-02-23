# Guide de création des tables - Refuge d'Animaux

## ⚠️ Problème détecté
Toutes les tables sont manquantes dans la base de données. Ce guide vous explique comment les créer.

## Méthode 1 : Via pgAdmin (Recommandé - Plus simple) ✨

### Étape 1 : Ouvrir pgAdmin
1. Lancez pgAdmin
2. Connectez-vous à votre serveur PostgreSQL

### Étape 2 : Se connecter à la base de données
1. Dans l'arborescence de gauche, développez :
   - **Servers** → Votre serveur → **Databases**
2. Cliquez avec le bouton droit sur la base de données **`refuge_animaux`**
3. Sélectionnez **Query Tool** (ou **Outil de requête**)

### Étape 3 : Exécuter le script SQL
1. Dans l'onglet **Query Tool** qui s'est ouvert :
   - Cliquez sur l'icône **📂 Open File** (Ouvrir un fichier)
   - Naviguez vers le dossier du projet
   - Sélectionnez le fichier **`creertables_JUNGLING_Pierre.sql`**
   - Cliquez sur **Ouvrir**

2. Vérifiez que le script est bien chargé dans l'éditeur

3. Cliquez sur le bouton **▶️ Execute/Refresh** (Exécuter/Rafraîchir)
   - Ou appuyez sur **F5**
   - Le script va créer toutes les tables

4. Vous devriez voir un message de succès en bas :
   - `Query returned successfully in X ms.`
   - `Command was successful.`

### Étape 4 : Vérifier que les tables sont créées
1. Dans l'arborescence de gauche, sous **`refuge_animaux`** :
   - Développez **Schemas** → **public** → **Tables**
2. Vous devriez voir toutes ces tables :
   - ✅ ADOPTION
   - ✅ ANIMAL
   - ✅ ANIMAL_COULEUR
   - ✅ ANI_COMPATIBILITE
   - ✅ ANI_ENTREE
   - ✅ ANI_SORTIE
   - ✅ COMPATIBILITE
   - ✅ CONTACT
   - ✅ COULEUR
   - ✅ FAMILLE_ACCUEIL
   - ✅ PERSONNE_ROLE
   - ✅ ROLE
   - ✅ VACCIN
   - ✅ VACCINATION

**OU** exécutez le script de vérification :
1. Dans le **Query Tool**, ouvrez le fichier **`verifier_tables.sql`**
2. Exécutez-le (**F5**)
3. Toutes les tables devraient maintenant afficher **"✓ Existe"**

---

## Méthode 2 : Via ligne de commande (Terminal)

### Prérequis
- PostgreSQL doit être installé
- `psql` doit être accessible depuis le terminal

### Étape 1 : Ouvrir un terminal
- Sur macOS/Linux : Ouvrez Terminal
- Sur Windows : Ouvrez PowerShell ou CMD

### Étape 2 : Naviguer vers le dossier du projet
```bash
cd "/Users/pierrejungling/Documents/Bac Informatique/Bac 3/Projet SGBD/Travail/Code/ConsoleAPI/JUNGLING_Pierre_SGBD"
```

### Étape 3 : Exécuter le script SQL
```bash
psql -U refuge_animaux_user -d refuge_animaux -f creertables_JUNGLING_Pierre.sql
```

**Note :** Vous devrez entrer le mot de passe : `p@ssword`

### Étape 4 : Vérifier que les tables sont créées
```bash
psql -U refuge_animaux_user -d refuge_animaux -f verifier_tables.sql
```

Toutes les tables devraient maintenant afficher **"✓ Existe"**.

---

## Problèmes courants et solutions

### ❌ Erreur : "permission denied" ou "access denied"
**Solution :** Assurez-vous d'être connecté en tant que `refuge_animaux_user` ou avec un utilisateur ayant les droits nécessaires.

### ❌ Erreur : "database does not exist"
**Solution :** La base de données n'existe pas. Exécutez d'abord :
```bash
psql -U postgres -f creer_database_user.sql
```

### ❌ Erreur : "relation already exists"
**Solution :** C'est normal si vous exécutez le script plusieurs fois. Le script commence par supprimer les tables existantes avant de les recréer. Cette erreur peut apparaître si la suppression a échoué, mais généralement ce n'est pas grave.

### ❌ Erreur de syntaxe SQL dans pgAdmin
**Solution :** Assurez-vous d'avoir ouvert le bon fichier (`creertables_JUNGLING_Pierre.sql`) et qu'il est complet.

---

## Après la création des tables

Une fois les tables créées avec succès :

1. ✅ Vérifiez que toutes les tables existent avec `verifier_tables.sql`
2. ✅ Relancez votre application console
3. ✅ L'erreur "relation 'role' does not exist" devrait disparaître

---

## Si vous avez besoin d'insérer des données de test

Après avoir créé les tables, vous pouvez insérer des données de test avec :
```bash
psql -U refuge_animaux_user -d refuge_animaux -f inserer_donnees_test.sql
```

**OU** dans pgAdmin :
1. Ouvrez le fichier `inserer_donnees_test.sql` dans le Query Tool
2. Exécutez-le (F5)