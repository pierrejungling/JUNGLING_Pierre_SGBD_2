-- Script d'insertion de données de test
-- À exécuter après avoir créé les tables
-- Auteur: JUNGLING Pierre
-- Date: 2026

-- ============================================
-- INSERTION DES RÔLES
-- ============================================

INSERT INTO ROLE (rol_nom) VALUES ('benevole') ON CONFLICT DO NOTHING;
INSERT INTO ROLE (rol_nom) VALUES ('adoptant') ON CONFLICT DO NOTHING;
INSERT INTO ROLE (rol_nom) VALUES ('candidat') ON CONFLICT DO NOTHING;
INSERT INTO ROLE (rol_nom) VALUES ('Famille_accueil') ON CONFLICT DO NOTHING;

-- ============================================
-- INSERTION DE COULEURS
-- ============================================

INSERT INTO COULEUR (nom_couleur) VALUES ('noir') ON CONFLICT DO NOTHING;
INSERT INTO COULEUR (nom_couleur) VALUES ('blanc') ON CONFLICT DO NOTHING;
INSERT INTO COULEUR (nom_couleur) VALUES ('marron') ON CONFLICT DO NOTHING;
INSERT INTO COULEUR (nom_couleur) VALUES ('roux') ON CONFLICT DO NOTHING;
INSERT INTO COULEUR (nom_couleur) VALUES ('gris') ON CONFLICT DO NOTHING;
INSERT INTO COULEUR (nom_couleur) VALUES ('tigré') ON CONFLICT DO NOTHING;

-- ============================================
-- INSERTION DE TYPES DE COMPATIBILITÉ
-- ============================================

INSERT INTO COMPATIBILITE (type) VALUES ('chat') ON CONFLICT DO NOTHING;
INSERT INTO COMPATIBILITE (type) VALUES ('chien') ON CONFLICT DO NOTHING;
INSERT INTO COMPATIBILITE (type) VALUES ('jeune enfant') ON CONFLICT DO NOTHING;
INSERT INTO COMPATIBILITE (type) VALUES ('enfant') ON CONFLICT DO NOTHING;
INSERT INTO COMPATIBILITE (type) VALUES ('jardin') ON CONFLICT DO NOTHING;
INSERT INTO COMPATIBILITE (type) VALUES ('poney') ON CONFLICT DO NOTHING;

-- ============================================
-- INSERTION DE VACCINS
-- ============================================

INSERT INTO VACCIN (nom) VALUES ('Vaccin antirabique') ON CONFLICT DO NOTHING;
INSERT INTO VACCIN (nom) VALUES ('Vaccin CHPPiL') ON CONFLICT DO NOTHING;
INSERT INTO VACCIN (nom) VALUES ('Vaccin contre la rage') ON CONFLICT DO NOTHING;
INSERT INTO VACCIN (nom) VALUES ('Vaccin contre la leucose') ON CONFLICT DO NOTHING;

-- ============================================
-- EXEMPLE D'INSERTION D'UN CONTACT (optionnel)
-- ============================================

-- Décommentez pour créer un contact de test
/*
INSERT INTO CONTACT (nom, prenom, rue, cp, localite, registre_national, GSM, email)
VALUES (
    'Dupont',
    'Jean',
    'Rue de la Paix, 10',
    4000,
    'Liège',
    '85.03.15-123.45',
    '0478123456',
    'jean.dupont@email.com'
) ON CONFLICT (registre_national) DO NOTHING;

-- Ajouter un rôle au contact
INSERT INTO PERSONNE_ROLE (pers_identifiant, rol_identifiant)
SELECT contact_identifiant, rol_identifiant
FROM CONTACT, ROLE
WHERE registre_national = '85.03.15-123.45'
AND rol_nom = 'adoptant'
ON CONFLICT DO NOTHING;
*/

-- ============================================
-- VÉRIFICATION DES DONNÉES
-- ============================================

-- Vérifier les rôles
SELECT * FROM ROLE;

-- Vérifier les couleurs
SELECT * FROM COULEUR;

-- Vérifier les compatibilités
SELECT * FROM COMPATIBILITE;

-- Vérifier les vaccins
SELECT * FROM VACCIN;
