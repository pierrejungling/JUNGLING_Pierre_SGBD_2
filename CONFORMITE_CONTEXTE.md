# Conformité avec le contexte du projet – Refuge Animaux

Ce document relie l’énoncé du travail à l’application réalisée.

## Contexte et modélisation

- **Animal** : identifiant `yymmdd99999` (yymmdd = date d’entrée au refuge, 5 chiffres = séquence). Nom, type (chat, chien), sexe, couleur(s), stérilisé, date stérilisation, date naissance, date décès, description, particularités. **→ Implémenté** (identifiant généré par `animal_prochain_identifiant`, couleurs en liste multi-sélection).
- **Compatibilités** : type (chat, chien, jeune enfant, enfant, jardin, poney), valeur (oui/non), description optionnelle. **→ Tables et procédures en place** ; gestion dans l’app selon les écrans existants.
- **Motifs d’entrée** : abandon, errant, décès propriétaire, saisie, retour adoption (+ retour famille d’accueil). **→ Liste dans la fenêtre « Ajouter un animal » (ComboBox).**
- **Motifs de sortie** : adoption, retour propriétaire, décès de l’animal, famille d’accueil. **→ Gérés en base et dans la gestion entrées/sorties.**
- **Adoption** : statuts (Demande, acceptée, rejet environnement, rejet comportement), personne de contact. **→ Gestion des adoptions dans l’app.**
- **Famille d’accueil** : personne de contact, date début, date fin. **→ Gestion des familles d’accueil.**
- **Vaccinations** : nom du vaccin, date, fait ou non. **→ Gestion des vaccinations.**
- **Contacts** : identifiant, nom, prénom, registre national, adresse, GSM, téléphone, email ; rôles (bénévole, adoptant, candidat, Famille d’accueil). **→ Gestion des contacts.**

## Fonctionnalités demandées ↔ Menu / écrans

| Fonctionnalité (énoncé) | Où dans l’app |
|-------------------------|----------------|
| Ajouter un animal | Menu « Gestion des animaux » → bouton **Ajouter** (fenêtre avec identifiant yymmdd99999, motif d’entrée, couleurs, etc.) |
| Consulter un animal | Gestion des animaux → **Consulter** |
| Supprimer un animal | Gestion des animaux → **Supprimer** |
| Ajouter une information sur un animal (compatibilité, description, particularité) | Via les écrans de détail / gestion animal |
| Supprimer une information sur un animal | Idem |
| Ajouter une personne de contact | Gestion des contacts |
| Consulter une personne de contact | Gestion des contacts |
| Lister les animaux présents au refuge (toutes les entrées) | Menu **« 7. Lister les animaux présents au refuge »** |
| Lister les familles d’accueil par animal | Gestion des familles d’accueil |
| Lister les animaux accueillis par une famille d’accueil | Gestion des familles d’accueil |
| Ajouter une nouvelle famille d’accueil à un animal (date arrivée + contact obligatoires) | Gestion des familles d’accueil |
| Lister les adoptions et leur statut | Gestion des adoptions |
| Ajouter une adoption / Modifier le statut d’une adoption | Gestion des adoptions |
| Ajouter un vaccin (date, nom, fait ou non) à un animal | Gestion des vaccinations |
| Modifier les coordonnées d’une personne de contact | Gestion des contacts |
| Supprimer une personne de contact | Gestion des contacts |

## Identifiant animal (yymmdd99999)

- **Règle** : `yymmdd` = date d’entrée au refuge ; les 5 derniers chiffres forment une séquence pour cette date.
- **Dans l’app** :  
  - Fenêtre « Ajouter un animal » : bouton **Générer** remplit l’identifiant à partir de la date d’entrée (procédure stockée `animal_prochain_identifiant`).  
  - La saisie manuelle est possible ; elle est contrôlée (11 chiffres, 6 premiers = date d’entrée).

## Procédures stockées (Travail 2 – partie 2)

La couche d’accès aux données appelle des procédures stockées (fichier `procedures_stockees_refuge.sql`), notamment :

- `animal_prochain_identifiant(date)` – génération de l’identifiant
- `animal_inserer`, `animal_consulter`, `animal_supprimer`, `animal_lister_*`, `animal_ajouter_couleur`, etc.
- `entree_ajouter`, `entree_lister_par_animal`
- `couleur_lister_toutes`
- Et les autres procédures pour contacts, adoptions, familles d’accueil, vaccinations, sorties.

Après modification du script SQL, réexécuter `procedures_stockees_refuge.sql` sur la base `refuge_animaux`.
