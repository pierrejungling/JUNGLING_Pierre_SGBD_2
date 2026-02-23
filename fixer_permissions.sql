-- Script de correction des permissions pour refuge_animaux_user
-- Auteur: JUNGLING Pierre
-- Date: 2026
-- Base de données: PostgreSQL
-- 
-- IMPORTANT: Ce script doit être exécuté en tant que superutilisateur (postgres)
-- Exécution: psql -U postgres -d refuge_animaux -f fixer_permissions.sql
-- OU dans pgAdmin: Cliquez droit sur la base refuge_animaux -> Query Tool
--                  Connectez-vous en tant que postgres, puis exécutez ce script

-- ============================================
-- CORRECTION DES PERMISSIONS
-- ============================================

-- S'assurer que l'utilisateur existe
DO $$
BEGIN
    IF NOT EXISTS (SELECT FROM pg_catalog.pg_user WHERE usename = 'refuge_animaux_user') THEN
        CREATE USER refuge_animaux_user WITH PASSWORD 'p@ssword';
    END IF;
END
$$;

-- Attribuer tous les privilèges sur le schéma public
GRANT ALL PRIVILEGES ON SCHEMA public TO refuge_animaux_user;
GRANT USAGE ON SCHEMA public TO refuge_animaux_user;

-- Configurer les privilèges par défaut pour les objets futurs
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON TABLES TO refuge_animaux_user;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON SEQUENCES TO refuge_animaux_user;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON FUNCTIONS TO refuge_animaux_user;

-- Donner tous les privilèges sur toutes les tables existantes ET changer le propriétaire
DO $$
DECLARE
    r RECORD;
BEGIN
    -- Donner tous les privilèges sur toutes les tables existantes
    FOR r IN 
        SELECT tablename 
        FROM pg_tables 
        WHERE schemaname = 'public'
    LOOP
        -- Changer le propriétaire de la table
        EXECUTE 'ALTER TABLE public.' || quote_ident(r.tablename) || ' OWNER TO refuge_animaux_user';
        -- Donner tous les privilèges
        EXECUTE 'GRANT ALL PRIVILEGES ON TABLE public.' || quote_ident(r.tablename) || ' TO refuge_animaux_user';
        RAISE NOTICE 'Permissions accordées et propriétaire changé pour la table: %', r.tablename;
    END LOOP;
    
    -- Donner tous les privilèges sur toutes les séquences existantes ET changer le propriétaire
    FOR r IN 
        SELECT sequence_name 
        FROM information_schema.sequences 
        WHERE sequence_schema = 'public'
    LOOP
        -- Changer le propriétaire de la séquence
        EXECUTE 'ALTER SEQUENCE public.' || quote_ident(r.sequence_name) || ' OWNER TO refuge_animaux_user';
        -- Donner tous les privilèges
        EXECUTE 'GRANT ALL PRIVILEGES ON SEQUENCE public.' || quote_ident(r.sequence_name) || ' TO refuge_animaux_user';
        RAISE NOTICE 'Permissions accordées et propriétaire changé pour la séquence: %', r.sequence_name;
    END LOOP;
    
    -- Donner les privilèges d'exécution sur toutes les fonctions existantes
    FOR r IN 
        SELECT proname, oidvectortypes(proargtypes) as argtypes
        FROM pg_proc 
        WHERE pronamespace = (SELECT oid FROM pg_namespace WHERE nspname = 'public')
    LOOP
        -- Changer le propriétaire de la fonction
        EXECUTE 'ALTER FUNCTION public.' || quote_ident(r.proname) || '(' || r.argtypes || ') OWNER TO refuge_animaux_user';
        -- Donner le privilège d'exécution
        EXECUTE 'GRANT EXECUTE ON FUNCTION public.' || quote_ident(r.proname) || '(' || r.argtypes || ') TO refuge_animaux_user';
        RAISE NOTICE 'Permissions accordées et propriétaire changé pour la fonction: %(%)', r.proname, r.argtypes;
    END LOOP;
END
$$;

-- Privilèges sur la base de données elle-même
GRANT ALL PRIVILEGES ON DATABASE refuge_animaux TO refuge_animaux_user;
GRANT CONNECT ON DATABASE refuge_animaux TO refuge_animaux_user;

-- ============================================
-- VÉRIFICATION
-- ============================================

-- Afficher les tables et leurs propriétaires
SELECT 
    tablename as "Table",
    tableowner as "Propriétaire"
FROM pg_tables 
WHERE schemaname = 'public'
ORDER BY tablename;

-- Afficher un message de succès
DO $$
BEGIN
    RAISE NOTICE '';
    RAISE NOTICE '============================================';
    RAISE NOTICE 'Permissions corrigées avec succès!';
    RAISE NOTICE 'L''utilisateur refuge_animaux_user a maintenant tous les privilèges.';
    RAISE NOTICE 'Toutes les tables lui appartiennent maintenant.';
    RAISE NOTICE '============================================';
    RAISE NOTICE '';
END
$$;