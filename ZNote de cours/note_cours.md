IPEFA Sup Seraing - Verviers
Projet de développement
SGDB
Notes de cours
Georgette Collard
2025-2026
Notes de cours – Projet de développement SGDB G. Collard 1
Table des matières
Chapitre 1 : Introduction ......................................................................................................................... 4
Chapitre 2 : Le modèle entité-relation (ou entité – association) ............................................................ 4
Les entités ............................................................................................................................................ 4
Les ensembles d’entités (entity sets) .................................................................................................. 4
Rôle ...................................................................................................................................................... 5
Contraintes de cardinalité ................................................................................................................... 5
Diagrammes entité-relation (entity-relationship diagrams) ............................................................... 5
Les attributs ......................................................................................................................................... 7
Clé ........................................................................................................................................................ 8
Ensembles d’entités faibles ................................................................................................................. 8
Contraintes d’intégrité ...................................................................................................................... 10
Une extension du modèle entité-relation : Relation IS-A ................................................................. 10
Héritage ......................................................................................................................................... 12
Exercices ............................................................................................................................................ 13
Exercice résolu ............................................................................................................................... 13
Processus d'élaboration d'un modèle entité-association ................................................................. 14
Exercices ............................................................................................................................................ 17
Chapitre 3 : Le modèle relationnel ........................................................................................................ 18
Passage du modèle entité-association au modèle relationnel ......................................................... 19
Comment convertir un modèle entité-association en un modèle relationnel ? ........................... 20
Redondance et normalisation ........................................................................................................... 23
Principes de la normalisation ........................................................................................................ 24
Exercices ........................................................................................................................................ 27
Chapitre 4 – PostgreSQL ........................................................................................................................ 28
Installation de PostgreSQL ................................................................................................................ 29
Création d’une base de donnée ........................................................................................................ 34
Type de données ............................................................................................................................... 38
Type numérique ............................................................................................................................ 38
Type de caractères ........................................................................................................................ 38
Type Date – Heure ......................................................................................................................... 38
Type boolean ................................................................................................................................. 38
Type géométrique ......................................................................................................................... 39
Type d’adresse réseau ................................................................................................................... 39
Notes de cours – Projet de développement SGDB G. Collard 2
Type tableau .................................................................................................................................. 39
Type composite ............................................................................................................................. 40
Comptabilité .................................................................................................................................. 40
Créer une table (CREATE TABLE) ....................................................................................................... 41
Modifier une table (ALTER TABLE) .................................................................................................... 41
Ajouter une colonne ...................................................................................................................... 41
Supprimer une colonne ................................................................................................................. 41
Ajouter une contrainte .................................................................................................................. 41
Supprimer une contrainte ............................................................................................................. 41
Modifier des valeurs par défaut .................................................................................................... 42
Modifier les types de valeur d’une colonne .................................................................................. 42
Renommer des colonnes ............................................................................................................... 42
Renommer une table ......................................................................................................................... 42
Supprimer une table .......................................................................................................................... 42
Créer les tables de la base de données Bibliothèques ...................................................................... 43
Via pgadmin ....................................................................................................................................... 43
Via le terminal de commande de pgAdmin ....................................................................................... 44
Insertion d’enregistrements dans une table. (INSERT) ..................................................................... 45
Modifier des enregistrements (UPDATE) .......................................................................................... 46
Supprimer un(des) enregistrement(s) (DELETE) ................................................................................ 46
Application : Bibliotheque ................................................................................................................. 47
Consulter les données (SELECT) ........................................................................................................ 48
Transactions ...................................................................................................................................... 52
Exemple : ....................................................................................................................................... 53
Héritage ............................................................................................................................................. 54
Notes de cours – Projet de développement SGDB G. Collard 3
Chapitre 1 : Introduction
Depuis la conception jusqu’à l’usage d’une base de données, plusieurs étapes doivent être
considérées :
1) 2) 3) 4) La création du modèle entité-association : représentation graphique des concepts du monde
réel qu’on veut mémoriser
La création du modèle relationnel : le modèle entité-association est converti en un modèle
relationnel, un modèle constitué d’un ensemble de tables.
La création de la base de données : à partir du modèle relationnel, la base de données et les
tables sont créées dans un système de gestion de bases de données (SGDB).
L’utilisation de la base de données : une application va transmettre des requêtes SQL au SGDB
en vue d’ajouter, de consulter, de modifier et de supprimer des lignes dans les tables des bases
de données.
Chapitre 2 : Le modèle entité-relation (ou entité – association)
Le modèle entité-relation (entity-relationship) est un cadre de définition général du type de contenu
potentiel d’une base de données. Il utilise deux types génériques de base :
Les ensembles d’entités : ensemble d’objets de même structure (enregistre les mêmes informations)
Les relations (ou associations) entre ces ensembles.
Les entités
Une entité (entity) est un objet (abstrait ou concret) au sujet duquel on conserve de l’information dans
la base de données. Il faut qu’une entité soit individualisable, en d’autres mots qu’il puisse être
distingué d’une autre entité.
Exemples :
- Entité concrètes : une personne, une voiture
- Entité abstraites : un trajet, un horaire
Note : Du point de vue d’une base de données, une entité n’a de sens que si l’on peut identifier les
informations qui seront conservé à son sujet.
Les ensembles d’entités (entity sets)
Les entités sont regroupées en ensembles d’entités semblables (de même type), c’est-à-dire au sujet
desquelles on veut conserver la même information.
Exemples :
- Les acteurs d’un film
- Les employés d’une firme
- Les cours de la promotion sociale de Seraing
Notes de cours – Projet de développement SGDB G. Collard 4
Rôle
La participation d’un ensemble d’entités à une relation est son rôle. On donne un nom au rôle d’un
ensemble d’entités dans une relation.
Exemples :
- Dans la relation ENSEIGNE : EMPLOYE_PROV_LIEGE, COURS_PROSOC
o Le rôle de l’ensemble d’entités EMPLOYE_PROV_LIEGE est « enseignant »
o Le rôle de l’ensemble d’entités COURS_PROSOC est « cours enseigné »
- Dans la relation PATERNITE : PERSONNE, PERSONNE
o L’ensemble d’entités PERSONNE a deux rôles distincts : « père » et « enfant »
Contraintes de cardinalité
Pour chaque ensemble d’entités participant à une relation, c’est-à-dire pour chaque rôle d’un
ensemble d’entités, on précise dans combien de tuples de la relation chaque entité peut apparaître.
En fait, on ne donne pas un nombre mais on donne un intervalle : (min, max) où
- min est en général 0 ou 1
- max est en général 1 ou N (*)
Exemple :
Quelle est la participation de l’ensemble d’entités EMPLOYE_PROV_LIEGE dans la relation
ENSEIGNE ?
En d’autres termes, quelles est la cardinalité du rôle « enseignant » dans la relation ENSEIGNE?
Ou encore, combien de fois un employé particulier de la province de Liège peut-il apparaître
dans une quelconque extension de la relation ENSEIGNE
- min : 0
- max : N
Diagrammes entité-relation (entity-relationship diagrams)
Un diagramme entité-relation est une description graphique des ensembles d’entités des relations qui
seront représentés dans une base de données.
Ces diagrammes représentent donc le schéma des données qui seront représentées, par opposition à
leur extension qui sera le contenu de la base de données.
Notation :
- Ensemble d’entité : nom dans un rectangle
- Relation : nom dans un losange ou un hexagone avec traits vers les ensembles d’entités
participant à la relation (chaque trait est étiqueté par le nom du rôle et la contrainte de
cardinalité correspondants)
Notes de cours – Projet de développement SGDB G. Collard 5
Exemples
On veut modéliser le fait que les clients peuvent passer des commandes à une centrale d’achat qui a
des fournisseurs habituels pour les différents produits possibles et qui décide par quel fournisseur un
produit commandé doit être livré.
Notes de cours – Projet de développement SGDB G. Collard 6
Les attributs
Les informations conservées au sujet des entités d’un ensemble sont leurs attributs.
Exemple :
- Le nom, l’adresse, la date de naissance d’une personne
- L’intitulé, la charge horaire d’un cours
Toutes les entités d’un ensemble ont exactement les mêmes attributs.
Chaque attribut d’un ensemble d’entités
- a un nom unique dans le contexte de cet ensemble d’entités,
- prend ses valeurs dans un domaine spécifié (type de l’attribut).
Différentes catégorie d’attributs
Un attribut peut être
- Simple (atomique) ou composite (décomposable en plusieurs attributs plus simples)
Exemple : l’attribut ADRESSE peut être constitué des attributs plus simples :
RUE_ET_NUMERO, BOITE, VILLE, CODE_POSTAL, PAYS.
- Obligatoire (une entité doit avoir une valeur pour cet attribut) ou facultatif
- À valeur unique ou à plusieurs valeurs
Exemple : l’attribut DIPLOMES (d’une personne) peut avoir plusieurs valeurs pour une même
personne.
- Enregistré ou dérivé (d’un autre attribut)
Exemple : l’attribut AGE (d’une personne) peut être dérivé de l’attribut enregistré
DATE_DE_NAISSANCE.
Notes de cours – Projet de développement SGDB G. Collard 7
Clé
Une clé d’un ensemble d’entités est un ensemble minimum d’attributs qui identifient de façon unique
une entité parmi cet ensemble.
Donc, deux entités distinctes d’un ensemble ne peuvent jamais avoir les mêmes valeurs pour les
attributs de la clé.
Exemples :
- Le numéro de matricule d’un enseignant
- La marque et le numéro de série d’une voiture
Notation : les attributs de la clé sont soulignés.
Ensembles d’entités faibles
Il peut arriver qu’un ensemble d’entité ne dispose pas d’attributs constituant une clé : c’est un
ensemble d’entités faibles
Les entités d’un ensemble faible se distinguent les unes des autres par
- (éventuellement) certains de ses attributs,
et
- Le fait qu’elles sont en relation avec d’autres ensembles d’entités.
On doit donc généraliser la notion de clé : la clé d’un ensemble d’entités est constituée
- D’attributs de l’ensemble d’entités
et/ou
- De rôles joués par d’autres ensembles d’entités dans leur relation avec cet ensemble d’entités
(relations identifiantes).
Notes de cours – Projet de développement SGDB G. Collard 8
Notes de cours – Projet de développement SGDB G. Collard 9
Contraintes d’intégrité
Les contraintes d’intégrité sont des contraintes que doivent satisfaire les données d’une base de
données à tout instant. Elles sont exprimées au niveau du schéma.
- Les contraintes de cardinalité (min, max)
- Les clés
- Contraintes sur les attributs
- Contraintes référentielles (on ne fait référence qu’à une entité existante)
- etc
Une extension du modèle entité-relation : Relation IS-A
Il est parfois intéressant de définir des ensembles d’entités de façon hiérarchique, la hiérarchie
correspondant à une structure d’inclusion.
Ensemble d’entité inclus : sous-classe (ou ensemble d’entités spécifique)
Ensemble d’entités en comprenant d’autres : super-classe (ou ensemble d’entités génériques)
Exemple : spécialisation : division d’un ensemble d’entités en sous-classes
Un contribuable peut être soit indépendante, soit salariée
Notes de cours – Projet de développement SGDB G. Collard 10
Exemple : généralisation : regroupement de plusieurs ensembles d’entités en une super-classe.
Une classe peut être divisée selon plusieurs critères indépendants, donnant lieu à plusieurs
spécialisations différentes.
Exemple : L’ensemble d’entité EMPLOYE peut donner lieu à deux taxonomies basées sur des critères
distincts :
- La fonction de l’employé donne lieu à une spécialisation en 3 sous-classes : SECRETAIRE,
TECHNICIEN, CADRE
- Le type de numération donne lieu à une autre spécialisation, complètement indépendante de
la première, en 2 sous-classes : SALARIE, CONSULTANT
Une classe peut être la sous-classe de plusieurs superclasses, participant ainsi à plusieurs
généralisations.
Contraintes sur les spécialisations/généralisations
Deux catégories de contraintes indépendantes :
- Quand l’union des sous-classes donne la super-classe, les sous-classes constituent une
couverture de la super-classe. Dans ce cas, la cardinalité minimum du rôle de la super-classe
dans la relation IS-A est 1 s’il y a couverture, et est 0 sinon.
- Les sous-classes d’une super-classe peuvent être disjointes ou non. Dans ce cas, la cardinalité
maximum du rôle de la super-classe dans la relation IS-A est 1 si les sous-classes sont disjointes,
et N sinon.
Notes de cours – Projet de développement SGDB G. Collard 11
Héritage
A l’inclusion entre ensembles d’entités correspond l’héritage des propriétés (attributs et rôles).
Lorsqu’un ensemble d’entités est une sous-classe de deux classes différentes, on parle d’héritage
multiple.
Notes de cours – Projet de développement SGDB G. Collard 12
Exercices
Exercice résolu
Soit une base de données décrivant des films avec leur réalisateur et leurs acteurs. Un film est réalisé
que par un seul réalisateur. Un acteur ne joue qu’un rôle dans un film donné. Les réalisateurs et les
acteurs sont des artistes. Chaque artiste est caractérisé par un numéro, un nom, un prénom et une
date de naissance. Un film est caractérisé par un titre, une année de sortie, un genre. Le réalisateur
peut être acteur dans ses films réalisés.
Notes de cours – Projet de développement SGDB G. Collard 13
Processus d'élaboration d'un modèle entité-association
1. On décompose l'énonce en propositions élémentaires (propositions binaires). Une
proposition binaire affirme l'existence de 3 types de faits: 2 concepts et 1 lien entre eux.
2. Les types de faits contenus dans chaque proposition binaire sont ajoutés au modèle entité-
association s'ils y sont absents et s'ils n'entrent pas en contradiction avec les informations déjà
présentes dans le modèle.
Chaque concept d'une proposition binaire est représenté dans le modèle entité-association
soit par un ensemble d'entités, soit par un attribut d'un ensemble d'entités.
• Quand un concept est une propriété de l'autre concept, alors ce concept-là devient un
attribut de l'autre concept qui lui devient un ensemble d'entités.
Exemple. Pour la proposition Tout client a un nom, les concepts sont client et nom.
Le lien est a. nom devient un attribut de l'ensemble d'entités Client:
• Quand les 2 concepts sont indépendants, c'est-à-dire quand aucun des 2 concepts n'est
une propriété de l'autre, alors ils deviennent chacun un ensemble d'entités.
Exemple. Pour la proposition Une commande est passée par un client, les concepts sont
commande et client. Le lien est est passée par. Les concepts commande et client
deviennent 2 ensembles d'entités et le lien est passée par devient une association:
Remarque: il importe de considérer les deux types suivants de propositions non binaires:
• Une proposition est non binaire réductible quand elle contient plus de 2 concepts et peut
être décomposée directement en plusieurs propositions binaires.
Exemple. Un trajet organise entre une ville de départ et une ville d'arrivée peut se réduire
en:
- un trajet a une ville de départ.
- un trajet a une ville d'arrivée.
Exemple. Les clients ont un nom et une adresse peut se réduire en:
- les clients ont un nom.
- les clients ont une adresse.
• Une proposition est non binaire non réductible quand elle contient plus de 2 concepts et
ne peut être décomposée directement en plusieurs propositions binaires.
Notes de cours – Projet de développement SGDB G. Collard 14
Exemple. Les clients achètent des produits chez des fournisseurs. On ne peut, sans perdre
de l'information, réduire cette proposition en les 3 propositions binaires suivantes:
- Les clients achètent des produits.
- Les fournisseurs vendent des produits.
- Les clients achètent chez des fournisseurs.
Pour illustrer ce problème, supposons les clients Jean et Paul, les produits P1 et P2, et les
fournisseurs F1 et F2 ainsi que la proposition initiale Les clients achètent des produits chez
des fournisseurs. Supposons aussi les 3 faits suivants :
- Jean achète P1 chez F1.
- Jean achète P2 chez F2.
- Paul achète P1 chez F2.
En réduisant ces 3 faits non binaires en des faits binaires, on obtient les 9 faits binaires
suivants :
- Jean achète P1.
- Jean achète P2.
- Paul achète P1.
- F1 vend P1.
- F2 vend P1.
- F2 vend P2.
- Jean achète chez F1.
- Jean achète chez F2.
- Paul achète chez F2.
Ces 9 faits binaires ne sont pas équivalents aux 3 faits non binaires donnés plus haut, car,
à partir de ces faits binaires, on peut générer, par exemple, le fait Jean achète P1 chez F2
lequel dans la réalité est faux !!!
La solution à ce problème est d'ajouter le concept central achat dans la proposition
initiale Les clients achètent des produits chez des fournisseurs. Celle-ci devient alors les
clients font des achats de produits chez des fournisseurs. Cette proposition non binaire
peut maintenant être réduite en les 3 propositions binaires suivantes:
- Un achat est effectué par un client.
- Un achat concerne un produit.
- Un achat s'effectue chez un fournisseur.
Les 3 faits non binaires originaux peuvent alors être exprimes ainsi:
- l'achat X est effectué par Jean.
- l'achat Y est effectué par Jean.
- l'achat Z est effectué par Paul.
- l'achat X concerne P1.
- l'achat Y concerne P2.
- l'achat Z concerne P1.
- l'achat X s'effectue chez F1.
- l'achat Y s'effectue chez F2.
- l'achat Z s'effectue chez F2.
Notes de cours – Projet de développement SGDB G. Collard 15
Grâce au concept d'achat, on retrouve les 3 faits originaux:
- l'achat X concerne Jean, P1 et F1.
- l'achat Y concerne Jean, P2 et F2.
- l'achat Z concerne Paul, P1 et F2.
-
Modèle entité-association avant l'ajout du concept Achat :
Modèle entité-association après l'ajout du concept Achat :
Notes de cours – Projet de développement SGDB G. Collard 16
Exercices
Exercice 1
Produire un modèle entité-association qui décrit les informations concernant les produits d’un
supermarché. Chaque produit a un nom et un prix et appartient à une catégorie. Le supermarché a
plusieurs rayons, un rayon étant caractérisé par un étage et un numéro de rangée. On veut maintenir
l’emplacement des produits dans les rayons. Les produits d’une même catégorie sont placés dans le
même rayon, mais un rayon peut contenir des produits de plusieurs catégories.
Exercice 2
Un tournoi de tennis est constitué de joueurs, de terrains et de rencontres (matchs de simple). Le
tournoi comprend plusieurs rencontres. Un joueur participe à une rencontre, laquelle se joue sur un
terrain. Après chaque rencontre, seul un joueur est déclaré vainqueur. Le joueur est décrit par un
matricule unique, un nom et un classement. La rencontre est décrite par un numéro unique, une date
et une heure précise. Un terrain est décrit par un numéro unique et une surface (terre battue
extérieur, terre battue intérieur, french court).
Exercice 3
La sécurité sociale veut développer un système informatique permettant de suivre les prescriptions
médicales effectuées par les médecins. Chaque médecin consulte un patient à la fois. Au terme de la
consultation, le médecin peut prescrire des médicaments aux patients. Un médecin est caractérisé par
un matricule, un nom, un prénom, une adresse et sa spécialité. Un patient est caractérisé par un
numéro de sécurité sociale, un nom, un prénom, une adresse et sa mutuelle. Le médicament est décrit
par un code, par un nom, par une description.
Exercice 4
Un journaliste est identifié par son matricule, par un nom et par une date de naissance. Un article est
décrit par un numéro et un contenu. Un article est rédigé que par un seul journaliste et traite que d’un
seul sujet. Un sujet donné peut être traité par plusieurs articles. Un article n’apparaît que dans une
seule édition de journal. L’édition d’un journal se caractérise par un numéro et une date de parution.
Un journaliste peut travailler pur plusieurs journaux. Un journal est caractérisé par un titre et par une
adresse.
Exercice 5
Un livre appartient à une seule catégorie. Un livre est identifié par son ISBN et est caractérisé par un
titre, auteur et la maison d’édition. Une catégorie est identifiée par un numéro unique et est
caractérisé par un nom obligatoire et éventuellement une sous-catégorie. Il peut exister plusieurs
exemplaires pour un livre donné. Chaque exemplaire est identifié par un numéro unique.
Une personne (emprunteur) est identifiée par un identifiant unique, un nom, prénom, adresse, GSM.
Une personne peut emprunter à un moment donné au maximum 10 livres, et peut éventuellement
prolonger un exemplaire emprunté mais qu’une seule fois. Un livre emprunté peut être prolongé
endéans les 28 jours de l’emprunt. Un livre emprunté ou prolongé doit être restitué endéans les 28
jours après la date de sortie ou la date de prolongation.
Notes de cours – Projet de développement SGDB G. Collard 17
Chapitre 3 : Le modèle relationnel
Une base de données permet de centraliser, stocker, rechercher, supprimer, consulter tous type
d’informations. C’est un ensemble organisé d’informations traitant d’un même sujet.
Les principaux avantages d’une base de données sont
- Non redondance d’informations : afin d’éviter les problèmes lors des mises à jour, chaque
donnée ne doit être présente qu’une seule fois dans la base
- Cohérence des données : Les données sont soumises à un certain nombre de contraintes
d’intégrité qui définissent un état cohérent de la base. Elles doivent pouvoir être exprimées
simplement et vérifiées automatiquement à chaque insertion, modification ou suppression
des données. La cohérence implique qu’il n’y a pas de redondance de données (un acteur ne
peut pas jouer un rôle dans un film qui n’est pas connu du système ou l’âge d’une personne
doit être positif).
Un système de gestion de base de données (SGBD) est un logiciel qui permet de manipuler les
informations stockées dans une base de données.
Le modèle relationnel (SGBDR, Système de gestion de bases de données relationnelles) : les
données sont enregistrées dans des tableaux à deux dimensions (lignes et colonnes).
Une ligne représente un enregistrement. Les colonnes représentent les champs. Un
enregistrement correspond à une entrée complète comparable à une fiche. Chaque
enregistrement contient des champs, chacun des champs comprend une partie d’information
de l’enregistrement comme par exemple le nom, le prénom, l’adresse, … Une colonne (un
champ, un attribut) prends sa valeur parmi un domaine de valeurs. Celui-ci représente des
valeurs possibles pour la colonne. Un domaine de valeurs correspond à un type de données.
La clé primaire d’une table est un ensemble minimum de colonnes qui identifie de façon unique un
enregistrement (une ligne de la table). On ne peut enlever aucun attribut à la clé primaire sans lui faire
perdre son statut de clé primaire. Deux lignes distinctes d’une table ne peuvent jamais avoir la même
valeur pour la clé primaire.
Une clé étrangère dans une table est un ensemble de colonnes qui correspond à la copie de la clé
primaire d'une autre table.
Contrainte d’unicité : aucun composant d’une clé PRIMAIRE ne peut être nul.
Contrainte référentiel : Une clé étrangère est soit nulle, soit égale à une valeur de la clé primaire de la
table qu’elle référence.
Notes de cours – Projet de développement SGDB G. Collard 18
Passage du modèle entité-association au modèle relationnel
Exemple : Réalisateur, comédiens de film
ARTISTES
Id nom prenom anneeNaissance
200 Veber Francis 28/7/1937
201 Cameron James 16/8/1954
202 Spielberg Steven 18/12/1946
203 Villeret Jacques 6/2/1951
204 Prévost Daniel 20/10/1939
FILMS
id titre annee genre idRealisateur
10 Le diner de cons 1998 Comédie 200
11 Titanic 1997 Romance dramatique 201
12 La liste de Schindler 1993 Drame historique 201
ROLES
idFilm idArtiste role
10 203 François Pignon
10 204 Lucien Cheval
Schéma
ARTISTES(id, nom, prenom, anneeNaissance)
FILMS(id, titre, annee, genre, idRealisateur)
ROLES(idFilm, idArtiste, role)
Base de données relationnelles
Notes de cours – Projet de développement SGDB G. Collard 19
Comment convertir un modèle entité-association en un modèle relationnel ?
Un ensemble d’entité (non faible) est représenté par une relation dont les attributs sont les attributs
simples de l’ensemble d’entités.
Les attributs composites sont remplacés par leurs composantes.
Les attributs multivalués : voir plus loin
Exemple :
PERSONNE(no_rn, nom, adresse)
Un ensemble d’entité faible est représenté par une relation dont les attributs sont
- Les attributs de l’ensemble d’entité plus
- Les attributs de la clé de chacun des ensembles d’entités participant aux relations identifiantes
de l’ensemble d’entité faible.
Exemple :
VOL(no, orig, dest, hdep, harriv)
DEPART(date, no, nb_passagers)
Sous forme de tables:
Notes de cours – Projet de développement SGDB G. Collard 20
Une association entre plusieurs ensembles d’entités est représentée par une relation dont les
attributs sont
- Les attributs de l’association plus
- Les attributs de la clé de chacun des ensembles d’entités participant à la relation.
Exemple :
RESERVATION(nom, adresse, date, no)
Cas particulier : rôle (1,1)
Soit E1 et E2 représenté pat T1 et T2 et une association R
Plutôt que de représenter R, on peut ajouter à T1
- Les attributs de la clé de T2
- Les attributs de la relation R
Exemple :
COURS(mmémo, intitulé, h_cours, h_ex, h_tp, no_matr_enseignant)
Les attributs multivalués d’un ensemble d’entité sont représentés à l’aide d’une nouvelle relation dont
les attributs sont
- Les attributs de la clé de la relation correspondant à E
- Un attribut correspondant à l’attribut multivalué (s’il est composite => ses composantes)
Notes de cours – Projet de développement SGDB G. Collard 21
Exemple :
ETUDIANT(no, nom, adresse)
RESULTAT(no_etudiant, cours, note)
Spécialisation/généralisation : Un ensemble d’entités étant une sous classe d’un autre ensemble
d’entités est représenté par une relation dont les attributs sont
- Les attributs de la clé de la super-classe
- Les attributs de la sous-classe
Exemple :
EMPLOYE(no_matr, prenom, nom, date_naiss, adresse)
SECRETAIRE(no_matr, vitesse)
TECHNICIEN(no_matr, grade)
PROFESSEUR(no_matr, discipline)
Notes de cours – Projet de développement SGDB G. Collard 22
Redondance et normalisation
Soit la relation :
IMPLANTATION NOM ADRESSE
CODE
POSTAL LOCALITE N° Distributeur
Date
Vente
Quantite
Vendue
CODE
PRODUIT DESIGNATION
1 HELP Quai du Barbou, 2 4000 Liège 2 18-10-17 6 bo0023 coca can 1 HELP Quai du Barbou, 3 4000 Liège 2 18-10-17 32 bo0027 EAU 1 HELP Quai du Barbou, 4 4000 Liège 2 19-10-17 4 bo0023 coca can 2 IPEPS Seraing Sup Rue Colard Trouillet, 48 4100 Seraing 5 18-10-17 5 bo0023 coca can 2 IPEPS Seraing Sup Rue Colard Trouillet, 48 4100 Seraing 5 18-10-17 8 bo0027 EAU 2 IPEPS Seraing Sup Rue Colard Trouillet, 48 4100 Seraing 5 19-10-17 2 bo0023 coca can 3 IPES Avenue Delchambre, 13 4500 Huy 7 18-10-17 7 bo0023 coca can 3 IPES Avenue Delchambre, 14 4500 Huy 7 18-10-17 5 bo0027 EAU 3 IPES Avenue Delchambre, 15 4500 Huy 8 18-10-17 8 bo0023 coca can 3 IPES Avenue Delchambre, 16 4500 Huy 7 19-10-17 12 bo0023 coca can 3 IPES Avenue Delchambre, 17 4500 Huy 8 19-10-17 3 bo0023 coca can 4 IPEPS Verviers tech rue aux laines, 69 4800 Verviers 3 18-10-17 3 bo0023 coca can 4 IPEPS Verviers tech rue aux laines, 70 4801 Verviers 3 18-10-17 5 bo0027 EAU 4 IPEPS Verviers tech rue aux laines, 71 4802 Verviers 3 19-10-17 3 bo0023 coca can ➢ Proposez une clé primaire pour cette relation
➢ Cette relation contient-elles des redondances ? Si oui, lesquels ? Justifiez brièvement.
➢ Si la relation contient des redondances, proposez une solution contenant exactement la même information, mais sans redondance.
Notes de cours – Projet de développement SGDB G. Collard 23
Prix
Achat
Prix
Vente
0,20 0,51
0,22 0,51
0,20 0,51
0,20 0,51
0,22 0,51
0,20 0,51
0,20 0,51
0,22 0,51
0,20 0,51
0,20 0,51
0,20 0,51
0,20 0,51
0,22 0,51
0,20 0,51
Imaginons qu’on souhaite représenter des personnes, identifiées par leur numéro de sécurité
sociale, caractérisées par leur nom, leur prénom, les véhicules qu’elles ont achetés à une certaine
date et à un certain prix. Un véhicule est caractérisé par un type, une marque et une puissance.
On peut aboutir à la représentation relationnelle suivante :
Des redondances sont présentes et conduiront à des problèmes de contrôle de la cohérence de
l’information de mise à jour (changement de nom à reporter dans de multiples tuples), de la perte
d’information lors de la suppression de données (disparition des informations concernant un type
de véhicule) et de difficulté à représenter certaines informations (un type de véhicule sans
propriétaire).
Principes de la normalisation
La théorie de la normalisation est une théorie destiné à concevoir un bon schéma d’une BD sans
redondance d’information et sans risques d’anomalie de mise à jour.
La théorie de la normalisation est fondée sur deux concepts principaux :
- Les dépendances fonctionnelles : Elles traduisent des contraintes sur les données
- Les formes normales : Elles définissent des relations bien conçues
Première forme normale (1FN)
Une relation est en 1NF si elle possède au moins une clé et si tous ses attributs sont
atomiques.
Un attribut est atomique s’il ne contient qu’une seule valeur pour un tuple donné, et donc s’il ne
regroupe pas un ensemble de plusieurs valeurs.
Exemple :
Soit la relation PERSONNE : PERSONNE( id, nom, prenom, date_naissance, diplôme)
Id Nom Prenom Date_naissance diplôme
1 Dupont Martine 14/7/1980 Master en informatique – agrégation AESS
2 Durant Simon 28/2/1970 Master en économie
Une personne peut avoir plusieurs diplômes => l’attribut diplôme n’est pas atomique
Pour que la relation soit en 1FN, on pourrait ajouter l’attribut diplôme à la clé et avoir ainsi deux tuples
pour Martine Dupont.
Notes de cours – Projet de développement SGDB G. Collard 24
PERSONNE( id, diplôme ,nom, prenom, date_naissance)
Id Nom Prenom Date_naissance diplôme
1 Dupont Martine 14/7/1980 Master en informatique
1 Dupont Martine 14/7/1980 agrégation AESS
2 Durant Simon 28/2/1970 Master en économie
Deuxième forme normale (2FN)
La deuxième forme normale permet d’éliminer les dépendances entre des parties de clé et des
attributs n’appartenant pas à cette clé.
Une relation est en 2FN si elle est en 1FN et si tout attribut qui n’est pas dans une clé ne
dépend pas d’une partie seulement d’une clé.
(attention : vérifier pour la clé primaire et pour toutes les clés candidates)
Exemple :
Soit la relation EMPLOYE : EMPLOYE(nom, profession, salaire)
Soit les dépendances fonctionnelles profession -> salaire
À partir de la profession, on peut déterminer le salaire, donc la clé n’est pas élémentaire. Donc, cette
relation n’est pas en 2FN
Pour avoir un schéma relationnel en 2FN, il faut alors décomposer la relation EMPLOYE en deux
relations :
• EMPLOYE(nom, profession)
• PROFESSION(profession, salaire)
Reprenant la relation précédente PERSONNE( id, diplôme ,nom, prenom, date_naissance)
Cette relation n’est pas en 2FN car id détermine le nom (id -> nom), id détermine le prénom
(id -> prenom), id détermine la date de naissance (id -> date_naissance).
Ces relations le sont :
• PERSONNE( id, nom, prenom, date_naissance)
• DIPLÔME( id, diplôme)
Troisième forme nomale (3FN)
La troisième forme normale permet d’éliminer les dépendances entre les attributs n’appartenant pas
à une clé
Une relation est en 3FN si elle est en 2FN et si tout attribut n’appartenant pas à une clé
ne dépend pas d’un autre attribut n’appartenant pas à une clé. (concerne la clé primaire
et toutes les clés candidates)
Notes de cours – Projet de développement SGDB G. Collard 25
Exemple :
Soit la relation Profession
PROFESSION(profession, salaire, prime) et les dépendances fonctionnelles sur cette relation :
• profession-> salaire
• salaire -> prime
Cette relation n’est pas en 3FN car salaire, qui n’est pas la clé, détermine prime.
Pour avoir un schéma relationnel en 3FN, il faut décomposer Profession
• PROFESSION(profession, salaire)
• SALAIRE(salaire, prime)
La forme normale de Boyce-Codd (BNCF)
La forme normale de Boyce-Codd permet d’éliminer les dépendances entre les attributs n’appartenant
pas à une clé vers les parties de clé.
Une relation est en BCFN si elle est en 3FN et si tout attribut qui n’appartient pas à la clé
n’est pas source d’une dépendance fonctionnelle vers une partie d’une clé.
Exemple :
Soit la relation PERSONNE : PERSONNE(numSS, pays, nom, region) et les dépendances fonctionnelles :
• numSS Pays -> nom
• numSS pays -> region
• region -> pays
Il existe une dépendance fonctionnelle qui n’est pas issue d’une clé et qui détermine un attribut
appartenant à une clé. Cette relation est en 3FN, mais pas en BCNF car en BCNF, toutes les
dépendances fonctionnelles sont issues de la clé.
Pour avoir un schéma en BCNF, il faut décomposer cette relation :
• PERSONNE(numSS, nom, region)
• REGION(region, pays)
Attention : une décomposition en BCFN ne préserve pas toujours toutes les dépendances
fonctionnelles.
Conclusion
La normalisation permet de décomposer un schéma relationnelle afin d’obtenir des relations non
redondantes.
La 3FN est souhaitable car il est toujours possible de l’obtenir sans perte d’informations et sans perte
de dépendances fonctionnelles. La BCNF est également indiquée car elle est un peu plus puissante et
plutôt plus simple que la 3FN.
Notes de cours – Projet de développement SGDB G. Collard 26
La BCNF n’est pas encore suffisante pour éliminer toutes les redondances. Il existe pour cela les 4FN
et 5FN qui ne sont pas abordées dans ce cours. Notons également que les cas de non-4FN et de
non-5FN sont assez rares dans la réalité.
Exercices
Exercice 1
Convertir les modèles entité-association des 5 exercices du chapitre précédent en les modèles
relationnels équivalents.
Notes de cours – Projet de développement SGDB G. Collard 27
Chapitre 4 – PostgreSQL
PostgreSQL est un système de gestion de base de données relationnelle et objet (SGBDRO).
PostreSQL est un logiciel libre, disponible selon les termes d’une licence de type BSD (Berkeley
Software Distribution License) Comme les projets libres Apache, Linux, PostgreSQL n’est pas contrôlé
par une seule entreprise, mais est fondé sur une communauté mondiale de développeurs et
d’entreprises.
Histoire :
• Ingres (Intelligent Graphic RElational System) développée à Berkeley par Michaël Stonebrake
• 1985 : Michaël Stonebrake recommence le développement de Ingres et le nomme
PostgreSQL(Post-ingres)
• 1995 : ajout des fonctionnalités SQL => Postgres95
• 1996 : changement de nom => PostgreSQL
Caractéristiques
• PostgreSQL utilise des type de données simples (entiers, caractères, …) et de types de données
composés (créé par l’utilisateur). L’utilisateur peut créer des types, utiliser l’héritage de type.
• PostgreSQL est pratiquement conforme aux normes ANSI SQL 89, SQL92, SQL99, SQL2003 et
SQL2008.
• Il fonctionne sur LINUX et toute sorte d’UNIX. Depuis, la version Windows 8, il fonctionne
nativement sur Windows.
Interfaces utilisateurs
• psql : permet la saisie de requêtes SL, directement ou par l’utilisation de procédures stockées.
• pgadmin : outil d’administration graphique pour PostgreSQL distribué selon les termes de la
licence PostgreSQL
• phpPgAdmin : interface web d’administration pour PostgreSQL, écrit en PHP
pilote pour c#
Npgsql :
• dataprovider ADO.net,
• open source,
• écrit en C#,
• permet d’accèder au serveur de base de données PostgreSQL
Notes de cours – Projet de développement SGDB G. Collard 28
Installation de PostgreSQL
https://www.postgresql.org/
Une fois l’installateur Interactive installer by EnterpriseDB téléchargé, lancer-le.
Cliquer sur suivant
Définir le répertoire d’installation Cliquer sur suivant
Notes de cours – Projet de développement SGDB G. Collard 29
Définir le répertoire de données
Encoder le mot de passe pour le superuser postres
Notes de cours – Projet de développement SGDB G. Collard 30
Laisser le port par défaut Laisser la locale par défaut
Cliquer sur Suivant pour démarrer l’installation
Notes de cours – Projet de développement SGDB G. Collard 31
stack Builder permet d’installer des outils
supplémentaires
Notes de cours – Projet de développement SGDB G. Collard 32
installation du pilote ngpSQL
Notes de cours – Projet de développement SGDB G. Collard 33
Création d’une base de donnée
➢ Lancer l’application pgadmin
➢ Entrer le mot de passe du superuser
➢ Cliquer sur server et entrer le mot de passe du superuser postgres
La fenêtre suivante apparaît
Notes de cours – Projet de développement SGDB G. Collard 34
➢ Pour créer une nouvelle base de données, cliquer sur Databases – create – Database …
1. Créer la base de données Bibliotheque
2. Se connecter à la base de données Bibliotheque
Notes de cours – Projet de développement SGDB G. Collard 35
3. Se déconnecter de la base de données Bibliotheque
4. Se déconnecter du serveur
5. quitter pgadmin
Se connecter à la base de données Bibliotheque à partir du terminal de commande
1. 2. 3. Ouvrir un terminal de commande et taper cd " c:\Program Files\PostgreSQL\11\bin"
Se connecter au serveur de base de donnée en tapant : psql –U postgres –d Bilbliotheque
Entrer le mot de passe
Notes de cours – Projet de développement SGDB G. Collard 36
➢ Pour quitter taper \q
➢ Pour ne plus voir apparaître ce message, taper la commande chcp 1252
Notes de cours – Projet de développement SGDB G. Collard 37
Type de données
PostgreSQL a un large type de données disponibles. Les utilisateurs peuvent ajouter de nouveaux
types à PostgreSQL en utilisant la commande CREATE TYPE.
Type numérique
tableau numérique - https://docs.postgresql.fr/8.1/datatype.html
Type de caractères
tableau de caractères - https://docs.postgresql.fr/8.1/datatype-character.html
Type Date – Heure
tableau date-heure - https://docs.postgresql.fr/8.1/datatype-datetime.html
Type boolean
Ce type utilise un octet de stockage.
Les valeurs booléennes sont affichées en utilisant les lettres t (true) et f (false)
Valeur littéral valide pour true Valeur littéral valide pour false
TRUE FALSE
'true' 'false'
't' 'f'
'1' '0'
'y' 'n'
'yes' 'no'
Il est recommandé d’utiliser TRUE et FALSE qui sont compatible avec la norme SQL
Notes de cours – Projet de développement SGDB G. Collard 38
Type géométrique
type géométrique - https://docs.postgresql.fr/8.1/datatype-geometric.html
ProgreSQL a un ensemble de fonctions et d’opérateurs permettant d’effectuer différentes opérations
géométriques comme l’agrandissement, la translation, la rotation, la détermination des intersections
Type d’adresse réseau
type d’adresse réseau - https://docs.postgresql.fr/8.1/datatype-net-types.html
Type tableau
Voir https://docs.postgresql.fr/8.1/arrays.html
Ceci pour des variables multi-valeurs.
Attention : première forme normale n’est plus respectée, redondance des informations !!!
Notes de cours – Projet de développement SGDB G. Collard 39
Type composite
Soit une adresse composée d’une rue, d’une localité et d’un numéro de code postal
Insérer dans une table
Accéder à une variable composite
ou en utilisant le nom de la table
Il ne faut pas placer des parenthèses autour des noms de colonnes apparaissant après SET, mais il faut
les mettre lors de la référence à la même colonne à droite du signe d’égalité.
Comptabilité
Les types suivants sont conformes à la norme SQL : bit, bit varying, boolean, char, character varying,
character, varchar, date, double précision, integer, interval, decimal, real, smallint, time, timestamp.
Notes de cours – Projet de développement SGDB G. Collard 40
Créer une table (CREATE TABLE)
(pas les mettes tous comme exemples)
Modifier une table (ALTER TABLE)
Ajouter une colonne
Supprimer une colonne
Ajouter une contrainte
Supprimer une contrainte
Les contraintes non NULL n’ont pas de nom
Notes de cours – Projet de développement SGDB G. Collard 41
Pour voir la liste des contraintes :
Pour voir le nom des contraintes de la table personne
Modifier des valeurs par défaut
Supprimer une valeur par défaut, revient à modifier la valeur par défaut à NULL
Cela n’affecte pas les lignes existantes de la table, mais uniquement la valeur par défaut pour les
futures commandes INSERT.
Modifier les types de valeur d’une colonne
Elle ne peut réussir que si chaque valeur de la colonne peut être convertie dans le nouveau type par
une conversion implicite. Si une conversion plus complexe est nécessaire, une clause USING peut être
ajoutée qui indique comment calculer les nouvelles valeurs à partir des anciennes.
Renommer des colonnes
Renommer une table
Supprimer une table
Notes de cours – Projet de développement SGDB G. Collard 42
Créer les tables de la base de données Bibliothèques
Voici le modèle entité-association
Le modèle relationnel est :
PERSONNE(idPersonne, nom, prenom, GSM, (rue, cp, localite))
CATEGORIE(idCategorie, nom, sous_categorie)
LIVRE(isbn, titre, auteur, edition,#idCategorie)
EXEMPLAIRE(idLivre, #isbn)
EMPRUNT(#idPersonne, #iidLivre, date_sortie, date_retour, date_prolongation)
Les contraintes sont
➢ Nom et prénom doivent au moins contenir 3 caractères
➢ Le champ nom de la table catégorie ne peut être NULL
➢ La date de prolongation doit être comprise entre la date de sortie et
la date de sortie + 28 jours
➢ Si la date de prolongation est non null alors la date de retour doit être supérieure à la
date de prolongation sinon la date de retour doit être supérieure à la date de sortie
➢ La date de sortie a comme valeur par défaut la date courante.
Insérer des enregistrements dans les tables
Via pgadmin
➢ Lancer pgadmin et se connecter
Notes de cours – Projet de développement SGDB G. Collard 43
➢ Se connecter au serveur PostgreSQL 11 (entrer de nouveau le mot de passe)
➢ Se connecter à la base de données en cliquant sur la base de données
Via le terminal de commande de pgAdmin
➢ Cliquer droit sur la base de données – cliquer sur Query Tool
Notes de cours – Projet de développement SGDB G. Collard 44
➢ Se déconnecter de la base de données Bibliotheque
➢ Se déconnecter du serveur
➢ Quitter pgadmin
Insertion d’enregistrements dans une table. (INSERT)
Notes de cours – Projet de développement SGDB G. Collard 45
Modifier des enregistrements (UPDATE)
Supprimer un(des) enregistrement(s) (DELETE)
Notes de cours – Projet de développement SGDB G. Collard 46
Application : Bibliotheque
➢ Lancer pgadmin et se connecter
➢ Ouvrir une fênetre de commande (Query tool)
➢ Encoder les insertions suivantes et le sauver dans un fichier
➢ Quitter l’éditeur.
➢ Ouvrir un query tools
Notes de cours – Projet de développement SGDB G. Collard 47
Ouvrir le fichier insertPersonne.sql
Exécuter le fichier.
Consulter les données (SELECT)
Tester les commandes suivantes :
➢ Obtenir toutes les lignes de la table PERSONNE
➢ Lister les nom et prénom des personnes habitant Seraing
➢ Afficher les différentes localités des personnes
Notes de cours – Projet de développement SGDB G. Collard 48
➢ Afficher les nom et prénom des personnes habitant Seraing et dont le nom commence par la
lettre C
➢ Afficher le nom et prénom des personnes n’habitant pas Seraing et dont le nom commence
par C, et obtenir également les personnes habitant Verviers.
➢ Afficher les personnes pour lesquels on ne connait pas l'adresse
➢ Classer par ordre croissant les noms des personnes
➢ Classer par ordre décroissant les noms et prénoms des personnes
➢ Donner le nombre de livres empruntés
➢ Donner le nombre de livres
➢ Donner les nombres de livres par auteur
Notes de cours – Projet de développement SGDB G. Collard 49
➢ Donner le nombre de livres empruntés par personne
➢ Donner le nombre de livres empruntés pour les personnes ayant emprunté plus de 3 livres
➢ Donner la moyenne des durées des emprunts
➢ Donner le nombre d’emprunts dont la durée est supérieure à la moyenne des durées des
emprunts
➢ Quels sont les personnes qui ont emprunté un livre entre 10 et 20 jours
➢ Quels sont les personnes qui ont emprunté le livre Couleur d’Europe de Jean-Philippe
Lerclos ?
Notes de cours – Projet de développement SGDB G. Collard 50
➢ Obtenir le(s) personne(s) qui ont actuellement empruntés dont le délai est dépassé
➢ Rechercher la date du premier emprunt de la personne dont l’id est 2
Attribut dérivé
Pour créer un attribut dérivé dans PostgreSQL, il y a deux possibilités
- soit une fonction calculée ou une expression dans une requête, dans ce cas la valeur de
l’attribut n’est pas stockée de manière permanente dans la table
o expression dans une requête
o via une fonction
- soit utiliser une colonne calculée ou mettre à jour manuellement (ou trigger ) lors de
l’insertion ou la mise à jour. Dans ce cas, la valeur de l’attribut dérivé est stockée de manière
permanente dans la table.
Notes de cours – Projet de développement SGDB G. Collard 51
o Trigger lors de l’insertion ou la mise à jour
o Colonne calculée (à partie de la version 12 de postgreSQL)
Transactions
Les transactions sont un concept fondamental de tous les systèmes de bases de données. Le point
essentiel d'une transaction est qu'il assemble plusieurs étapes en une seule opération tout-ou-rien.
Les états intermédiaires entre les étapes ne sont pas visibles par les autres transactions concurrentes,
et si un échec survient empêchant la transaction de bien se terminer, alors aucune des étapes n'affecte
la base de données.
Une transaction est réalisée en entourant les commandes SQL de la transaction avec les commandes
BEGIN et COMMIT.
Si, au cours de la transaction, on décide qu’on ne veut pas valider, on exécute la commande ROLLBACK
au lieu de COMMIT, et toutes nos mises à jour jusqu'à maintenant seront annulées.
En fait, PostgreSQL traite chaque instruction SQL comme étant exécutée dans une transaction.
Si on ne lance pas une commande BEGIN, alors chaque instruction individuelle se trouve enveloppée
avec un BEGIN et (en cas de succès) un COMMIT implicites. Un groupe d'instructions entouré par un
BEGIN et un COMMIT est quelque fois appelé un bloc transactionnel.
Notes de cours – Projet de développement SGDB G. Collard 52
Exemple :
Supposons qu’on puisse réserver un livre à la bibliothèque, dès que le livre réservé est emprunté, on
supprime la réservation de la table RESERVATION et on ajoute un enregistrement dans la table
EMPRUNT. Si une de ces deux commande SQL (delete, insert) échoue, aucune des deux ne doit être
exécutée.
Notes de cours – Projet de développement SGDB G. Collard 53
Héritage
PostgreSQL implémente l’héritage des tables
Soit la table PERSONNE qui contient les patients, soit la table MEDECIN qui contient les médecins, cette
table hérite de la table PERSONNE et contient deux champs supplémentaires : matricule et specialite.
On utilise le mot-clé inherits pour l’héritage.
Notes de cours – Projet de développement SGDB G. Collard 54
➢ Insérons quelques enregistrements
➢ Pour lister toutes les personnes :
➢ Pour lister tous les médecins :
➢ Pour lister les personnes qui ne sont pas médecin (mot-clé only)
➢ Pour lister toutes les personnes et savoir s’ils font partie des tables enfants
Notes de cours – Projet de développement SGDB G. Collard 55
➢ Insérons dans la table MEDECIN, un médecin avec le même ONSS que le médecin
Toubib Jean
L’insertion s’est bien déroulée, malgré que ONSS est une clé primaire de la table PERSONNE.
➢ Insérons dans la table MEDECIN, un médecin avec le même ONSS que Anspach Jules qui n’est
pas médecin.
L’insertion s’est bien déroulée, malgré un ONSS identique. Or ONSS est la clé primaire de la table
PERSONNE.
➢ Insérons dans la table PERSONNE, un enregistrement avec le même ONSS qu’une autre
personne qui n’est pas médecin.
L’insertion est impossible car viole la clé primaire.
La contrainte UNIQUE du champ ONSS n’a pas été héritée par la table MEDECIN.
Notes de cours – Projet de développement SGDB G. Collard 56
➢ Supprimons tous les enregistrements des tables PERSONNE et MEDECIN
➢ Ajoutons une contrainte d’unicité du champ onss à la table MEDECIN
➢ Ensuite, réinsérons les enregistrements dans la table MEDECIN
Maintenant, nous ne pouvons plus ajouter un médecin ou une personne ayant le même ONSS qu’un
autre médecin.
Les contraintes d’unicité ne sont pas héritées, il faut les redéfinir dans les tables enfants.
➢ Ajoutons une contrainte de vérification à la table PERSONNE
Par contre, les contraintes de vérification sont héritées.
En conclusion, les contraintes peuvent être définies sur les tables de la hiérarchie d’héritage. Toutes
les contraintes de vérification d’une table parent sont automatiquement héritées par tous ses enfants.
Néanmoins, les autres types de contraintes (unicité, clé étrangère, …) ne sont pas hérités.
Notes de cours – Projet de développement SGDB G. Collard 57







IPEFA Sup Seraing - Verviers
Projet de développement
SGDB
Notes de cours
Georgette Collard
2025-2026
Notes de cours – Projet de développement SGDB G. Collard 1
Table des matières
Chapitre 5 : PostgreSQL server et application console en C# ................................................................. 4
Aperçu de l’application finale .............................................................................................................. 4
La base de données ........................................................................................................................... 13
Création de tables en postgreSQL ................................................................................................. 14
Création des tables ........................................................................................................................ 14
Conception de l’application ............................................................................................................... 16
Architecture à deux couches ......................................................................................................... 16
Espaces de noms ........................................................................................................................... 17
Chapitre 6 : Création du projet de l’application dans Visual C# ............................................................ 20
Signification des espaces de noms: ................................................................................................... 22
Aperçu du projet général terminé ..................................................................................................... 22
Exécution de l'application ............................................................................................................. 24
Chapitre 7 : Les classes métiers ............................................................................................................. 28
Diagrammes de classes ..................................................................................................................... 28
Implémentation en C# ....................................................................................................................... 31
Correspondances entre les types de données en PostgreSQL et les types de données en C# ..... 33
Classe Livre .................................................................................................................................... 34
Classe Exemplaire .......................................................................................................................... 36
Classe Personne ............................................................................................................................. 37
Classe Emprunt .............................................................................................................................. 38
Classe Adresse – composant composite ....................................................................................... 39
Chapitre 8 : Création de la couche accès aux données ......................................................................... 39
Diagramme de classes ....................................................................................................................... 41
Etablir une connexion avec la base de données ............................................................................... 69
Requête SELECT ............................................................................................................................. 70
Requête contenant un ou des paramètre(s) ................................................................................. 71
Commande INSERT – UPDATE – DELETE ....................................................................................... 73
Transaction .................................................................................................................................... 74
L'exception ExceptionAccesBD .......................................................................................................... 77
Chapitre 9 : Création de la couche présentation .................................................................................. 78
La classe Program .............................................................................................................................. 79
La classe présentation ....................................................................................................................... 79
Notes de cours – Projet de développement SGDB G. Collard 2
Classe AccesControl
Notes de cours – Projet de développement SGDB G. Collard 3
........................................................................................................................................................... 93
Chapitre 5 : PostgreSQL server et application console en C#
Aperçu de l’application finale
Apres avoir discuté avec un client fictif, voici les premières fonctionnalités qu'il veut obtenir dans
l'application de Bibliotheque :
• Obtenir la liste des catégories
• Obtenir la liste des livres appartenant à une catégorie
• Obtenir les informations sur les exemplaires (identifiant, emprunté/disponible) dont le titre
et l’auteur sont donnés
• Obtenir la liste des emprunteurs
• Obtenir les informations sur un emprunteur
• Ajout d’un emprunteur
• Ajout d’une catégorie
• Ajout d’un livre/exemplaire
Notes de cours – Projet de développement SGDB G. Collard 4
• Supprimer un livre/exemplaire
• Modifier les coordonnées d’un emprunteur
• Faire en sorte qu’un exemplaire est emprunté par une personne
• Faire en sorte qu’un exemplaire est restitué
• Faire en sorte qu’un exemplaire est prolongé
Notes de cours – Projet de développement SGDB G. Collard 5
Notes de cours – Projet de développement SGDB G. Collard 6
Notes de cours – Projet de développement SGDB G. Collard 7
Menu principal
Quand on sélectionne 1 dans le menu principal, le l’écran « Afficher les livres » apparaît
Notes de cours – Projet de développement SGDB G. Collard 8
Quand on sélectionne 1 dans le menu Afficher les livres, la liste des catégories stockées dans la base
de données s’affiche
Quand on sélectionne 2 dans le menu Afficher les livres, puis qu’on entre l’identifiant de la catégorie
liste des livres appartenant à cette catégorie s’affichent
Quand on sélectionne 3 dans le menu Afficher les livres, puis qu’on entre le titre et l’auteur, la liste des
exemplaires empruntés et disponibles correspondant à ce titre et cet auteur s’affichent
Notes de cours – Projet de développement SGDB G. Collard 9
Quand on sélectionne 2 dans le menu principal le menu « Afficher les personnes » s’affiche
Quand on sélectionne 1 dans le menu Afficher les personne, la liste des personnes stockée dans la base
de données s’affichent
Quand on sélectionne 2 dans le menu Afficher les personnes, puis on entre l’identifiant de la personne,
les informations de cette personne sont affichée
Notes de cours – Projet de développement SGDB G. Collard 10
Quand on sélectionne 2 dans le menu principal le menu «Gestion des livres » s’affiche
Quand on sélectionne 1 dans le menu Gérer les livres puis qu’on entre le nom de la catégorie et le nom
de la sous-catégorie, cette catégorie est ajoutée dans la base de donnée.
Quand on sélectionne 2 dans le menu Gérer les livres puis qu’on entre l’isbn, si aucun livre avec cet
isbn existe, on entre le titre, l’auteur, l’édition et l’identifiant de la catégorie. Une insertion dans la
table livre et une insertion dans la table exemplaire sont faites. Si un livre avec cet isbn existe, alors
un exemplaire est ajouté à la base de données.
Notes de cours – Projet de développement SGDB G. Collard 11
Ecran Gestion des personnes
Ecran Gestion des Emprunt
Notes de cours – Projet de développement SGDB G. Collard 12
La base de données
Voici le modèle entité-association
Le modèle relationnel est :
PERSONNE(idPersonne, nom, prenom, GSM, (rue, cp, localite))
CATEGORIE(idCategorie, nom, sous_categorie)
LIVRE(isbn, titre, auteur, edition,#idCategorie)
EXEMPLAIRE(idLivre, #isbn)
EMPRUNT(#idPersonne, #iidLivre, date_sortie, date_retour, date_prolongation)
Notes de cours – Projet de développement SGDB G. Collard 13
Création de tables en postgreSQL
Définition d’un type composite : adresse
Création des tables
Notes de cours – Projet de développement SGDB G. Collard 14
Contraintes
Notes de cours – Projet de développement SGDB G. Collard 15
Conception de l’application
Architecture à deux couches
1) L'application conçue en C# de « Bibliotheque » que nous allons décrire dans ce document est
constituée de 2 couches distinctes:
2) Couche présentation (presentation layer): cette couche gère les accès à la console (clavier et
écran) et effectue les traitements sur les données : tests, calculs, ... Dans notre application, il
va s'agir d'une classe appelée Presentation.
Couche accès aux données (data access layer): cette couche effectue les accès en lecture et
en écriture dans la base de données.
Voici une figure montrant ou se situe chaque couche:
Pendant l'exécution de l'application, les objets des classes Presentation et AccesBD s'échangent des
objets de classes directement construites à partir des tables de la base de données (CATEGORIE, LIVRE,
PERSONNE, …). Ces objets sont appelés objets métier.
Pourquoi avoir divisé l'application en plusieurs couches différentes?
- On peut changer la nature de l'interface utilisateur (ex: passer d'une application console à
une application fenêtrée), donc changer la couche présentation, sans avoir à modifier,
ajouter ou supprimer la moindre ligne de code dans les autres classes.
- On peut changer la couche accès aux données pour qu'elle prenne, par exemple, en compte
l'usage de procédures stockées, sans avoir à modifier, ajouter ou supprimer la moindre ligne
de code dans les autres classes.
Notes de cours – Projet de développement SGDB G. Collard 16
Espaces de noms
Sur base de l'architecture de l'application, voici les espaces de noms qui vont être créés dans
l'application:
Trois espaces de noms vont être créés :
- L'espace de noms couchePresentation va stocker les classes d'accès à l'écran et au clavier.
- L'espace de noms coucheAccesBD va stocker les classes d'accès à la base de données.
- L'espace de noms classesMetier va stocker les classes métier.
Notes de cours – Projet de développement SGDB G. Collard 17
Notes de cours – Projet de développement SGDB G. Collard 18
Notes de cours – Projet de développement SGDB G. Collard 19
Chapitre 6 : Création du projet de l’application dans Visual C#
➢ Cliquer sur Créer un projet
➢ Choisir Application Console et cliquer sur Suivant
Notes de cours – Projet de développement SGDB G. Collard 20
➢ Donner un nom à votre projet et cliquer sur Créer
Le projet contient un fichier appelé Program.cs. Dans ce fichier se trouve comme contenu initial la
classe Program contenant une méthode statique Main. Celle-ci sera la toute première méthode
exécutée lorsque l'exécution du programme commencera.
Dans le projet, créez les dossiers : classesMetier, coucheAccesBD et couchePresentation.
Pour créer un dossier, clic droit sur le nom du projet -> Ajouter -> Nouveau dossier.
Notes de cours – Projet de développement SGDB G. Collard 21
Signification des espaces de noms:
- Dans le dossier classesMetier, on va placer les fichiers qui vont stocker les classes metier :
Categorie, Livre, Personne, Emprunt.
- Dans le dossier coucheAccesBD, on va placer les fichiers qui vont stocker les classes
permettant les accès aux données : AccesBD et ExceptionAccesBD.
- Dans le dossier couchePresentation, on va placer les fichiers qui vont stocker les classes
permettant les accès à la console : AccesConsole et Presentation ainsi que la classe Program.
Chaque dossier va correspondre à un espace de noms spécifique.
Aperçu du projet général terminé
Le fichier Program.cs a été déplacé dans le dossier couchePresentation.
Notes de cours – Projet de développement SGDB G. Collard 22
Notes de cours – Projet de développement SGDB G. Collard 23
Exécution de l'application
Voici comment les éléments de l'application vont fonctionner ensemble :
1. La classe Program contient la méthode statique Main. Cette méthode est automatiquement
exécutée quand démarre le programme.
2. 3. 4. 5. La méthode Main crée un objet de la classe Presentation.
Le constructeur de la classe Presentation crée un objet de la classe AccesBD.
La méthode Main donne ensuite le contrôle à la méthode menuPrincipal de l'objet de la classe
Presentation.
La méthode MenuPrincipal affiche le menu principal de l'application. Dès que l'utilisateur fait
un choix, cette methode invoque la methode en relation avec le choix effectué par l'utilisateur.
Par exemple, si l'utilisateur choisit 2, la methode MenuAfficherPersonne() est exécutée. Le
menu Afficher Personne s’affiche, si l’utilisateur choisit 1, la méthode AfficherPersonne() est
exécutée.
6. La methode AfficherPersonne de l'objet de la classe Presentation appelle la methode
ListePersonne de l'objet de la classe AccesBD pour obtenir de la base de données la liste des
personnes.
Diagramme de séquence
Menu principal
Notes de cours – Projet de développement SGDB G. Collard 24
Notes de cours – Projet de développement SGDB G. Collard 25
Menu Afficher les livres
Notes de cours – Projet de développement SGDB G. Collard 26
Menu Afficher Personne
Notes de cours – Projet de développement SGDB G. Collard 27
Chapitre 7 : Les classes métiers
Tout comme les tables de la base de données, les classes métier décrivent des entités du métier
(compte, client, transaction ..., pour une banque). Les instances des classes métier sont les objets
métier.
Diagrammes de classes
Nous allons convertir les tables CATEGORIE, LIVRE, EXEMPLAIRE, PERSONNE, EMPRUNT en les classes
Catégorie, Livre, Exemplaire, Personne, Emprunt.
Représente le type composite adresse défini dans postgreSQL. Ceci est un composant de la classe
Personne
Notes de cours – Projet de développement SGDB G. Collard 28
Notes de cours – Projet de développement SGDB G. Collard 29
Notes de cours – Projet de développement SGDB G. Collard 30
Espace de noms classesMetier
Les classes décrites dans ce chapitre sont placées dans l'espace de noms classesMetier.
Implémentation en C#
Prenons le cas de la classe Categorie pour illustrer comment passer d'une table de la base de données
a une classe en C#.
Notes de cours – Projet de développement SGDB G. Collard 31
Si une colonne de la table Categorie devient un attribut de la classe Categorie. Le type de données de
cette colonne de la table doit être similaire à celui de l'attribut correspondant dans la classe.
On ajoute trois constructeurs:
• Categorie() : constructeur sans paramètre
• Categorie(Categorie categorie) : constructeur avec un paramètre de type Categorie, il copie
l’état de cet objet dans l’objet en cours de création
• Categorie(int idcategorie, string nom, string souscategorie) : constructeur qui assigne une
valeur à chaque attribut de l’objet en cours de création
On ajoute un constructeur supplémentaire car livre utilise cette classe et idCategorie sera
éventuellement le seul champ spécifié
• Categorie (int idCategorie)
Notes de cours – Projet de développement SGDB G. Collard 32
Correspondances entre les types de données en PostgreSQL et les types de données en C#
Type postgreSQL Type c#
Bigint Int64
Boolean Boolean
Box, Circle, Line, LSeg, Path,
Point, Polygon Object
Bytea Byte[]
Date DateTime
Double Double
Integer Int32
Numeric Decimal
Real Single
Smallint Int16
Text String
Time DateTime
Timestamp DateTime
Varchar String
Array Array
type composite struct, objet
Notes de cours – Projet de développement SGDB G. Collard 33
Classe Livre
Un livre appartient à une catégorie => attribut categorie (de type Categorie) dans la classe Livre
Notes de cours – Projet de développement SGDB G. Collard 34
Notes de cours – Projet de développement SGDB G. Collard 35
Classe Exemplaire
Notes de cours – Projet de développement SGDB G. Collard 36
Classe Personne
Notes de cours – Projet de développement SGDB G. Collard 37
Classe Emprunt
Emprunt comprend une personne et un exemplaire (composant faible – agrégation)
Notes de cours – Projet de développement SGDB G. Collard 38
Classe Adresse – composant composite
Chapitre 8 : Création de la couche accès aux données
Notes de cours – Projet de développement SGDB G. Collard 39
Concrètement, la couche accès aux données consiste en les classes qui vont :
1. Créer et stocker la connexion avec la base de données.
2. 3. Transmettre les requêtes SQL a PostgreSQL Server.
Transformer toute entité stockée dans la base de données, c’est-à-dire toute ligne d'une table,
en un objet métier et réciproquement.
Par exemple, dans la figure ci-dessous la couche accès aux données convertit une ligne de la table
Client en un objet métier qui est une instance de la classe métier Client. Cet objet métier stocke les
données de la ligne provenant de la table Client.
Notes de cours – Projet de développement SGDB G. Collard 40
Diagramme de classes
Les classes AccesBD et ExceptionAccesBD se trouvent dans l'espace de noms coucheAccesBD.
Notes de cours – Projet de développement SGDB G. Collard 41
La classe AccesBD
Notes de cours – Projet de développement SGDB G. Collard 42
Notes de cours – Projet de développement SGDB G. Collard 43
Notes de cours – Projet de développement SGDB G. Collard 44
Notes de cours – Projet de développement SGDB G. Collard 45
Notes de cours – Projet de développement SGDB G. Collard 46
Notes de cours – Projet de développement SGDB G. Collard 47
Notes de cours – Projet de développement SGDB G. Collard 48
Notes de cours – Projet de développement SGDB G. Collard 49
Notes de cours – Projet de développement SGDB G. Collard 50
Notes de cours – Projet de développement SGDB G. Collard 51
Notes de cours – Projet de développement SGDB G. Collard 52
Notes de cours – Projet de développement SGDB G. Collard 53
Notes de cours – Projet de développement SGDB G. Collard 54
Notes de cours – Projet de développement SGDB G. Collard 55
Notes de cours – Projet de développement SGDB G. Collard 56
Notes de cours – Projet de développement SGDB G. Collard 57
Notes de cours – Projet de développement SGDB G. Collard 58
Notes de cours – Projet de développement SGDB G. Collard 59
Notes de cours – Projet de développement SGDB G. Collard 60
Notes de cours – Projet de développement SGDB G. Collard 61
Notes de cours – Projet de développement SGDB G. Collard 62
Notes de cours – Projet de développement SGDB G. Collard 63
Notes de cours – Projet de développement SGDB G. Collard 64
Pour chaque fonctionnalité de notre application qui requiert un accès à la base de données, nous avons
ajouté une méthode à la classe AccesBD.
Fonctionnalités Méthode de la classe AccesBD
Obtenir la liste des catégories ListeCategorie(): List<Categorie>
Obtenir la liste des livres appartenant à une catégorie ListeLivreCategorie(categorie: Categorie): List<Livre>
Obtenir les informations sur les livres dont le titre et l’auteur
sont donnés
ListeLivreDisponible(titre: string, auteur: string):
List<Exemplaire>
ListeLivreEmprunte(titre: string, auteur: string):
List<Exemplaire>
Obtenir la liste des personnes ListePersonne():List<Personne>
Obtenir les informations sur une personne InfoPersonne(idPersonne: integer): Personne
Ajout d’une personne AjouterPersonne(personne: Personne): integer
Ajout d’une catégorie AjouterCategorie(categorie: Categorie):integer
Ajout d’un livre AjouterLivre(livre: Livre): integer
AjouterExemplaire(livre: Livre): integer
Supprimer un livre SupprimerLivre(exemplaire: Exemplaire): integer
Modifier les coordonnées d’une personne Faire en sorte qu’un livre est emprunté par une personne Faire en sorte qu’un livre est restitué Faire en sorte qu’un livre est prolongé ProlongerEmprunt(emprunt: Emprunt): integer
ModifierPersonne(personne: Personne): integer
AjouterEmprunt(emprunt: Emprunt): integer
RestituerLivre(emprunt: Emprunt): integer
En plus de ces fonctionnalités, nous avons ajouté les méthodes suivantes :
• ExisteCategorie(nom: string, souscategorie: string): integer
Vérifie si une catégorie existe
• ExistePersonne(idPersonne: integer): integer
Vérifie si une personne avec un identifiant donné existe déjà
• VerifyEmprunt(emprunt: Emprunt): integer
Vérifie si l’exemplaire existe, si exemplaire (n’)est (pas) emprunté, (pas) prolongé par
l’emprunteur
• GetLivreISBN(isbn: string): Livre
Recherche les informations sur un livre dont l’ISBN est donné
Notes de cours – Projet de développement SGDB G. Collard 65
Les lignes using Npgsql doit figurer au début de ce fichier pour rendre accessibles les classes d'accès
à une base de données PostgreSQL Server. Sans ces lignes, Visual C# indiquera ne pas connaitre les
classes NpgsqlConnection, NpgsqlCommand, ...
Pour utiliser la librairie Npgsql.dll, il ajouter la référence au projet. via l’application Stack Builder.
L’installation de ce pilote se fait
➢ Lancer Stack Builder
➢ Démarrer stack builder
➢ Choisir PosgreSQL puis cliquer sur next
Notes de cours – Projet de développement SGDB G. Collard 66
Executer ….exe
Notes de cours – Projet de développement SGDB G. Collard 67
Dans c# pour se connecter à la base de donnée
Cliquer sur le nom de projet – menu conceptuel – ajouter – référence
Notes de cours – Projet de développement SGDB G. Collard 68
Etablir une connexion avec la base de données
Pour accéder à une base de données, une application passe par le service postgreSQL server
Pour communiquer avec le service postgreSQL Server:
1. 2. L'application établit une connexion avec le service SQL Server.
Une fois cette connexion établie, l'application peut transmettre ses requêtes SQL au service
postgreSQL Server et recevoir en retour les informations de la base de données dont elle a
besoin.
3. Le code suivant montre comment établir une connexion avec la base de données Bibliotheque :
using Npgsql;
sqlConn.open() : établit la connexion avec le service postreSQL server.
Notes de cours – Projet de développement SGDB G. Collard 69
Requête SELECT
Constructeur de la classe NpgsqlCommand
• NpgsqlCommand() : initialise une nouvelle instance de la classe NpgsqlCommand
• NpgsqlCommand(string cmdText) : cmdText représente le texte de la requête
• NpgsqlCommand(string cmdText, NpgsqlConnection connection) :
cmdText représente le texte de la requête
connection : un objet NpgsqlConnection qui représente la connexion au postgreSQL server
NpgsqlDataReader sqlReader = sqlCmd.ExecuteReader() transmet à postgreSQL Server la requête et
fournit en retour un objet de type NpgsqlDataReader. C'est via cet objet qu'on va récupérer chaque
ligne de données.
Pour charger l'entièreté d'une ligne de données récupéré dans l’objet sqlreader, on utilise
sqlReader.Read(). Cette méthode retourne la valeur false quand il n'y a plus aucune ligne à lire, sinon
elle retourne true.
Pour obtenir la valeur d'une colonne de la ligne de données chargée, on utilise
sqlReader["nom de la colonne"].
Pour rendre compatible le type de données des colonnes des tables avec celui des valeurs en
C#, voici ce qu'on utilise :
integer (postgreSQL) varchar (postgreSQL) char (postgreSQL) real (postgreSQL) double (postgreSQL) numeric (postgreSQL) date (postgreSQL) → Convert.ToInt32() → int (c#)
→ Convert.ToString() → string (c#)
→ Convert.ToString() → string (c#)
→ Convert.ToDouble() → double (c#)
→ Convert.ToDouble() → double (c#)
→(decimal) Convert.ToDouble() → decimal (c#)
→ Convert.ToDateTime() → DateTime (c#)
….
La méthode sqlReader.Close() doit être appelée quand il n'y a plus aucune ligne à récupérer dans la
Si cette méthode n'est pas exécutée, plus aucune requête via la connexion courante
base de données. ne pourra être réalisée.
Notes de cours – Projet de développement SGDB G. Collard 70
Requête contenant un ou des paramètre(s)
Dans le texte de la requête, les paramètres sont précédés de :
Pour ajouter un paramètre et le type à une requête :
sqlCmd.Parameters.Add(new NpgsqlParameter("idCategorie",
NpgsqlTypes.NpgsqlDbType.Integer));
Pour preparer la requête:
sqlCmd.Prepare();
Pour ajouter les valeurs aux paramètres
sqlCmd.Parameters[0].Value = categorie.Idcategorie;
Notes de cours – Projet de développement SGDB G. Collard 71
Notes de cours – Projet de développement SGDB G. Collard 72
Commande INSERT – UPDATE – DELETE
La méthode ExecuteReader de la classe NpgsqlCommand est utilisée lors de l’exécution d’une
commande SELECT et retourne les résultats de la requête en tant qu’objet DataReader.
La méthode ExecuteNonQuery de la classe NpgsqlCommand est utilisée pour des requêtes qui ne
renvoient aucune donnée, tels que les commandes INSERT, DELETE, UPDATE. Cette méthode exécute
la commande et retourne le nombre de lignes affectées.
Notes de cours – Projet de développement SGDB G. Collard 73
Transaction
Pour créer une transaction, on appelle la méthode BeginTransaction() de la classe NpgsqlConnection
sqltr = this.SqlConn.BeginTransaction();
this.SqlConn représente une connexion à la base de données
Pour ajouter une commande postgreSQL à une transaction
sqlCmd1.Transaction = sqltr;
sqlCmd1 représente une instance de la classe pgsqlCommand
Dans la méthode SupprimerLivre(Exemplaire exemplaire) , si c’est le dernier exemplaire, on doit
également le supprimer dans la table livre. Ces deux commandes de suppression sont indivisibles,
pour cela on crée une transaction qui comprendra ces deux commandes.
Notes de cours – Projet de développement SGDB G. Collard 74
Notes de cours – Projet de développement SGDB G. Collard 75
Si dans la base de donnée un champ est à null, cette valeur ne peut pas être assigné à une variable
dans c#, pour savoir si le champ est null, utiliser la méthode IsDBNull de la classe NpgsqlDataReader
Notes de cours – Projet de développement SGDB G. Collard 76
L'exception ExceptionAccesBD
Quand l'objet de la classe AccesBD rencontre un problème lors de l'accès à la base de données
(impossible de créer une connexion, erreur pendant l'exécution d'une requête, ...), il déclenche une
exception de type ExceptionAccesBD.
Cette classe contient l'attribut Details qui mémorise des informations détaillées sur la nature de
l'erreur.
Notes de cours – Projet de développement SGDB G. Collard 77
Chapitre 9 : Création de la couche présentation
Les classes Program, Presentation et AccesConsole se trouvent dans l'espace de noms
couchePresentation.
Notes de cours – Projet de développement SGDB G. Collard 78
La classe Program
La méthode Main est automatiquement exécutée quand démarre l'exécution du programme. Cette
méthode crée un objet de type Presentation, puis elle donne le contrôle à la méthode MenuPrincipal
de cet objet.
La classe présentation
Notes de cours – Projet de développement SGDB G. Collard 79
Notes de cours – Projet de développement SGDB G. Collard 80
Notes de cours – Projet de développement SGDB G. Collard 81
Notes de cours – Projet de développement SGDB G. Collard 82
Notes de cours – Projet de développement SGDB G. Collard 83
Notes de cours – Projet de développement SGDB G. Collard 84
Notes de cours – Projet de développement SGDB G. Collard 85
Notes de cours – Projet de développement SGDB G. Collard 86
Notes de cours – Projet de développement SGDB G. Collard 87
Notes de cours – Projet de développement SGDB G. Collard 88
Notes de cours – Projet de développement SGDB G. Collard 89
Notes de cours – Projet de développement SGDB G. Collard 90
Notes de cours – Projet de développement SGDB G. Collard 91
Notes de cours – Projet de développement SGDB G. Collard 92
Classe AccesControl
Notes de cours – Projet de développement SGDB G. Collard 93
Notes de cours – Projet de développement SGDB G. Collard 94
Notes de cours – Projet de développement SGDB G. Collard 95





IPEFA Sup Seraing - Verviers
Projet de développement
SGDB
Notes de cours
Georgette Collard
2025-2026
Notes de cours G. Collard 1
Table des matières
Chapitre 10: PL/pgSQL ............................................................................................................................. 3
Commentaire ....................................................................................................................................... 4
Déclaration de variable ....................................................................................................................... 4
Paramètres de fonctions ..................................................................................................................... 5
Fonction déclarée avec des paramètres de sortie .............................................................................. 5
Copie de type ....................................................................................................................................... 6
Type ligne (%ROWTYPE) ...................................................................................................................... 6
Type record ......................................................................................................................................... 7
Plusieurs fonctions avec le même nom - polymorphisme .................................................................. 7
Instruction de base .............................................................................................................................. 7
Assignation ...................................................................................................................................... 7
Instruction IF.................................................................................................................................... 8
Instruction CASE .............................................................................................................................. 8
Boucle simple .................................................................................................................................. 8
SELECT … INTO et la variable FOUND ............................................................................................ 10
Fonction retournant des tables ..................................................................................................... 11
Gestion des erreurs ........................................................................................................................... 15
bloc de gestion des erreurs ........................................................................................................... 15
GET DIAGNOSTICS ............................................................................................................................. 16
Récupérer l’identifiant après une insertion ...................................................................................... 16
Génération d’une erreur ................................................................................................................... 17
Déclencheur....................................................................................................................................... 19
Exemples ....................................................................................................................................... 20
Chapitre 11 : Création des procédures stockées ................................................................................... 22
Chapitre 12 : modification de la couche accès aux données ................................................................ 32
Classe AccesBD .................................................................................................................................. 32
Notes de cours G. Collard 2
Chapitre 10: PL/pgSQL
Tout d’abord PostgreSQL ne fait pas de différence entre une fonction et une procédure car tout est
fonction dans PostgreSQL, même, dans une certaine mesure, pour les déclencheurs.
PL/pgSQL, signifie « Programming Language / postgreSQL » et sert justement à coder les routines
(UDF, procédures et trigger) dans le langage interne à PostGreSQL au même titre que Transact SQL sert
à MS SQL Server et PL/SQL à Oracle…
PL/pgSQL est un langage structuré en blocs. Le texte complet de la définition d'une fonction doit être
un bloc. Un bloc est défini comme
[ <<label>> ]
[ DECLARE
déclarations ]
BEGIN
instructions
END [ label ];
Exemple :
Exécution :
Notes de cours G. Collard 3
Chaque déclaration et chaque expression au sein du bloc est terminé par un point-virgule. Un bloc qui
apparaît à l'intérieur d'un autre bloc doit avoir un point-virgule après END ; néanmoins, le END final
qui conclut le corps d'une fonction n'a pas besoin de point-virgule.
Tous les mots clés et identifiants peuvent être écrits en majuscules et minuscules mélangées. Les
identifiants sont implicitement convertis en minuscule à moins d'être entourés de guillemets doubles
Commentaire
Double tiret (- -) : une ligne de commentaire
/* … */ : bloc de commentaire
Déclaration de variable
Toutes les variables utilisées dans un bloc doivent être déclarées dans la section déclaration du bloc.
La seule exception est que la variable de boucle d'une boucle FOR effectuant une itération sur des
valeurs entières est automatiquement déclarée comme variable entière (type integer)
La clause DEFAULT, si indiquée, spécifie la valeur initiale assignée à la variable quand on entre dans le
bloc. Si la clause DEFAULT n'est pas indiquée, la variable est initialisée à la valeur SQL NULL.
L'option CONSTANT empêche l'assignation de la variable, de sorte que sa valeur reste constante pour
la durée du bloc. Si NOT NULL est spécifié, l'assignement d'une valeur NULL aboutira à une erreur
d'exécution.
Exemples :
quantité integer DEFAULT 32;
url varchar := 'http://mysite.com';
id_utilisateur CONSTANT integer := 10;
Notes de cours G. Collard 4
Paramètres de fonctions
Pour donner un nom au paramètre, il existe deux façons :
Fonction déclarée avec des paramètres de sortie
Les paramètres en sortie sont utiles lors du retour de plusieurs valeurs.
L’exemple ci-dessous a le même résultat :
On crée un type composite anonyme pour le résultat de la fonction.
Notes de cours G. Collard 5
Copie de type
Exemple
id_utilisateur utilisateurs.id_utilisateur%TYPE;
On n’a pas besoin de connaître le type de données de id_utilisateur. Si le type de id_utilisateur change
dans le futur (integer en real par exemple), on n’a pas besoin de changer la définition de fonction.
%TYPE est particulièrement utile dans le cas de fonctions polymorphes puisque les types de données
nécessaires aux variables internes peuvent changer d'un appel à l'autre. Des variables appropriées
peuvent être créées en appliquant %TYPE aux arguments de la fonction ou à la variable fictive de
résultat.
Type ligne (%ROWTYPE)
Une variable de type composite est appelée variable ligne (ou variable row-type). Une telle variable
peut contenir une ligne entière de résultat de requête SELECT ou FOR, du moment que l'ensemble de
colonnes de la requête correspond au type déclaré de la variable. Les champs individuels de la valeur
row sont accessibles en utilisant la notation pointée, par exemple varligne.champ.
Notes de cours G. Collard 6
Type record
Les variables record sont similaires aux variables de type ligne mais n'ont pas de structure prédéfinie.
Elles empruntent la structure effective de type ligne de la ligne à laquelle elles sont affectées durant
une commande SELECT ou FOR. La sous-structure d'une variable record peut changer à chaque fois
qu'on l'affecte. Une conséquence de cela est qu'elle n'a pas de sous-structure jusqu'à ce qu'elle ait été
affectée, et toutes les tentatives pour accéder à un de ses champs entraînent une erreur d'exécution.
La structure réelle de la ligne n'est pas connue quand la fonction est écrite mais, dans le cas d'une
fonction renvoyant un type record, la structure réelle est déterminée quand la requête appelante est
analysée, alors qu'une variable record peut changer sa structure de ligne à la volée.
Plusieurs fonctions avec le même nom - polymorphisme
Instruction de base
Assignation
identifiant := expression;
Exemples :
id_utilisateur := 20;
tax := sous_total * 0.06;
Notes de cours G. Collard 7
Instruction IF
Instruction CASE
Boucle simple
Instruction loop
[<<label>>]
LOOP
instructions
END LOOP [ label ];
Notes de cours G. Collard 8
Instruction exit
EXIT [ label ] [ WHEN expression ];
Si aucun label n'est donné, la boucle la plus imbriquée se termine et l'instruction suivant END LOOP est
exécutée. Si un label est donné, ce doit être le label de la boucle, du bloc courant ou d'un niveau moins
imbriqué. La boucle ou le bloc nommé se termine alors et le contrôle continue avec l'instruction située
après le END de la boucle ou du bloc correspondant.
Si WHEN est spécifié, la sortie de boucle ne s'effectue que si expression vaut true. Sinon, le contrôle
passe à l'instruction suivant le EXIT.
Instruction continue
CONTINUE [ label ] [ WHEN expression ];
CONTINUE à l’intérieur de la boucle aura le même effet qu’en langage C, on retourne au début de la
boucle (itération suivante).
Instruction while
[<<label>>]
WHILE expression LOOP
instructions
END LOOP [ label ];
Notes de cours G. Collard 9
Instruction FOR
[<<label>>]
instructions
END LOOP [ label ];
FOR iterator IN [ REVERSE ] expression .. expression [bY expr ] LOOP
La variable iterator est automatiquement déclarée comme une variable de type INTEGER. Dans la
version avec une étiquette iterator peut-être préfixée par l’étiquette.
Exemples :
SELECT … INTO et la variable FOUND
On peut récupérer le résultat d’une requête SELECT grâce au SELECT … INTO …
La requête SELECT ne doit pas retourner plus qu’une ligne ou une valeur.
La variable destination est une variable ligne ou une liste de variable simple séparé par des virgules.
FOUND permet de vérifier si la requête a effectivement retourné une ligne ou non.
FOUND est une variable booléenne affectée par un SELECT INTO, et peut être immédiatement testée
après cette commande pour savoir si le select a retourné un résultat
Notes de cours G. Collard 10
Fonction retournant des tables
Il y a deux manière de déclarer qu’une fonction retourne une table, soit en utilisant
RETURNS SETOF sometype soit avec RETURNS TABLE(nom1 type1,...,nomn typen). La table de résultat
est construite avec les appels à RETURN QUERY et/ou RETURN NEXT
Returns setof
Une fonction qui retourne une table déclare la valeur de retour comme RETURNS SETOF sometype
Dans l’exemple suivant on suppose qu’il existe la table nommée LIVRE. Donc la fonction listerLivre()
retourne une table dont chaque ligne a exactement le même type qu’une ligne de la table LIVRE.
RETURNS TABLE
La fonction suivante retourne une table avec les attributs nomCategorie-souscatégorie (nom de la
catégorie et la sous-catégorie concaténée) et isbn, titre, auteur des livres disponibles.
Ajouter des éléments dans la table de résultats : RETURN QUERY et RETURN NEXT
Exemple : Retourner une table avec les attributs attributs nomCategorie-souscatégorie (nom de la
catégorie et la sous-catégorie concaténée) et isbn, titre, auteur des livres disponibles.
RETURN NEXT
Notes de cours G. Collard 11
RETURN QUERY
On peut faire la même chose sans boucle :
Ces 3 fonctions CategorieLivre, CategorieLivre _1, CategorieLivre_2 sont identiques.
Notes de cours G. Collard 12
Soit la fonction :
Les 4 commandes suivantes donnent le même résultat :
Les 3 fonctions suivantes sont identiques
Notes de cours G. Collard 13
Si ce livre existe, les 3 commandes ci-dessus donneront le même résultat
Par contre, si le livre n’existe pas, InfoLivre ne retourne aucune ligne, par contre InfoLivre0 et
InfoLivre1 retourne une ligne dont les valeurs des champs sont mis à null.
Notes de cours G. Collard 14
Gestion des erreurs
Le gestionnaire d’exception (bloc de code EXCEPTION) permet de récupérer la main sur le code après
une erreur.
La fonction RAISE permet de lancer une erreur ou un avertissement
bloc de gestion des erreurs
Le bloc de code EXCEPTION doit être situé en fin de corps avant le mot clé END.
EXCEPTION
WHEN <erreur11> [ OR <erreur12> [ OR ... ] ]
THEN
...
[ WHEN <erreur21> [ OR <erreur22> [ OR ... ] ]
THEN
... ]
[ ... ]
Où
<erreurN> ::=
{ erreur_SQL | SQLSTATE 'valeur' | OTHERS }
erreur_SQL SQLSTATE ‘valeur’ OTHERS Exemple
: est le code alphabétique d’une erreur
: permet d’indiquer une classe d’erreur
: permet de trapper toutes les erreurs.
Notes de cours G. Collard 15
Exécution :
Le 17/11/2019 est un dimanche, dans ce cas d vaut 0 et ne se trouve pas dans la définition du case.
GET DIAGNOSTICS
GET DIAGNOSTICS nbre = ROW_COUNT ;
nbre contiendra le nombre de lignes traitées par la dernière commande SQL exécutée.
Exemple
Cette fonction retourne le nombre de lignes insérées.
Récupérer l’identifiant après une insertion
Cette fonction retourne le nombre de ligne insérée et l’identifiant de la nouvelle personne.
Notes de cours G. Collard 16
Génération d’une erreur
Il est possible de générer une erreur à l’aide de la commande RAISE dont la syntaxe est la suivante :
RAISE [ { DEBUG | LOG | INFO | NOTICE | WARNING | EXCEPTION } ]
[ { 'message_erreur' [ <expressions> ] | erreur_SQL | SQLSTATE 'valeur' } ]
[ USING { MESSAGE | DETAIL | HINT | ERRCODE } = 'message' ];
DEBUG, LOG, INFO : enregistre l’erreur dans le journal PostGreSQL. Le code n’est pas affecté et aucun
gestionnaire d’erreur (bloc EXCEPTION) ne la prend en compte.
NOTICE, WARNING : enregistre l’erreur dans le journal PostGreSQL et l’envoie au client à titre
d’avertissement. Le code n’est pas affecté et aucun gestionnaire d’erreur (bloc EXCEPTION) ne la
prend en compte.
EXCEPTION : enregistre l’erreur dans le journal PostGreSQL, l’envoie au client à titre d’erreur, annule
automatiquement la transaction, interrompt le code et passe le contrôle au gestionnaire d’erreur
(bloc EXCEPTION) s’il y en a un.
EXCEPTION est la valeur par défaut dans la liste facultative suivant le mot clé RAISE.
La chaîne de caractères message_erreur peut contenir des caractères % qui seront remplacé par les
expressions à la position ordinale considérée.
Les journaux PostGreSQL sont situé dans l’arborescence d’installation du serveur dans
\PostgreSQL\[N° de version)\data\pg_log\
Notes de cours G. Collard 17
Notes de cours G. Collard 18
Déclencheur
Un déclencheur spécifie que la base de données doit exécuter automatiquement une fonction donnée
chaque fois qu'un certain type d'opération est exécuté. Les fonctions déclencheur peuvent être
attachées à une table, une vue.
La fonction déclencheur doit être définie avant que le déclencheur lui-même puisse être créé. La
fonction déclencheur doit être déclarée comme une fonction ne prenant aucun argument et
retournant un type trigger (la fonction déclencheur reçoit ses entrées via une
structure TriggerData passée spécifiquement, et non pas sous la forme d'arguments ordinaires de
fonctions).
Une fois qu'une fonction déclencheur est créée, le déclencheur (trigger) est créé
avec CREATE TRIGGER. Une même fonction déclencheur peut être utilisable par plusieurs
déclencheurs.
Voici la syntaxe de la création d’un déclencheur
1) D’abord, on spécifie le nom du déclencheur après les mots clés CREATE TRIGGER.
2) Ensuite, on spécifie le moment que le déclencheur se déclenche. Cela peut être BEFORE ou
AFTER avant ou après qu’un événement se produise.
3) Ensuite, on spécifie l’événement qui appelle le déclencheur. L’événement peut être INSERT,
DELETE, UPDATE
4) Ensuite, on spécifie le nom de la table associé au déclencheur après le mot-clé ON.
5) Ensuite, on spécifie le type de déclencheurs qui peut être :
- Déclencheur au niveau de la ligne qui est spécifié par la clause FOR EACH ROW.
- Déclencheur au niveau de l’instruction qui est spécifié par la clause FOR EACH
STATEMENT.
Quand une fonction PL/pgSQL est appelée en tant que trigger, plusieurs variables spéciales sont créées
automatiquement dans le bloc de plus haut niveau. Comme par exemple :
NEW
Type de données RECORD ; variable contenant la nouvelle ligne de base de données pour les
opérations INSERT / UPDATE dans les triggers de niveau ligne. Cette variable est non initialisée
dans un trigger de niveau instruction et pour les opérations DELETE.
OLD
Type de données RECORD ; variable contenant l'ancienne ligne de base de données pour les
opérations UPDATE/DELETE dans les triggers de niveau ligne. Cette variable est non initialisée
dans les triggers de niveau instruction et pour les opérations INSERT.
Notes de cours G. Collard 19
Exemples
Déclencheur de vérification
Création de la fonction déclencheur
Création du déclencheur
Notes de cours G. Collard 20
Exemple 2
Supposons que si le nom change ou le prénom change, on veut avoir une trace dans une table
personne_audit
Création de la fonction déclencheur
Création du déclencheur
Notes de cours G. Collard 21
Chapitre 11 : Création des procédures stockées
Reprenons le modèle d'application en 2 couches
1) 2) Couche présentation (presentation layer): cette couche gère les accès à la console (clavier et
écran) et effectue les traitements sur les données : tests, calculs, ... Dans notre application, il
va s'agir d'une classe appelée Presentation.
Couche accès aux données (data access layer): cette couche inclut toute classe d'accès à la
base de données. Dans notre application, il va s'agir de la classe AccesBD.
Voici une figure montrant ou se situe chaque couche:
Pendant l'exécution de l'application, les objets des classes Presentation et AccesBD s'échangent des
objets de classes directement construites à partir des tables de la base de données (Livre, Personne,
...). Ces classes sont appelées classes métier.
Grace à ce modèle en 2 couches, nous allons pouvoir modifier la couche accès aux données effectuant
les accès à la base de données sans avoir à ajouter, modifier ou supprimer une seule ligne de code
dans les autres classes.
Concrètement, dans cette nouvelle version de l'application Bibliotheque, nous allons transférer toutes
les requêtes SQL depuis les méthodes de la classe AccesBD vers des procédures stockées.
Notes de cours G. Collard 22
Voici a quoi maintenant ressemble l'application:
Dans cette architecture, la couche accès aux données a été divisée en 2 parties:
1) 2) La classe AccesBD a pour rôle d'appeler les procédures stockées qui sont mémorisées dans la
base de données Bibliotheque.
Les procédures stockées accèdent aux tables de la base de données.
Rien ne va changer dans l'application par rapport à la version précédente, a l'exception du contenu du
fichier AccesBD.cs se trouvant dans l'espace de noms coucheAccesBD.
On va ajouter les procédures stockées suivantes dans la base de données :
Notes de cours G. Collard 23
Notes de cours G. Collard 24
Notes de cours G. Collard 25
Notes de cours G. Collard 26
Notes de cours G. Collard 27
Notes de cours G. Collard 28
Notes de cours G. Collard 29
Notes de cours G. Collard 30
Notes de cours G. Collard 31
Chapitre 12 : modification de la couche accès aux données
Nous avons vu que la couche accès aux données de l'application est à présent constituée de 2 parties:
la classe AccesBD et les procédures stockées.
• La classe AccesBD a pour rôle d'appeler les procédures stockées qui sont mémorisées dans la base
de données Bibliotheque.
• Les procédures stockées accèdent aux tables de la base de données.
Classe AccesBD
Vous ne trouverez plus aucune requête SQL dans les méthodes dans la classe AccesBD, car les requêtes
SQL se trouvent dans les procédures stockées :
Notes de cours G. Collard 32
Notes de cours G. Collard 33
Notes de cours G. Collard 34
Notes de cours G. Collard 35
Notes de cours G. Collard 36
Notes de cours G. Collard 37
Notes de cours G. Collard 38
Notes de cours G. Collard 39
Notes de cours G. Collard 40
Notes de cours G. Collard 41
Notes de cours G. Collard 42
Notes de cours G. Collard 43
Notes de cours G. Collard 44
Notes de cours G. Collard 45
Notes de cours G. Collard 46
Notes de cours G. Collard 47
Notes de cours G. Collard 48
Notes de cours G. Collard 49
Notes de cours G. Collard 50
Notes de cours G. Collard 51
Notes de cours G. Collard 52


IPEFA Sup Seraing - Verviers
Projet de développement
SGDB
Notes de cours
WPF
Georgette Collard
2025-2026
04_notesDeCours – projet SGDB G. Collard 1
Table des matières
Application WPF ...................................................................................................................................... 5
Composition d’un projet WPF ............................................................................................................. 6
APP.xaml .......................................................................................................................................... 8
MVVM ..................................................................................................................................................... 9
Exemple ............................................................................................................................................... 9
Explication relative à MVVM ......................................................................................................... 13
MVVM et les événements ................................................................................................................. 13
XAML (eXentensible Application Markup Language) ............................................................................ 17
Compilateur XAML ............................................................................................................................ 17
Syntaxe de base en XAML ................................................................................................................. 17
Code XAML vs C# ............................................................................................................................... 18
Définition de cet exemple en XAML .............................................................................................. 19
Définition de cet exemple en C# ................................................................................................... 19
Classe de référence ........................................................................................................................... 19
Espaces de noms ............................................................................................................................... 20
XAML et contrôles utilisateur ................................................................................................................ 20
Propriétés de positionnement des contrôles .................................................................................... 21
Alignement .................................................................................................................................... 21
Marges ........................................................................................................................................... 22
Propriété Padding .......................................................................................................................... 22
Fenêtre et contrôles de disposition ...................................................................................................... 24
Contrôle de grille ............................................................................................................................... 24
Grille et dimension des cellules ..................................................................................................... 26
Contrôle de type Panel ...................................................................................................................... 28
StackPanel ..................................................................................................................................... 28
DockPanel ...................................................................................................................................... 29
WrapPanel ..................................................................................................................................... 33
Autres contrôles de dispositions. ...................................................................................................... 35
Contrôle Canvas ............................................................................................................................. 35
Contrôle ViewBox .......................................................................................................................... 36
Contrôle ScrollViewer .................................................................................................................... 37
Border ............................................................................................................................................ 38
Contrôle ItemControl .................................................................................................................... 38
Principaux contrôles .............................................................................................................................. 41
Contrôles d’affichages ....................................................................................................................... 41
04_notesDeCours – projet SGDB G. Collard 2
TextBox .......................................................................................................................................... 41
Label .............................................................................................................................................. 44
Image ............................................................................................................................................. 45
StatusBar et ToolTip ...................................................................................................................... 45
Contrôle d’édition ............................................................................................................................. 46
Contrôle de sélection de données ..................................................................................................... 49
ComboBox ..................................................................................................................................... 49
CheckBox et RadioButton .............................................................................................................. 50
Sélection dans des objets complexes ............................................................................................ 50
Introduction aux Data Binding .......................................................................................................... 56
Exemple 1 : Syntaxe d’un Binding ................................................................................................. 56
Exemple 2 : Utilisation du contexte de données (DataContexte) ................................................. 57
Exemple 3 ...................................................................................................................................... 58
Exemple 4 ...................................................................................................................................... 59
Exemple 5 : .................................................................................................................................... 61
Exemple 6 : .................................................................................................................................... 63
Exemple 7 : .................................................................................................................................... 64
Lien de commandes........................................................................................................................... 65
Les commandes pré-définies ......................................................................................................... 65
Sélection de dates ......................................................................................................................... 68
Contrôles d’action utilisateur ........................................................................................................ 70
Fenêtrage .......................................................................................................................................... 72
Window ......................................................................................................................................... 72
NavigationWindow ........................................................................................................................ 72
DataBinding (liaison de données) .......................................................................................................... 74
Binding côté vue exclusivement ........................................................................................................ 74
Propriété Source ............................................................................................................................ 74
Propriété RelativeSource ............................................................................................................... 75
Propriété ElementName ................................................................................................................ 76
Binding entre vue et vue-modèle ...................................................................................................... 77
Présentation de l’objet de Binding ................................................................................................ 77
Propriété Mode de l’objet de binding ........................................................................................... 77
Propriété UpdateSourceTrigger de l’objet de binding .................................................................. 77
Exemple Binding défini en C# (entre vue-modèle et vue) ............................................................. 78
Même exemple défini en XAML .................................................................................................... 80
Binding de collections ........................................................................................................................ 82
04_notesDeCours – projet SGDB G. Collard 3
Binding avec ObservableCollection<T> ......................................................................................... 82
Binding avec DataView .................................................................................................................. 87
Binding avec DataView .................................................................................................................. 88
Binding de collection et ComboBox .............................................................................................. 92
04_notesDeCours – projet SGDB G. Collard 4
Chapitre 13 : WPF (Windows Presentation Foundation)
WPF est le successeur de la technologie Windows Forms et permet de créer des applications fenêtrées
de type « client lourd ». Les applications WPF sont des applications de type « client lourd ». Elles
s’exécutent directement depuis le système d’exploitation.
WPF est une bibliothèque permettant de réaliser des applications graphiques. Ces applications sont
dites événementielles car elles réagissent à des événements (clic sur un bouton, redimensionnement
de la fenêtre, saisie de texte, etc.)
WPF se base sur un paradigme MVVM (Modèle-vue-vue-modèle) ce qui permet une grande flexibilité
grâce à la séparation entre données, traitements et présentation. Il permet à un designer de travailler
spécifiquement sur une présentation sans se préoccuper des données à afficher qui est de la
responsabilité du développeur. La liaison entre la présentation et les données se fait via un procédé
nommé Binding ou DataBinding.
Application WPF
Création d’une application WPF dans Visual Studio :
04_notesDeCours – projet SGDB G. Collard 5
Composition d’un projet WPF
Une application WPF est construite grâce à deux langages. Un langage de présentation qui va
permettre de décrire le contenu de notre fenêtre : le XAML (prononcez xamelle) et du C# qui va
permettre de faire tout le code métier.
Une application WPF est décomposée en fenêtres qui sont affichées à partir de l’application principale.
Chaque fenêtre est composée d’un fichier .xaml qui contient la description de la fenêtre
(l’emplacement des boutons, des images, …) et d’un fichier de code .xaml.cs qui contient le code
associé à cette fenêtre. On appelle ce fichier le « code behind ».
Lorsqu’on crée un projet, Visual C# Express créé automatiquement une fenêtre par défaut qui
s’appelle MainWindow. Ce fichier est d’ailleurs ouvert automatiquement. Si nous déplions ce fichier
dans l’explorateur de solutions, nous pouvons voir un autre fichier dessous qui possède l’extension
.xaml.cs, le fichier de code behind.
Le XAML est un fichier XML qui va nous permettre de décrire le contenu de notre fenêtre. Cette fenêtre
est composée de « contrôles », qui sont des éléments graphiques unitaires comme un bouton, une
case à cocher, etc. Nous pouvons soit remplir ce fichier XAML à la main si nous connaissons la syntaxe,
soit utiliser le designer pour y glisser les contrôles disponibles dans la boite à outils.
1) 2) 3) le rendu de notre fenêtre dans la partie haute du fichier MainWindows.xaml. C’est ça qui
sera affiché lors du lancement de notre application.
le descriptif de cette fenêtre dans le code XAML, c'est-à-dire le langage permettant de
décrire la fenêtre. Ce XAML est modifié si nous ajoutons des contrôles avec le designer.
Inversement, si nous modifions le code XAML, le designer se met à jour avec les
modifications.
Nous pouvons voir et modifier les propriétés du contrôle dans la fenêtre de propriétés.
04_notesDeCours – projet SGDB G. Collard 6
Exemple
➢ Faire glisser le Button de la boîte à outils vers la fenêtre.
➢ Cliquer sur le bouton, cliquer droit et choisir Modifier le texte.
04_notesDeCours – projet SGDB G. Collard 7
➢ Encoder Cliquez-moi
APP.xaml
App.xaml est le point de départ des déclarations de l’application. Il est créé automatiquement par
Visual Studio, de même que le fichier code-behind App.xaml.cs. Cette classe démarre les instructions
et ensuite démarre la fenêtre ou la page désirée. C’est dans cette classe, qu’on souscrit à des
événements d’application, tels que le démarrage de l’application, les exceptions gérées, … C’est dans
cette classe, qu’on définit des ressources globales pouvant être accessibles depuis l’ensemble de
l’application par exemple des styles globaux.
Quand nous créons une application, le fichier App.xaml est automatiquement généré et a cette
structure comme ci-dessous :
04_notesDeCours – projet SGDB G. Collard 8
La propriété StartupUri définit la fenêtre ou la page à démarrer lorsque l’application est lancée. Dans
ce cas-ci, MainWindow.xaml sera lancé.
MVVM
La technologie WPF est directement basée sur les préceptes d’une architecture nommée
Modèle-Vue-Vue (MVVM – Model View ViewModel). Le principe est la séparation des données
(le modèle) de l’interface utilisateur (la vue ou la présentation) et dont les interactions sont gérées par
une couche intermédiaire, la vue-modèle.
MVVM implique la présence de 3 grandes entités :
- L’interface graphique (la vue)
- L’accès aux données (modèle)
- Une couche interactive (le modèle-vue )
Exemple
Soit une interface utilisateur permettant de mettre à jour une valeur numérique.
Le projet doit comprendre 3 couches : vue, vue-modèle et modèle. Ces trois couches doivent
respectivement correspondre à la présentation (vue), à la gestion des données (vue-modèle) et l’accès
aux données (modèle). Idéalement, la valeur numérique doit être synchronisée entre la vue et la vue-
modèle, voire avec le modèle.
Arborescence du projet :
04_notesDeCours – projet SGDB G. Collard 9
- Accès aux données : c’est une simple classe modèle.cs
- Couche vue-modèle : c’est une classe Vue-Modèle.cs qui implémente l’interface
INotifyPropertyChanged. Cette interface est le dispositif qui permet de notifier à la vue-modèle
un changement de valeur. Cette interface est au cœur du mécanisme de Binding.
- Couche vue : MainWindow.xaml. Cette fenêtre reçoit comme contexte d’exécution la
vue-modèle.
La présentation contient les contrôles suivants :
- Une TextBox contenant la valeur synchronisée avec la vue-modèle.
- Une CheckBox qui si elle est cochée, indique que non seulement la valeur numérique est
synchronisée avec la vue-modèle, mais également avec le modèle.
Modèle.cs
04_notesDeCours – projet SGDB G. Collard 10
Vue-Modèle.cs
04_notesDeCours – projet SGDB G. Collard 11
MainWindow.xaml
MainWindow.xaml.cs
04_notesDeCours – projet SGDB G. Collard 12
Explication relative à MVVM
L’alimentation de la TextBox se fait uniquement par l’utilisation de
Text="{Binding MaValeur}
C’est-à-dire que l’alimentation de la Textbox depuis la vue-modèle et l’envoi de la valeur de la TextBox
vers la vue-modèle se réalisent uniquement avec ce morceau de code. Cette approche permet de
laisser à un graphiste la responsabilité du design XML de la vue tandis que le développeur se chargera
du fonctionnel. L’utilisation de l‘interface INotifyPropertyChanged permet de gérer cette séparation
entre présentation et gestion des données.
- La vue-modèle « connaît » le modèle : en effet, c’est la vue-modèle qui l’instancie.
- La vue « connaît » la vue-modèle : en effet, c’est la vue qui l’instancie et qui l’envisage comme
son contexte de données.
Chaque couche ne connaît que ce dont elle a besoin.
- Le modèle ne connaît pas son utilisation.
- La vue-modèle ne connaît pas son contexte d’utilisation, c’est-à-dire qu’elle est indépendante
des vues qui l’utiliseront.
De même, WPF offre un mécanisme basé sur les commandes permettant de gérer les événements tout
en respectant la séparation entre couches MVVM.
MVVM et les événements
Soit une fenêtre comprenant deux boutons : Bouton_1 et Bouton_2 ainsi qu’une zone de texte non
éditable, une TextBox nommée Resultat. Bouton_1 déclenche l’affichage d’un message dans la zone
de texte et utilise une gestion d’événements classique. Bouton_2 utilise une commande et affiche
également un message dans la zone de texte.
04_notesDeCours – projet SGDB G. Collard 13
MainWindow.xaml
MainWindow.xaml.cs
bouton_1 a un événement Click défini auquel est associé la méthode Bouton_1_Click dont le travail
est de modifier le contenu de la zone de texte Resultat.
Le Bouton_2 modifie le contenu de la zone de text Resultat, mais d’une manière différente, propre à
WPF et respectant les préceptes de MVVM qui suggère la séparation stricte des fonctions, y compris
quand il s’agit de gestion d’événements.
04_notesDeCours – projet SGDB G. Collard 14
Vue_modèle.cs
04_notesDeCours – projet SGDB G. Collard 15
RelaisCommande.cs
La zone de texte Resultat est « bindée » avec l’attribut message de la vue-modèle qui a été définie
comme le DataContext de la vue grâce à cette ligne :
this.DataContext = new Vue_Modele();
Le bouton Bouton_2 voit l’action Click « bindée » avec une commande grâce à cet attribut dans le code
XAML :
Command="{Binding Commande}"
04_notesDeCours – projet SGDB G. Collard 16
XAML (eXentensible Application Markup Language)
Dans un projet WPF minimal, il y a au moins deux fichiers d’extension .xaml : celui de l’application elle-
même et celui de la description et de définition de la fenêtre principale.
Le code XAML est basé sur le langage de balisage XML. Chaque élément d’une interface, une vue, un
bouton, un tableau ou tout autre contrôle graphique usuel, peut se définir en XAML.
Compilateur XAML
On retrouve du XAML pour le développement mobile phone, pour Silverlight, pour le développement
d’application accessibles dans le Windows Store voire à du développement du type Xamarin.
Le compilateur XAML permet de transformer du code XAML en une sorte de code interne qu’un
compilateur C# saura interpréter.
Syntaxe de base en XAML
Le langage XAML permet d’instancier des objets .Net. En effet, le XAML est un langage balisé dont la
syntaxe est qualifiée d’ « élément-propriété ». En effet, à une balise (un élément), on associe une
collection de propriétés.
Exemple : L’élément Button possède les propriétés Foreground, Background, Width, Height, Margin
et Content.
04_notesDeCours – projet SGDB G. Collard 17
Le bouton a son fond coloré en blanc, son écriture en rouge, ses dimensions et une marge sont définies
et contient le texte « Cliquez ! »
Le code suivant correspond au précédent. La propriété Content est écrite dans une balise dédiée et
non comme attribut de la balise de l’élément.
Chaque élément peut avoir une propriété par défaut en XAML. La propriété par défaut de l’élément
Button est Content. Le code suivant est identique aux précédentes. La propriété Content n’est pas
précisée, elle est implicite.
Code XAML vs C#
Button.
L’exemple suivant ajoute un contrôle StackPanel comprenant un contrôle TextBox et un contrôle
04_notesDeCours – projet SGDB G. Collard 18
Définition de cet exemple en XAML
Définition de cet exemple en C#
Classe de référence
Dans le code XAML, qui correspond à une fenêtre (Window), il est nécessaire de préciser la classe
correspondance. Par défaut, lors de la génération d’un projet WPF, cette classe est automatiquement
créée et renseignée. L’élément de syntaxe XML utilisé est x :class en attribut de la balise window.
<Window x:Class="MaClasse"
04_notesDeCours – projet SGDB G. Collard 19
Espaces de noms
En développement C#, il est fréquent d’utiliser le mot-clé using pour référencer des espaces de noms
comprenant des composants ou des éléments logiciels que le code utilise. En XAML, on fait de même.
xmlns:PREFIXE="clr-namespace:NOM_ESPACE_DE_NOMS"
- PREFIXE nomme localement l’espace de nom
- NOM_ESPACE_DE_NOMS référence l’espace de noms lui-même.
Par défaut, lors de la création d’un nouveau projet, ces espaces de noms sont créés :
<Window x:Class="MonEspaceDeNom.MainWindow"
xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
xmlns:local="clr- MonEspaceDeNom"
XAML et contrôles utilisateur
Les contrôles possèdent un certain nombre de propriétés communes comme par exemple la largeur
(Width), la hauteur (Height), le nom (Name), la couleur de fond (Background). Car, ils héritent de la
classe Control.
Concernant les dimensions, Height et Width peuvent prendre les valeurs suivantes :
- Une valeur numérique exprimée par défaut en pixels
- Le mot-clé Auto qui signifie que le contrôle courant va occuper toute la place possible dans les
limites du contrôle qui le contient.
Par exemple, le code XAML :
permet l’affichage de cette fenêtre :
04_notesDeCours – projet SGDB G. Collard 20
- Le conteneur du bouton est un StackPanel de largeur 200 pixels (Width="200") qui par défaut
est centré.
- La hauteur du bouton est de 200 pixels car explicitement indiquée comme telle (Height="200")
- La largeur du bouton prend la valeur Auto, c’est-à-dire que le bouton occupe en largeur tout
l’espace qui lui et potentiellement octroyé par son conteneur, en l’occurrence, les 200 pixels
du StackPanel.
Propriétés de positionnement des contrôles
Alignement
HorizontalAlignement peut prendre les valeurs Left, Right, Center, Strech.
VerticalAlignement peut prendre les valeurs Top, Bottom, Center ou Strech.
Les propriétés Left, Right, Top, Bottom permettent de « coller » le contrôle sur l’un des 4 bords tandis
que Center permet un alignement centré verticalement et/ou horizontalement.
La propriété Stretch indique au contrôle d’occuper le maximum d’espace possible au sein de son
conteneur dans la direction indiquée (verticale ou horizontal).
<Window x:Class="WPF_App1.MainWindow"
xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
xmlns:local="clr-namespace:WPF_App1"
mc:Ignorable="d"
Title="MainWindow" Height="160" Width="300">
<StackPanel Orientation="Horizontal">
<Button VerticalAlignment="Top">Button 1</Button>
<Button VerticalAlignment="Center">Button 2</Button>
<Button VerticalAlignment="Bottom">Button 3</Button>
<Button VerticalAlignment="Bottom">Button 4</Button>
<Button VerticalAlignment="Center">Button 5</Button>
<Button VerticalAlignment="Top">Button 6</Button>
</StackPanel>
</Window>
<StackPanel Orientation="Vertical">
<Button HorizontalAlignment="Left">Button 1</Button>
<Button HorizontalAlignment="Center">Button 2</Button>
<Button HorizontalAlignment="Right">Button 3</Button>
<Button HorizontalAlignment="Right">Button 4</Button>
<Button HorizontalAlignment="Center">Button 5</Button>
<Button HorizontalAlignment="Left">Button 6</Button>
</StackPanel>
04_notesDeCours – projet SGDB G. Collard 21
Marges
Les marges permettent de spécifier une mesure sur chacune des 4 directions (gauche, haut, droite,
bas) entre le bord du conteneur et le bord du contrôle courant.
On utilise la propriété Margin.
Ce code place une marge de 5 à gauche, 10 en haut, 15 à droite et 20 en bas. (L’ordre est important,
rotation dans le sens des aiguilles d’une montre à partir de la gauche)
Margin="5,10,15,20"
Ce code est équivalent à Margin="8,8,8,8"
Margin="8"
Propriété Padding
Cette propriété permet de définir une zone à l’intérieur du contrôle dans laquelle le contenu ne pourra
pas s’afficher.
En quelque sorte, le Padding est l’équivalent de Margin pour l’intérieur du contrôle là où les marges
gèrent l’espace à l’extérieur du contrôle.
Syntaxiquement, elle reprend le même principe que Margin.
04_notesDeCours – projet SGDB G. Collard 22
Exemple
Ce code correspond à
Si la valeur du Padding est mise à 15, on a le résultat suivant : « Cliquez-ici » est tronqué.
04_notesDeCours – projet SGDB G. Collard 23
Fenêtre et contrôles de disposition
Les contrôles de type conteneurs ou contrôles de disposition héritent de la classe Control qui possède
une propriété Content. L’existence éventuelle de cette propriété Content permet d’envisager le
contrôle en question comme un conteneur, c’est-à-dire un contrôle pouvant contenir d’autres
contrôles.
Contrôle de grille
Le contrôle Grid consiste en une grille de lignes et de colonnes. Par défaut, si aucune propriété n’est
indiquée, la grille ne comporte qu’une seule cellule. Mais son utilisation permet un quadrillage de
l’espace.
Pour ce faire, le contrôle Grid fournit deux propriétés de type collection :
- RowDefinitions
- ColumnDefinitions
Ces deux collections contiennent respectivement des objets de type RowDefinitions et
ColumnDefinitions. L’idée est de construire une matrice définie par une collection de lignes et une
collection de colonnes.
Exemple : ce code définit la grille :
04_notesDeCours – projet SGDB G. Collard 24
À ce stade, seule la définition de la grille est réalisée. Il reste encore à l’utiliser et à insérer des contrôles
et des informations dans les cellules de la grille.
Pour cela, on utilise les propriétés Grid.row et Grid.Column qui permettent de référencer une cellule
grâce à un système de coordonnées :
Grid.Row="0" et Grid.Column="4" correspond à la cellule située à la première ligne et à la cinquième
colonne.
Exemple : Damier de trois cases sur trois cases colorées en noir et blanc alternativement. Les cases
blanches sont numérotées.
<Window x:Class="DAMIERGRIL.MainWindow"
xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
xmlns:local="clr-namespace:CH3_SOURCE1"
mc:Ignorable="d"
Title="Gril - exemple 2" Height="333" Width="310">
<Grid ShowGridLines="True">
<Grid.RowDefinitions>
<RowDefinition Height="100" />
<RowDefinition Height="100" />
<RowDefinition Height="*" />
</Grid.RowDefinitions>
<Grid.ColumnDefinitions>
<ColumnDefinition Width="100" />
<ColumnDefinition Width="100" />
<ColumnDefinition Width="*" />
</Grid.ColumnDefinitions>
<Grid Grid.Row="0" Grid.Column="0" Background="Black" />
<Grid Grid.Row="0" Grid.Column="2" Background="Black" />
<Grid Grid.Row="1" Grid.Column="1" Background="Black" />
<Grid Grid.Row="2" Grid.Column="0" Background="Black" />
<Grid Grid.Row="2" Grid.Column="2" Background="Black" />
<StackPanel Grid.Row="0"
Grid.Column="1"
HorizontalAlignment="Center"
VerticalAlignment="Center">
<TextBlock Text="1" FontSize="25"/>
</StackPanel>
<StackPanel Grid.Row="0"
Grid.Column="1"
HorizontalAlignment="Center"
VerticalAlignment="Center">
<TextBlock Text="1" FontSize="25"/>
</StackPanel>
<StackPanel Grid.Row="1"
Grid.Column="0"
HorizontalAlignment="Center"
VerticalAlignment="Center">
<TextBlock Text="2" FontSize="25"/>
</StackPanel>
<StackPanel Grid.Row="1"
Grid.Column="2"
04_notesDeCours – projet SGDB G. Collard 25
HorizontalAlignment="Center"
VerticalAlignment="Center">
<TextBlock Text="3" FontSize="25"/>
</StackPanel>
<StackPanel Grid.Row="2"
Grid.Column="1"
HorizontalAlignment="Center"
VerticalAlignment="Center">
<TextBlock Text="4" FontSize="25"/>
</StackPanel>
</Grid>
</Window>
La grille peut rester visible à l’exécution. Pour cela, il suffit d’ajouter la propriété ShowGridLines à la
valeur True.
Grille et dimension des cellules
Width ou Height peuvent prendre les valeurs suivantes :
- Une valeur numérique Height="100". La hauteur sera de 100 pixels.
- "Auto" Cette valeur signifie que la taille de la colonne ou de la ligne concernée s’adapte au
contenu de ladite colonne ou ligne. S’il n’y a pas de contenu, la largeur ou la longueur
concernée sera nulle.
- * : Cette valeur permet de spécifier une proportion de l’espace restante. Ainsi, si l’espace
restante (après prise en compte des valeurs numériques et des valeurs "Auto") voit trois
colonnes avec ces largeurs "*", "3*", "4*", la première colonne occupera 1/8 de l’espace
restante, la seconde 3/8 et la dernière, la moitié.
04_notesDeCours – projet SGDB G. Collard 26
04_notesDeCours – projet SGDB G. Collard 27
Par défaut, chaque contrôle occupe une case ou cellule. Pour qu’un contrôle occupe plus d’une ligne
ou d’une colonne, on utilise respectivement la propriété RowSpan ou ColumnSpan.
<Grid>
<Grid.ColumnDefinitions>
<ColumnDefinition Width="1*" />
<ColumnDefinition Width="1*" />
</Grid.ColumnDefinitions>
<Grid.RowDefinitions>
<RowDefinition Height="*" />
<RowDefinition Height="*" />
</Grid.RowDefinitions>
<Button>Button 1</Button>
<Button Grid.Column="1">Button 2</Button>
<Button Grid.Row="1" Grid.ColumnSpan="2">Button 3</Button>
</Grid>
Contrôle de type Panel
Outre, la grille détaillée précédemment, les différents contrôles de type Panel sont les principaux
contrôles qui participent à l’organisation d’une fenêtre WPF. En particulier, ils permettent de
structurer l’espace de la fenêtre et de dédier un emplacement de la fenêtre à une fonctionnalité
donnée. Ainsi, le développeur peut par exemple structurer sa fenêtre en consacrant une partie de
l’espace à un formulaire de recherche et aux critères de recherche, et une autre partie de l’espace à
l’affichage des résultats. En WPF, on distingue trois types de Panel : le StackPanel, le DockPanel, le
WrapPanel.
StackPanel
Ce contrôle permet de disposer de façon horizontale ou verticale plusieurs composants visuels. Sa
principale propriété est donc Orientation qui prend comme valeur soit Horizontal ou Vertical (valeur
par défaut).
Exemple : formulaire de recherche défini à l’aide d’un StackPanel orienté horizontalement.
Résultat :
04_notesDeCours – projet SGDB G. Collard 28
DockPanel
Le DockPanel se distingue par la gestion des contrôles selon un système d’ancres. L’idée est de fixer
un composant inclus dans le DockPanel sur son bord haut, bas, gauche ou droit via la propriété Dock.
Exemple :
Résultat :
Résultat :
04_notesDeCours – projet SGDB G. Collard 29
<Window x:Class="WPF_App1.MainWindow"
xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
xmlns:local="clr-namespace:WPF_App1"
mc:Ignorable="d"
Title="MainWindow" Height="200" Width="200">
<DockPanel>
<Button DockPanel.Dock="Left">Left</Button>
<Button DockPanel.Dock="Top">Top</Button>
<Button DockPanel.Dock="Right">Right</Button>
<Button DockPanel.Dock="Bottom">Bottom</Button>
<Button>Center</Button>
</DockPanel>
</Window>
<DockPanel>
<Button DockPanel.Dock="Top" Height="50">Top</Button>
<Button DockPanel.Dock="Bottom" Height="50">Bottom</Button>
<Button DockPanel.Dock="Left" Width="50">Left</Button>
<Button DockPanel.Dock="Right" Width="50">Right</Button>
<Button>Center</Button>
</DockPanel>
04_notesDeCours – projet SGDB G. Collard 30
Propriété LastChildFill
La propriété LastFill permet d’indiquer si le dernier contrôle instancié va occuper (valeur True) ou non
(valeur False), l’espace restant au sein du DockPanel.
Exemple : les composants sur le bord haut et on donne une hauteur déterminé au control DockPanel.
Résultat
04_notesDeCours – projet SGDB G. Collard 31
Exemple : LastChildFill mis à False
Résultat
04_notesDeCours – projet SGDB G. Collard 32
WrapPanel
Le WrapPanel permet dans le cas d’une orientation horizontale de gérer la largeur de contrôle avec
une approche par ligne. Si l’espace n’est pas suffisant sur la ligne courante, le WrapPanel affecte le
composant à instancier à la ligne suivante. De même, si l’orientation est verticale, le WrapPanel évalue
l’espace disponible dans la colonne courante.
Exemple : orientation horizontal
Résultat :
04_notesDeCours – projet SGDB G. Collard 33
Le CheckBox et le Button sont donc instancié sur la seconde ligne.
Exemple : orientation verticale
Résultat :
Lorsqu’il s’agit d’instancier le bouton Recherche, la colonne courante n’a plus assez d’espace
disponible. Le bouton est alors instancié sur la seconde colonne.
04_notesDeCours – projet SGDB G. Collard 34
Autres contrôles de dispositions.
Contrôle Canvas
Le contrôle Canvas permet de définir un contenu de manière absolu. On spécifie les positionnements
absolus des éléments contenus. On précise, par exemple, que tel contrôle est situé à x pixels du bord
gauche et à y pixels du bord haut.
04_notesDeCours – projet SGDB G. Collard 35
Contrôle ViewBox
Le contrôle ViewBox permet l’étirement maximal des éléments contenus s’adaptant ainsi à l’espace
disponible (transformation homothétique).
Exemple :
Résultat :
En cas d’agrandissement de la fenêtre (Width="650"):
04_notesDeCours – projet SGDB G. Collard 36
Plusieurs modalités d’étirement graphique existent.
- Fill : le contenu est étiré en fonction de la fenêtre et non en fonction des proportions de
départ.
- None : pas de transformation.
- Uniform : (valeur par défaut) le contenu est étiré en fonction des dimensions de la fenêtre
mais conserve les proportions de départ).
- UniformToFill : le contenu s’ajuste au mieux à la destination sans distorsion (il conserve ses
proportions de départ). Par contre, le contenu peut être dérouté pour s’adapter au mieux à
la destination.
Contrôle ScrollViewer
Un contrôle de type ScrollViewer permet d’associer aux éléments contenus dans la barre de
défilement. Certains contrôles possèdent leurs propres propriétés à même d’afficher des barres de
défilement mais ce n’est pas toujours le cas. Ce contrôle permet de « scroller » n’importe quel
contenu.
<Window x:Class="CH3_SCROLL.MainWindow"
xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
xmlns:local="clr-namespace:CH3_SCROLL"
mc:Ignorable="d"
Title="MainWindow" Height="150" Width="150">
<Grid>
<ScrollViewer Height="100"
VerticalAlignment="Top"
ScrollViewer.HorizontalScrollBarVisibility="Auto"
ScrollViewer.VerticalScrollBarVisibility="Visible">
<TextBlock Width="200" Height="150" TextWrapping="Wrap"
Text="Le développement d'un logiciel nécessite
l'intervention de différentes personnes : architecte, testeur, développeur,
administrateur de base de données, designer, … La méthodologie MVVM apporte une
séparation entre le code métier et sa représentation graphique, la production d'un
code qui est testable, une organisation qui facilite la communication, et qui optimise
le travail entre développeurs et designers, une maintenance et une réutilisation
aisée." />
</ScrollViewer>
</Grid>
</Window>
Résultat
04_notesDeCours – projet SGDB G. Collard 37
Les propriétés ScrollViewer.HorizontalScrollBarVisibility et
ScrollViewer.VerticalScrollBarVisibility peuvent prendre les valeurs suivantes :
- Auto : affichage quand le scrolling est nécessaire.
- Disabled : permet de voir les barres de défilement qui ne seront jamais actives
- Hidden : permet de masquer les barres de défilement.
- Visible : rend toujours les barres de défilement visibles, quel que soit le besoin du scrolling.
Border
Un contrôle de type Border permet de munir les éléments contenus de bordures qui les entourent. En
effet, la bordure d’un contrôle n’est pas une propriété du contrôle lui-même. Il s’agit donc d’inclure
le ou les contrôle(s) dans un contrôle de type bordure.
Exemple :
Contrôle ItemControl
Le contrôle ItemsControl permet d’afficher une collection d’entités selon un conteneur donné. Par
défaut, le conteneur implicite est un StackPanel d’orientation verticale.
04_notesDeCours – projet SGDB G. Collard 38
Exemple :
On peut remarquer que la balise system:String est directement utilisée dans ce code, ce qui est rendu
possible par l’ajout de cet espace de noms
xmlns:system="clr-namespace:System;assembly=mscorlib"
Les trois chaînes de caractères sont alignées verticalement car le conteneur est un StackPanel orienté
verticalement.
Pour chaque élément de collection, on peut définir les contrôles qui seront utilisés. Pour ce faire, on
utilise ItemsControl.ItemTemplate et DataTemplate.
04_notesDeCours – projet SGDB G. Collard 39
Exemple : template d’élément de collection est un TextBox entouré d’une bordure noire.
04_notesDeCours – projet SGDB G. Collard 40
Principaux contrôles
Contrôles d’affichages
TextBox
Ce contrôle permet l’affichage d’un texte.
<TextBox Text="Ceci est un texte" />
La propriété Text étant la propriété par défaut, on peut écrire la ligne précédente de cette manière :
<TextBox>Ceci est un texte</TextBox>
TextTrimming
Cette propriété permet de mettre en page le contenu du contrôle. Il permet de tronquer le texte en
cas de manque de place en indiquant l’existence de texte complémentaire en affichant trois petits
points (…).
TextTrimming peut prendre les valeurs suivantes :
- WordEllipsis : tronque au niveau du dernier mot
- CharacterEllipsis : tronque au niveau du dernier caractère
- None
Exemple :
04_notesDeCours – projet SGDB G. Collard 41
Résultat :
TextWrapping
Cette propriété permet d’optimiser l’affichage du texte en optimisant l’espace disponible. Il peut
prendre les valeurs suivantes :
- Wrap
- WrapWithOverflow
- NoWrap
04_notesDeCours – projet SGDB G. Collard 42
Résultat
04_notesDeCours – projet SGDB G. Collard 43
Autres aspect de la mise en forme d’un TextBlock
Voici les principales balises de mises en forme :
- LineBreak : permet un passage à la ligne
- Bold : permet de mettre en gras une partie du texte
- Italic : permet de mettre en italique une partie du texte
- Hyperlink : permet d’insérer un lien hypertexte.
Résultat :
Label
de disposition.
Le label possède un contenu (propriété Content), un label peut inclure des contrôles, tel un contrôle
04_notesDeCours – projet SGDB G. Collard 44
Image
L’affichage dans les principaux formats (.png, .jpg, ..) utilisés se fait à l’aide de la balise Image. La
propriété spécifiant l’image à afficher est Source. La valeur de cette dernière peut être un chemin
physique sur la machine cliente ou une URI.
StatusBar et ToolTip
StatusBar permet d’afficher une barre de statut située en bas de la fenêtre.
ToolTip permet l’affichage détaillé de données via des infobulles.
Résultat
04_notesDeCours – projet SGDB G. Collard 45
Contrôle d’édition
TextBox
Dans une fenêtre de type formulaire, il est fréquent d’avoir une TextBox (zone de texte) permettant
d’entrer des données.
PasswordBox
est une zone de texte dédiée à un mot de passe.
La propriété PasswordChar : caractère utilisé pour masquer le mot de passe.
Exemple : Affichage d’un formulaire de connexion
Résultat :
04_notesDeCours – projet SGDB G. Collard 46
RichTextBox
Ce contrôle permet d’éditer du texte au format RTF
Exemple :
MainWindow.xaml
MainWindow.xaml.cs
04_notesDeCours – projet SGDB G. Collard 47
Au démarrage :
➢ Sélectionner puis cliquer sur afficher la sélection
Résultat
04_notesDeCours – projet SGDB G. Collard 48
Contrôle de sélection de données
ComboBox
Ce contrôle permet la sélection d’une seule valeur dans une liste de données.
ComboBox dérive du contrôle ItemsControl. ComboBoxItem dérive de ITemsControlItem.
Exemple
04_notesDeCours – projet SGDB G. Collard 49
CheckBox et RadioButton
Ces contrôles permettent de sélectionner ou non une valeur. La grande différence entre les deux
réside dans le fait que RadioButton aoute une exclusivité entre différents choix.
Exemple
Résultat:
Sélection dans des objets complexes
ListBox
Au niveau de la sélection, elle peut être unique ou multiple selon la valeur de la propriété
SelectionMode qui peut prendre les valeurs suivantes :
- Single : permet une sélection unique. L’élément sélectionné est accessible dans SelectedItem
- Multiple : permet la solution multiple sans recours au bouton [shift]. Les éléments
séléctionnées sont accessibles dans SelectedItems.
- Extended : permet la solution multiple avec recours au bouton [shift]. Les éléments
séléctionnées sont accessibles dans SelectedItems.
04_notesDeCours – projet SGDB G. Collard 50
Exemple :
<Window x:Class="WPF_App1.MainWindow"
xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
xmlns:local="clr-namespace:WPF_App1"
mc:Ignorable="d"
Title="ListBox" Height="250" Width="300">
<ListBox Height="250" Width="300" SelectionMode="Multiple">
<ListBoxItem>
<StackPanel Orientation="Horizontal">
<TextBlock Width="100" Margin="10"
FontWeight="Bold">Catégorie</TextBlock>
<TextBlock Width="100" Margin="10"
FontWeight="Bold">Sous-Catégorie</TextBlock>
</StackPanel>
</ListBoxItem>
<ListBoxItem>
<StackPanel Orientation="Horizontal">
<TextBlock Width="100" Margin="10">Roman</TextBlock>
<TextBlock Width="100" Margin="10">policier</TextBlock>
</StackPanel>
</ListBoxItem>
<ListBoxItem>
<StackPanel Orientation="Horizontal">
<TextBlock Width="100" Margin="10">Roman</TextBlock>
<TextBlock Width="100" Margin="10">historique</TextBlock>
</StackPanel>
</ListBoxItem>
<ListBoxItem>
<StackPanel Orientation="Horizontal">
<TextBlock Width="100" Margin="10">Poésie</TextBlock>
<TextBlock Width="100" Margin="10"></TextBlock>
</StackPanel>
</ListBoxItem>
<ListBoxItem>
<StackPanel Orientation="Horizontal">
<TextBlock Width="100" Margin="10">Bande dessinée</TextBlock>
<TextBlock Width="100" Margin="10">humoristique</TextBlock>
</StackPanel>
</ListBoxItem>
</ListBox>
</Window>
Résultat :
04_notesDeCours – projet SGDB G. Collard 51
ListView et GridView
Le contrôle ListView dérive du contrôle ItemsControl. ListView possède une propriété View qui
correspond aux données affichées. En général, View prend comme valeur GridView.
Exemple : ListView dont View est une GridView
Fichier.xaml
<Window x:Class="CH4_GRIDVIEW.MainWindow"
xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
xmlns:local="clr-namespace:CH4_GRIDVIEW"
mc:Ignorable="d"
Title="CH4 - ListView - GridView" Height="200" Width="400">
<ListView Margin="10" Name="ListViewLivre">
<ListView.View>
<GridView>
<GridViewColumn Header="Auteur" Width="140"
DisplayMemberBinding="{Binding Auteur}" />
<GridViewColumn Header="Titre" Width="260"
DisplayMemberBinding="{Binding Titre}" />
</GridView>
</ListView.View>
</ListView>
</Window>
04_notesDeCours – projet SGDB G. Collard 52
Fichier.xaml.cs
using System.Collections.Generic;
using System.Windows;
namespace CH4_GRIDVIEW
{
public class Livre
{
public string Auteur { get; set;}
public string Titre { get; set; }
};
{
public partial class MainWindow : Window
public MainWindow()
{
InitializeComponent();
List<Livre> livres = new List<Livre>();
livres.Add(new Livre() { Auteur = "Jules Verne",
Titre = "Le tour du monde en 80 jours"});
livres.Add(new Livre() { Auteur = "Georges Simenon",
Titre = "Inspecteur Maigret" });
ListViewLivre.ItemsSource = livres;
}
}
}
Résultat :
04_notesDeCours – projet SGDB G. Collard 53
TreeView
Ce contrôle permet la sélection au sein d’un arbre de données. La propriété TreeViewItem permet de
définir les entités de l’arbre.
Exemple :
<Window x:Class="CH4_TREEVIEW.MainWindow"
xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
xmlns:local="clr-namespace:CH4_TREEVIEW"
mc:Ignorable="d"
Title="CH4 - TreeView" Height="250" Width="300">
<TreeView>
<TreeViewItem Header="Bibliothèque" IsExpanded="True">
<TreeViewItem Header="Gestion des personnes" />
<TreeViewItem Header="Gestion des livres" IsExpanded="True">
<TreeViewItem Header="Gestion des exemplaires" IsExpanded="True">
<TreeViewItem Header="Exemplaires emprintés" />
<TreeViewItem Header="Exemplaires disponibles" />
</TreeViewItem>
</TreeViewItem>
</TreeViewItem>
</TreeView>
</Window>
Résultat :
04_notesDeCours – projet SGDB G. Collard 54
04_notesDeCours – projet SGDB G. Collard 55
Introduction aux Data Binding
La liaison des données est une technique générale qui lie les deux sources de données/informations
et assure la synchronisation des données entre elles.
➢ Créez un nouveau projet WFP et nommer ce projet WPF_App1
Exemple 1 : Syntaxe d’un Binding
➢ Copiez ceci dans le fichier MainWindow.xaml
<Window x:Class="WPF_App1.MainWindow"
xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
xmlns:local="clr-namespace:WPF_App1"
mc:Ignorable="d"
Title="MainWindow" Height="110" Width="280">
<StackPanel Margin="10">
<TextBox Name="txtValue" />
<WrapPanel Margin="0,10">
<TextBlock Text="Value: " FontWeight="Bold" />
<TextBlock Text="{Binding Path=Text, ElementName=txtValue}" />
</WrapPanel>
</StackPanel>
</Window>
➢ Exécutez
➢ Tapez du texte dans le textBox. Que remarquez-vous ?
Le texte de la TextBlock est synchronisé avec le texte tapé dans le premier TextBlock
Texte saisi
<TextBlock Text="{Binding Path=Text, ElementName=txtValue}" />
Les accolades servent à encapsuler une extension de balisage en XAML. Pour lier (bind) des données,
on utilise l’extension Binding qui permet de décrire un Binding pour la propriété Text.
Le Path indique vers quelle propriété on effectue le binding. Comme il s’agit de la propriété par défaut,
on n’a pas besoin de le préciser, dans ce cas-ci on aurait pu écrire :
<TextBlock Text="{Binding Text, ElementName=txtValue}" />
Un Binding possède d’autres propriétés, dans l’exemple ci-dessus, on utilise la propriété
ElementName. Cette propriété permet de se connecter directement à un autre élément de l’interface
utilisateur(UI). Les propriétés assignées dans le Binding sont séparées par une virgule.
04_notesDeCours – projet SGDB G. Collard 56
Exemple 2 : Utilisation du contexte de données (DataContexte)
➢ Remplacez le fichier MainWindows.xaml par ceci
<Window x:Class="WPF_App1.MainWindow"
xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
xmlns:local="clr-namespace:WPF_App1"
mc:Ignorable="d"
Title="MainWindow" Height="210" Width="280">
<StackPanel Margin="15">
<WrapPanel>
<TextBlock Text="Window title: " />
<TextBox Text="{Binding Title, UpdateSourceTrigger=PropertyChanged}"
Width="150" />
</WrapPanel>
<WrapPanel Margin="0,10,0,0">
<TextBlock Text="Window dimensions: " />
<TextBox Text="{Binding Width}" Width="50" />
<TextBlock Text=" x " />
<TextBox Text="{Binding Height}" Width="50" />
</WrapPanel>
</StackPanel>
</Window>
➢ Modifiez le fichier MainWindows.xaml.cs pour obtenir ceci :
namespace WPF_App1
{
/// <summary>
/// Logique d'interaction pour MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
public MainWindow()
{
InitializeComponent();
this.DataContext = this;
}
}
}
➢ Exécutez
➢ Entrez un autre titre dans le TextBox Window Title. Que remarquez-vous ?
Le titre de la fenêtre est mise à jour
➢ Modifiez la première dimension ? Que remarquez-vous ?
La fenêtre est redimensionnée
➢ Modifiez la deuxième dimension ? Que remarquez-vous ?
La fenêtre est redimensionnée
04_notesDeCours – projet SGDB G. Collard 57
Au démarrage, la propriété DataContext est égale à null. Un DataContext est passé par héritage à tous
les contrôles enfants.
L’instruction this.DataContext = this ; contexte de données. Width et Height.
indique à la classe Window qu’il soit lui-même son propre
Ici, nous avons utilisé plusieurs propriétés de la classe Window, tels que Title,
Cette utilisation du DataContxt permet de définir un contexte global à la fenêtre WPF. En aucun cas
cela n’oblige à faire se référer chaque composant à ce contexte. Un composant peut très bien avoir
son propre contexte ou temporairement ne pas utiliser le contexte global.
Un autre aspect fondamental relatif au contexte de données est l’héritage. En effet, si le contexte de
données est associé à une fenêtre par exemple (Window), ce dernier sera partagé par tous les
composants et sous-composants compris dans la fenêtre. Le contexte de données est donc associé à
un élément et à toute son arborescence-fille.
Exemple 3
Remplacez le fichier MainWindow.xaml par
<Window x:Class="WPF_App1.MainWindow"
xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
xmlns:local="clr-namespace:WPF_App1"
mc:Ignorable="d"
Title="MainWindow" Height="210" Width="280">
<StackPanel Margin="10">
<TextBox Name="txtValue" />
<WrapPanel Margin="0,10">
<TextBlock Text="Value: " FontWeight="Bold" />
<TextBlock Name="lblValue" />
</WrapPanel>
</StackPanel>
</Window>
➢ Modifiez le fichier MainWindows.xaml.cs pour obtenir ceci :
namespace WPF_App1
{
/// <summary>
/// Logique d'interaction pour MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
public MainWindow()
{
InitializeComponent();
Binding binding = new Binding("Text");
binding.Source = txtValue;
lblValue.SetBinding(TextBlock.TextProperty, binding);
})
}}
04_notesDeCours – projet SGDB G. Collard 58
➢ Exécutez
➢ Tapez du texte dans le TextBox. Que remarquez-vous ?
Dans cet exemple, nous créons une instance de liaison. Nous spécifions le chemin voulu directement
dans le constructeur (Binding binding = new Binding("Text"); ) , en l’occurrence "Text", puisque nous
voulons le lier à la propriété Text. Nous spécifions ensuite une Source, dans cet exemple, la source
est le contrôle TextBox (binding.Source = txtValue; ). Maintenant, WPF sait qu’il doit utiliser la TextBox
comme source de contrôle, et que nous recherchons spécifiquement la valeur contenue dans sa
propriété Text. Nous utilisons la méthode SetBinding pour combiner notre objet de liaison avec le
contrôle de destination/cible : TextBlock (lblvalue). La méthode SetBinding() prend deux paramètres,
le premier indiquant la propriété de dépendance à laquelle on veut faire la liaison et le second
contenant l’objet de liaison (lblValue.SetBinding(TextBlock.TextProperty, binding); ).
Exemple 4
➢ Remplacez le fichier MainWindow.xaml par
<Window x:Class="WPF_App1.MainWindow"
xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
xmlns:local="clr-namespace:WPF_App1"
mc:Ignorable="d"
Title="MainWindow" Height="130" Width="310">
<StackPanel Margin="15">
<WrapPanel>
<TextBlock Text="Window title: " />
<TextBox Name="txtWindowTitle" Text="{Binding Title,
UpdateSourceTrigger=Explicit}" Width="150" />
<Button Name="btnUpdateSource" Click="btnUpdateSource_Click" Margin="5,0"
Padding="5,0">*</Button>
</WrapPanel>
<WrapPanel Margin="0,10,0,0">
<TextBlock Text="Window dimensions: " />
<TextBox Text="{Binding Width, UpdateSourceTrigger=LostFocus}" Width="50"
/>
<TextBlock Text=" x " />
<TextBox Text="{Binding Height, UpdateSourceTrigger=PropertyChanged}"
Width="50" />
</WrapPanel>
</StackPanel>
</Window>
04_notesDeCours – projet SGDB G. Collard 59
➢ Remplacez le fichier MainWindow.xaml.cs par
namespace WPF_App1
{
/// <summary>
/// Logique d'interaction pour MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
public MainWindow()
{
InitializeComponent();
this.DataContext = this;
}
private void btnUpdateSource_Click(object sender, RoutedEventArgs e)
{
BindingExpression binding =
txtWindowTitle.GetBindingExpression(TextBox.TextProperty);
binding.UpdateSource();
}
}
}
➢ Exécutez
➢ Changez le titre dans la TextBox. Le titre a-t-il changé ? Non
➢ Cliquez sur le bouton Que remarquez-vous ? Le titre de la fenêtre a changé
➢ Changez la première valeur de la dimension de la fenêtre. Que remarquez-vous ?
La fenêtre est redimensionnée dès qu’on quitte ce textBox
➢ Changez la deuxième valeur de la dimension de la fenêtre. Que remarquez-vous ?
La fenêtre est redimensionnée dès qu’on change de valeur.
La valeur par défaut de UpdateSourceTrigger est « Default ». Les autres options sont
PropertyChanged, LostFocus et Explicit.
La première est définie comme Explicit, ce qui veut dire que la source ne sera pas mise à jour à
moins qu’on le fasse manuellement. Dans cet exemple, on a ajouté un bouton qui va mettre à
jour la source sur demande (dès qu’on clique dessus). Dans le Code-behing, on a le handler d
l’événement Click qui récupère le contrôle de destination pour ensuite appeler la méthode
UpdateSource() dessus.
La seconde TextBox utilise la valeur LostFocus, qui est en fait la valeur par défaut pour un binding
d’une propriété Text. Cela signifie que la valeur source sera mise à jour à chaque fois que le
contrôle destination perd le focus.
Le troisième TextBox utilise la valeur PropertyChanged, qui signifie que la valeur source sera mise
à jour chaque fois que la propriété liée change, ce qui se produit dès que le texte change.
04_notesDeCours – projet SGDB G. Collard 60
Exemple 5 :
➢ Remplacez le fichier MainWindow.xaml par
<Window x:Class="WPF_App1.MainWindow"
xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
xmlns:local="clr-namespace:WPF_App1"
mc:Ignorable="d"
Title="MainWindow" Height="150" Width="300">
<DockPanel Margin="10">
<StackPanel DockPanel.Dock="Right" Margin="10,0,0,0">
<Button Name="btnAjoutAuteur" Click="btnAjoutAuteur_Click">Ajouter
auteur</Button>
<Button Name="btnModificationAuteur" Click="btnModificationAuteur_Click"
Margin="0,5">
Modifier auteur</Button>
<Button Name="btnSuppressionAuteur" Click="btnSuppressionAuteur_Click">
Supprimer auteur</Button>
</StackPanel>
<ListBox Name="listeAuteurs" DisplayMemberPath="Nom"></ListBox>
</DockPanel>
</Window>
04_notesDeCours – projet SGDB G. Collard 61
➢ Changer le fichier MainWindows.xaml.cs pour obtenir ceci :
public partial class MainWindow : Window
{
private List<Auteur> auteurs = new List<Auteur>();
public MainWindow()
{
InitializeComponent();
auteurs.Add(new Auteur() { Nom = "Jules Verne" });
auteurs.Add(new Auteur() { Nom = "Georges Simenon" });
listeAuteurs.ItemsSource = auteurs;
private void btnAjoutAuteur_Click(object sender, RoutedEventArgs e)
auteurs.Add(new Auteur() { Nom = "Nouveau auteur" });
}
{
}
{
private void btnModificationAuteur_Click(object sender, RoutedEventArgs e)
if (listeAuteurs.SelectedItem != null)
(listeAuteurs.SelectedItem as Auteur).Nom = "Random Name";
}
{
private void btnSuppressionAuteur_Click(object sender, RoutedEventArgs e)
if (listeAuteurs.SelectedItem != null)
auteurs.Remove(listeAuteurs.SelectedItem as Auteur);
}
public class Auteur
public string Nom { get; set; }
}
{
}
}
➢ Exécutez
➢ Savez-vous ajouter un auteur, modifier un auteur, supprimer un auteur ? Non
La première étape est de faire en sorte que l’interface utilisateur réponde aux changements de source
à l’intérieur de la liste (ItemsSource), comme quand on ajoute ou supprime un élément dans la liste.
WPF fournit un type de liste qui prévient n’importe quelle destination des changements de son
contenu. Elle s’appelle ObservableCollection et elle s’utilise de façon similaire à une List<T>.
Dans l’exemple ci-dessous, on a simplement remplacé List<Auteur> par ObservableCollection<Auteur>
04_notesDeCours – projet SGDB G. Collard 62
Exemple 6 :
➢ Changer le fichier MainWindows.xaml.cs pour obtenir ceci :
namespace WPF_App1
{
/// <summary>
/// Logique d'interaction pour MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
private ObservableCollection<Auteur> auteurs = new
ObservableCollection<Auteur>();
public MainWindow()
{
InitializeComponent();
auteurs.Add(new Auteur() { Nom = "Jules Verne" });
auteurs.Add(new Auteur() { Nom = "Georges Simenon" });
listeAuteurs.ItemsSource = auteurs;
private void btnAjoutAuteur_Click(object sender, RoutedEventArgs e)
auteurs.Add(new Auteur() { Nom = "Nouveau auteur" });
}
{
}
{
private void btnModificationAuteur_Click(object sender, RoutedEventArgs e)
if (listeAuteurs.SelectedItem != null)
(listeAuteurs.SelectedItem as Auteur).Nom = "Random Name";
}
{
private void btnSuppressionAuteur_Click(object sender, RoutedEventArgs e)
if (listeAuteurs.SelectedItem != null)
auteurs.Remove(listeAuteurs.SelectedItem as Auteur);
}
public class Auteur
public string Nom { get; set; }
}
{
}
}
➢ Exécutez
➢ Savez-vous ajouter un auteur, modifier un auteur, supprimer un auteur ?
ajouter, supprimer : Oui
modifier : non
04_notesDeCours – projet SGDB G. Collard 63
Exemple 7 :
➢ Changer le fichier MainWindows.xaml.cs pour obtenir ceci :
namespace WPF_App1
{
/// <summary>
/// Logique d'interaction pour MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
private ObservableCollection<Auteur> auteurs = new
ObservableCollection<Auteur>();
public MainWindow()
{
InitializeComponent();
auteurs.Add(new Auteur() { Nom = "Jules Verne" });
auteurs.Add(new Auteur() { Nom = "Georges Simenon" });
listeAuteurs.ItemsSource = auteurs;
private void btnAjoutAuteur_Click(object sender, RoutedEventArgs e)
auteurs.Add(new Auteur() { Nom = "Nouveau auteur" });
private void btnModificationAuteur_Click(object sender, RoutedEventArgs e)
{
if (listeAuteurs.SelectedItem != null)
(listeAuteurs.SelectedItem as Auteur).Nom = "Random Name";
}
private void btnSuppressionAuteur_Click(object sender, RoutedEventArgs e)
{
if (listeAuteurs.SelectedItem != null)
auteurs.Remove(listeAuteurs.SelectedItem as Auteur);
}
}
}
{
}
public class Auteur : INotifyPropertyChanged
{
private string nom;
public string Nom {
get { return this.nom; }
set
{
if (this.nom != value)
{
this.nom = value;
this.NotifyPropertyChanged("Nom");
}
}
}
public event PropertyChangedEventHandler PropertyChanged;
public void NotifyPropertyChanged(string propName)
{
if (this.PropertyChanged != null)
this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
} }}
04_notesDeCours – projet SGDB G. Collard 64
➢ Exécutez
➢ Savez-vous ajouter un auteur, modifier un auteur, supprimer un auteur ? Oui
L’événement PropertyChanged de type PropertyChangedEventHandler se déclenche relativement au
nom de la propriété qui change alors de sa valeur.
Lien de commandes
Les commandes ne font en fait rien par elles-mêmes. A la base, elles consistent en une interface
ICommand, qui définit seulement un événement et deux méthodes : Execute() et CanExecute. La
première réalise effectivement l'action quand la seconde détermine si cette action est disponible. Pour
exécuter l'action associée à la commande vous avez besoin d'un lien entre cette commande et votre
code et c'est là que le CommandBinding entre en jeu.
Un CommandBinding est en général défini dans une Window ou un UserControl et contient d'une part
la référence à la commande dont il dépend ainsi que les eventHandlers associés aux événements
Execute() et CanExecute() de la commande.
Les commandes pré-définies
On peut évidemment implémenter ses propres commandes, mais WPF a défini plus de 100
commandes, les plus communément employées, qu’on peut utiliser directement. Elles sont réparties
en 5 catégories nommé ApplicationCommands, NavigationCommands, MediaCommands,
EditingCommands et ComponentCommands. ApplicationCommands en particulier contient les
commandes pour un grand nombre d'actions fréquemment utilisées telles que Nouveau, Ouvrir,
Sauvegarder, Couper, Copier et Coller.
Exemple 8 : utiliser les commandes WPF
➢ Changer le fichier MainWindows.xaml.cs pour obtenir ceci :
<Window x:Class="WPF_App1.MainWindow"
xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
xmlns:local="clr-namespace:WPF_App1"
mc:Ignorable="d"
Title="MainWindow" Height="100" Width="200">
<Window.CommandBindings>
<CommandBinding Command="ApplicationCommands.New"
Executed="NewCommand_Executed" CanExecute="NewCommand_CanExecute" />
</Window.CommandBindings>
<StackPanel HorizontalAlignment="Center" VerticalAlignment="Center">
<Button Command="ApplicationCommands.New">New</Button>
</StackPanel>
</Window>
04_notesDeCours – projet SGDB G. Collard 65
➢ Changer le fichier MainWindows.xaml.cs pour obtenir ceci :
namespace WPF_App1
{
/// <summary>
/// Logique d'interaction pour MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
public MainWindow()
InitializeComponent();
private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
e.CanExecute = true;
private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
MessageBox.Show("The New command was invoked");
{
}
{
}
{
}
}
}
➢ Exécutez
➢ Tester Que remarquez-vous ?
Dans cet exemple, nous avons défini un CommandBinding dans la Window en l’ajoutant dans sa
collection CommandBindings (<Window.CommandBindings>). Nous avons précisé la commande à
utiliser (New dans ApplicationCommands) et ses deux eventHandlers
(<CommandBinding Command="ApplicationCommands.New" Executed="NewCommand_Executed"
CanExecute="NewCommand_CanExecute" /> ).
L’interface visuelle se compose d’un unique bouton auquel, nous associons la commande à l’aide de
la propriété Command (<Button Command="ApplicationCommands.New">New</Button> ).
Dans le Code-behind, nous associons les deux événements à leur handler. Le handler CanExecute, que
WPF appellera quand l'application est libre pour regarder si la commande est disponible, est très
simple dans cet exemple car nous souhaitons que cette commande soit toujours disponible. Cela est
précisé en affectant la valeur True à la propriété CanExecute de l'argument de l'événement.
Le handler Executed se contente d'afficher un texte dans une MessageBox quand la commande est
appelée. Lorsque nous exécutons cet exemple et cliquons sur le bouton nous voyons ce message.
04_notesDeCours – projet SGDB G. Collard 66
Exemple 9 : Utiliser la méthode CanExecute
Dans l’exemple 8, nous avons implémenté un bouton qui est toujours accessible (CanExecute retourne
True). Dans de nombreux cas, on souhaite que le bouton soit activé ou désactivé suivant un certain
statut défini dans notre application. L’exemple est la bascule de boutons pour utiliser le presse-papier
de Window. Les boutons Couper et Copier sont activés uniquement lorsqu’un texte est sélectionné et
le bouton Coller est activé lorsqu’un texte est présent dans le presse-papier.
<Window x:Class="WPF_App1.MainWindow"
xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
xmlns:local="clr-namespace:WPF_App1"
mc:Ignorable="d"
Title="MainWindow" Height="200" Width="250">
<Window.CommandBindings>
<CommandBinding Command="ApplicationCommands.Cut"
CanExecute="CutCommand_CanExecute" Executed="CutCommand_Executed" />
<CommandBinding Command="ApplicationCommands.Paste"
CanExecute="PasteCommand_CanExecute" Executed="PasteCommand_Executed" />
</Window.CommandBindings>
<DockPanel>
<WrapPanel DockPanel.Dock="Top" Margin="3">
<Button Command="ApplicationCommands.Cut" Width="60">_Cut</Button>
<Button Command="ApplicationCommands.Paste" Width="60"
Margin="3,0">_Paste</Button>
</WrapPanel>
<TextBox AcceptsReturn="True" Name="txtEditor" />
</DockPanel>
</Window>
namespace WPF_App1
{
/// <summary>
/// Logique d'interaction pour MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
public MainWindow()
{
InitializeComponent();
}
private void CutCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
{
e.CanExecute = (txtEditor != null) && (txtEditor.SelectionLength > 0);
}
private void CutCommand_Executed(object sender, ExecutedRoutedEventArgs e)
{
txtEditor.Cut();
}
private void PasteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
{
e.CanExecute = Clipboard.ContainsText();
}
private void PasteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
{
txtEditor.Paste();
} } }
04_notesDeCours – projet SGDB G. Collard 67
Sélection de dates
Fichier.xaml
Fichier.xaml.cs
04_notesDeCours – projet SGDB G. Collard 68
Au démarrage
➢ Selection d’une date
Après sélection d’une date (10 janvier 2020)
04_notesDeCours – projet SGDB G. Collard 69
Contrôles d’action utilisateur
Les principaux contrôles d’action sont
- Le menu classique
- Un bouton ouvrant une boîte de message sur li clic gauche
- Un menu contextuel sur le clic droit
Le menu ou le ContextMenu se base sur des propriétés MenuItem pour décrire les menus.
Fichier.xaml
<Window x:Class="CH4_ACTION.MainWindow"
xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
xmlns:local="clr-namespace:CH4_ACTION"
mc:Ignorable="d"
Title="CH4 - actions" Height="150" Width="250">
<StackPanel Orientation="Vertical">
<Menu>
<MenuItem Header="Fichier">
<MenuItem Header="Nouveau..." />
<Separator />
<MenuItem Header="Ouvrir..." />
<Separator />
<MenuItem Header="Sauver..." />
<Separator />
<MenuItem Header="Sortir" />
</MenuItem>
</Menu>
<Button Margin="25" Width="120" HorizontalAlignment="Center"
Click="Button_Click"
Content="Ouvrir Message">
<Button.ContextMenu>
<ContextMenu>
<MenuItem Header="Compilation" />
</ContextMenu>
</Button.ContextMenu>
</Button>
</StackPanel>
</Window>
04_notesDeCours – projet SGDB G. Collard 70
Fichier.xaml.cs
➢ Clique sur ouvrir message la deuxième fenêtre apparaît
04_notesDeCours – projet SGDB G. Collard 71
Fenêtrage
Window
Propriétés
- Title: permet de nommer la fenêtre. Elle s’affiche en haut de la fenêtre dans la barre de titre.
- WindowState : trois valeurs : Maximized, Minimized, Normal
- ResizeMode: définir les modes de redimensionnement de la fenêtre, les valeurs possibles sont
CanMinimize, CanResize, CanResizeWithGrip, NoResize.
- ShowInTaskbar : valeur True ou false : indicateur si la fenêtre fait l’objet dans la barre des
tâches Wndows.
- WindowsStartupLocation : emplacement d’affichage de la fenêtre. Les valeurs possibles sont
CenterScreen, Manual, CenterOwner.
Exemple
Lancement depuis l’application WPF
La fenêtre Window est en général lancée depuis l’application, les modalités du lancement étant
définies, notamment le nom de la fenêtre à lancer, dans le fichier App.xaml. Dans ce dernier, une
balise Application possède une propriété StartupUri qui contient le nom de la fenêtre à lancer :
NavigationWindow
Le contrôle NavigationWindow est une alternative au contrôle Window. Il possède la spécificité
d’avoir un comportement de navigateur web, gérant un historique de navigation et envisageant
chaque fichier XAML affichée comme une page web : pour cela, il faut que le fichier XAML soit défini
grâce à une balise Page et non Window.
Exemple : une NavigationWindow dont la source est la Page Maison.xaml
04_notesDeCours – projet SGDB G. Collard 72
NavigationWindow.xaml
Maison.xaml
Résultat
04_notesDeCours – projet SGDB G. Collard 73
DataBinding (liaison de données)
Le binding est un mécanisme qui lie deux sources de données et qui assure la synchronisation.
Ce schéma ne présente pas un cycle. Il met en exergue la synchronisation d’une propriété de la
vue-modèle vers la vue et réciproquement.
L’objet source situé dans la couche vue-modèle contient une propriété source qui fait l’objet d’une
synchronisation via le binding.
Coté vue, un objet, pas nécessairement visuel, contient une propriété cible qu’elle aussi peut faire
l’objet d’une synchronisation via le binding.
L’objet Binding à même de réaliser le Binding dans les deux sens.
Binding côté vue exclusivement
Côté vue, dans le code XAML, l’objet ou la propriété « bindée » utilise une expression « Binding » pour
réaliser cette synchronisation.
L’objet Binding statique utilise trois propriétés : Source, RelativeSource ou ElementName pour se lier
avec l’objet source. Le Binding ne se réalise pas nécessairement avec la vue-modèle.
Propriété Source
Cette propriété permet de spécifier un Binding dont le chemin (path) est totalement connu et ne
dépend pas du DataContext.
Exemple : affiche le mois courant dans un TextBlock
04_notesDeCours – projet SGDB G. Collard 74
Nous sommes au mois de novembre (donc 11 sera affiché)
Propriété RelativeSource
Cette propriété est relative contrairement à la propriété précédente, c’est-à-dire qu’elle envisage
comme contexte l’arbre visuel défini par le code XAML dans lequel le contrôle s’insère.
Dans cet arbre visuel, on peut rechercher des informations sur le contrôle courant (self) ou sur un
ancêtre, une balise située plus haut dans l’arbre (FindAncestor). Une dernière utilisation possible est
d’appliquer un Binding aux éléments soumis à un template donné (DataTemplate).
Exemple :
- Self : une première écriture voit sa couleur de texte identique à la couleur de fond de son
parent.
- FindAncestor : une seconde écriture voit sa couleur de texte identique à la couleur de fond
d’un ancêtre donné (StackPanel)
- TemplateParent : une troisième écriture voit sa couleur de fond issue du contrôle qui accueille
son Template.
04_notesDeCours – projet SGDB G. Collard 75
Propriété ElementName
Cette propriété permet de référencer un élément de la vue par son nom. Ainsi, une première
TextBlock affiche le nom d’une seconde TextBlock et réciproquement. Le référencement de chaque
contrôle se fait via le nom de chacune d’elles.
Exemple :
04_notesDeCours – projet SGDB G. Collard 76
Binding entre vue et vue-modèle
Présentation de l’objet de Binding
En général, l’objet est syntaxiquement manipulé dans la source XAML. Mais, il est tout à fait possible
de l’utiliser via du code C#.
Les principales propriétés de l’objet Binding sont les suivantes :
- Source : permet de définir la source du Binding
- Path : permet d’associer l’attribut de classe de la Source qui sera précisément « bindé ».
- Mode : plusieurs valeurs existent pour réaliser le binding : bidirectionnel ou unidirectionnel ?
- UpdateSourceTrigger : si le mode permet à la vue de modifier la vue-modèle, sur quel
événement va se déclencher la mise à jour ?
Propriété Mode de l’objet de binding
- OneWay : le binding est unidirectionnel, de la source vers la cible (de la vue-modèle vers la
vue)
- OneWayToSource : le binding est unidirectionnel, de la cible vers la source (de la vue vers la
vue-modèle).
- TwoWay : le Binding est bidirectionnel. La vue-modèle affecte la vue et réciproquement.
- OneTime : correspond à un Binding OneWay qui ne s’effectue que la première fois.
- Default : c’est un Binding par défaut qui dépend du contrôle considéré. Par exemple TextBox,
ComboBox, MenuItem, la valeur par défaut est TwoWay.
Propriété UpdateSourceTrigger de l’objet de binding
Cette propriété permet d’indiquer l’événement qui va notifier le Binding et donc déclencher le besoin
de synchronisation.
Les valeurs sont :
- Explicit : la synchronisation sera effectuée sur l’appel explicite de la méthode UpdateSource.
- LostFocus : la synchronisation de la source est réalisée dès perte de focus par le contrôle.
- PropertyChanged : la synchronisation de la source est réalisée à chaque changement de valeur
dans le contrôle.
- Default : correspond à la valeur par défaut associé à la propriété du contrôle considéré
(Souvent : PropertyChanged)
04_notesDeCours – projet SGDB G. Collard 77
Exemple Binding défini en C# (entre vue-modèle et vue)
MainWindow.xaml
MainWindow.xaml.cs
04_notesDeCours – projet SGDB G. Collard 78
On remarque dans le code précédent que la création du binding est faite de manière explicite en C# :
Binding monBindingISBN = new Binding();
monBindingISBN.Source = monContexte;
monBindingISBN.Path = new PropertyPath("ISBN");
monBindingISBN.Mode = BindingMode.TwoWay;
monBindingISBN.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
Elle se termine par une association entre DependancyObject, DependancyProperty et l’objet Binding
créé.
BindingOperations.SetBinding(ISBN, TextBox.TextProperty, monBindingISBN);
04_notesDeCours – projet SGDB G. Collard 79
Résultat:
Même exemple défini en XAML
MainWindows.xaml
MainWindow.xaml.cs
04_notesDeCours – projet SGDB G. Collard 80
Dans cet exemple, c’est le code ci-dessous qui se charge du binding, au sein du DataContext affecté
dans le constructeur de la Window.
<TextBox Margin="10" Text="{Binding ISBN, Mode=TwoWay,
UpdateSourceTrigger=LostFocus}" />
04_notesDeCours – projet SGDB G. Collard 81
Binding de collections
Quand on blinde une collection avec un contrôle (ITemsControl, ListBox, ListView, ComboBox,
DataGrid, …) qui affiche une collection, l’idée est de blinder le contenu de la collection elle-même
(élément inséré, élément supprimé) et blinder chaque élément de la collection.
- INotifyCollectionChanged participe à l’observation de la collection elle-même (éléments
insérés, supprimés, …).
- InotifyPropertyChanged implémentée par chaque élément de la collection permet d’observer
chaque élément lui-même et notamment ses composantes éventuellement modifiées.
Binding avec ObservableCollection<T>
Soit une grille comprenant une liste de voitures (immatriculation, couleur, marque, modèle) modifiable
et dans laquelle l’utilisateur souhaite pouvoir supprimer une ligne ou en ajuter une nouvelle.
04_notesDeCours – projet SGDB G. Collard 82
MainWindows.xaml
04_notesDeCours – projet SGDB G. Collard 83
MainWindows.xaml.cs
04_notesDeCours – projet SGDB G. Collard 84
Vue-modele.cs
04_notesDeCours – projet SGDB G. Collard 85
Résultat
➢ Au lancement de l’application
04_notesDeCours – projet SGDB G. Collard 86
➢ En cliquant sur suppression
➢ En cliquant sur ajout
Binding avec DataView
Exemple : binding avec un DataView construit depuis une DataTable vers un contrôle de type ListBox.
L’élément sélectionné fera l’objet d’un binding avec un DataRowView. Il sera possible de supprimer
et d’ajouter un élément dans la liste.
Résultat :
04_notesDeCours – projet SGDB G. Collard 87
Binding avec DataView
MainWindow.xaml
04_notesDeCours – projet SGDB G. Collard 88
04_notesDeCours – projet SGDB G. Collard 89
MainWindow.xaml.cs
Vue-modele.cs
04_notesDeCours – projet SGDB G. Collard 90
04_notesDeCours – projet SGDB G. Collard 91
Binding de collection et ComboBox
La spécificité de la ComboBox réside dans le fait qu’il faut choisir ce qui est affiché. Ceci est définit
avec la propriété DisplayMemberPath.
Exemple :
04_notesDeCours – projet SGDB G. Collard 92
MainWindow.xaml
MainWindow.xaml.cs
04_notesDeCours – projet SGDB G. Collard 93
VueModele.cs
04_notesDeCours – projet SGDB G. Collard 94
04_notesDeCours – projet SGDB G. Collard 95