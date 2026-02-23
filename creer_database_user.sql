-- Script de création de la base de données et de l'utilisateur
-- Auteur: JUNGLING Pierre
-- Date: 2026
-- Base de données: PostgreSQL
-- 
-- IMPORTANT: Ce script doit être exécuté en tant que superutilisateur (postgres)
-- Exécution: psql -U postgres -f creer_database_user.sql

-- ============================================
-- CRÉATION DE L'UTILISATEUR
-- ============================================

-- Suppression de l'utilisateur s'il existe déjà (optionnel, pour réinitialisation)
DROP USER IF EXISTS refuge_animaux_user;

-- Création de l'utilisateur avec mot de passe
CREATE USER refuge_animaux_user WITH PASSWORD 'p@ssword';

-- ============================================
-- CRÉATION DE LA BASE DE DONNÉES
-- ============================================

-- Suppression de la base de données si elle existe déjà (optionnel, pour réinitialisation)
DROP DATABASE IF EXISTS refuge_animaux;

-- Création de la base de données avec l'utilisateur comme propriétaire
CREATE DATABASE refuge_animaux
    WITH 
    OWNER = refuge_animaux_user
    ENCODING = 'UTF8'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;

-- ============================================
-- ATTRIBUTION DES PRIVILÈGES
-- ============================================

-- Connexion à la base de données refuge_animaux
\c refuge_animaux

-- Attribution de tous les privilèges sur le schéma public à l'utilisateur
GRANT ALL PRIVILEGES ON DATABASE refuge_animaux TO refuge_animaux_user;
GRANT ALL PRIVILEGES ON SCHEMA public TO refuge_animaux_user;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO refuge_animaux_user;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO refuge_animaux_user;

-- Attribution des privilèges pour les objets futurs
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON TABLES TO refuge_animaux_user;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON SEQUENCES TO refuge_animaux_user;

-- ============================================
-- COMMENTAIRES
-- ============================================

COMMENT ON DATABASE refuge_animaux IS 'Base de données pour la gestion du refuge d''animaux';
