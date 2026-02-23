-- ============================================
-- CRÉATION DE L'UTILISATEUR
-- ============================================

DROP USER IF EXISTS refuge_animaux_user;
CREATE USER refuge_animaux_user WITH PASSWORD 'p@ssword';

-- ============================================
-- CRÉATION DE LA BASE DE DONNÉES
-- ============================================

DROP DATABASE IF EXISTS refuge_animaux;
CREATE DATABASE refuge_animaux
    WITH 
    OWNER = refuge_animaux_user
    ENCODING = 'UTF8'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;

-- ============================================
-- ATTRIBUTION DES PRIVILÈGES
-- ============================================

\c refuge_animaux

GRANT ALL PRIVILEGES ON DATABASE refuge_animaux TO refuge_animaux_user;
GRANT ALL PRIVILEGES ON SCHEMA public TO refuge_animaux_user;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO refuge_animaux_user;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO refuge_animaux_user;

ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON TABLES TO refuge_animaux_user;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON SEQUENCES TO refuge_animaux_user;

COMMENT ON DATABASE refuge_animaux IS 'Base de données pour la gestion du refuge d''animaux';
