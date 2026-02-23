-- ============================================
-- SUPPRESSION DES OBJETS EXISTANTS
-- ============================================

-- Suppression des triggers
DROP TRIGGER IF EXISTS trigger_check_accueil_apres_mort ON FAMILLE_ACCUEIL;
DROP TRIGGER IF EXISTS trigger_check_vaccination_apres_mort ON VACCINATION;
DROP TRIGGER IF EXISTS trigger_check_sortie_apres_mort ON ANI_SORTIE;
DROP TRIGGER IF EXISTS trigger_check_entree_apres_mort ON ANI_ENTREE;
DROP TRIGGER IF EXISTS trigger_check_vaccination_unique_jour ON VACCINATION;
DROP TRIGGER IF EXISTS trigger_check_vaccination_date ON VACCINATION;
DROP TRIGGER IF EXISTS trigger_check_adoption_acceptee ON ANI_SORTIE;
DROP TRIGGER IF EXISTS trigger_check_accueil_unique_actif ON FAMILLE_ACCUEIL;
DROP TRIGGER IF EXISTS trigger_check_retour_adoption ON ANI_ENTREE;
DROP TRIGGER IF EXISTS trigger_check_sortie_deces ON ANI_SORTIE;
DROP TRIGGER IF EXISTS trigger_check_deces_coherence ON ANIMAL;
DROP TRIGGER IF EXISTS trigger_check_sortie_unique ON ANI_SORTIE;
DROP TRIGGER IF EXISTS trigger_check_entree_sans_sortie ON ANI_ENTREE;

-- Suppression des fonctions
DROP FUNCTION IF EXISTS check_accueil_apres_mort() CASCADE;
DROP FUNCTION IF EXISTS check_vaccination_apres_mort() CASCADE;
DROP FUNCTION IF EXISTS check_sortie_apres_mort() CASCADE;
DROP FUNCTION IF EXISTS check_entree_apres_mort() CASCADE;
DROP FUNCTION IF EXISTS check_vaccination_unique_jour() CASCADE;
DROP FUNCTION IF EXISTS check_vaccination_date() CASCADE;
DROP FUNCTION IF EXISTS check_adoption_acceptee() CASCADE;
DROP FUNCTION IF EXISTS check_accueil_unique_actif() CASCADE;
DROP FUNCTION IF EXISTS check_retour_adoption() CASCADE;
DROP FUNCTION IF EXISTS check_sortie_deces() CASCADE;
DROP FUNCTION IF EXISTS check_deces_coherence() CASCADE;
DROP FUNCTION IF EXISTS check_sortie_unique() CASCADE;
DROP FUNCTION IF EXISTS check_entree_sans_sortie() CASCADE;

-- Suppression des tables (dans l'ordre inverse des dépendances)
DROP TABLE IF EXISTS VACCINATION CASCADE;
DROP TABLE IF EXISTS ANI_COMPATIBILITE CASCADE;
DROP TABLE IF EXISTS ANIMAL_COULEUR CASCADE;
DROP TABLE IF EXISTS FAMILLE_ACCUEIL CASCADE;
DROP TABLE IF EXISTS ADOPTION CASCADE;
DROP TABLE IF EXISTS ANI_SORTIE CASCADE;
DROP TABLE IF EXISTS ANI_ENTREE CASCADE;
DROP TABLE IF EXISTS PERSONNE_ROLE CASCADE;
DROP TABLE IF EXISTS CONTACT CASCADE;
DROP TABLE IF EXISTS ANIMAL CASCADE;
DROP TABLE IF EXISTS COULEUR CASCADE;
DROP TABLE IF EXISTS COMPATIBILITE CASCADE;
DROP TABLE IF EXISTS VACCIN CASCADE;
DROP TABLE IF EXISTS ROLE CASCADE;

-- ============================================
-- CRÉATION DES TABLES
-- ============================================

-- Table ROLE
CREATE TABLE ROLE (
    rol_identifiant SERIAL PRIMARY KEY,
    rol_nom VARCHAR(50) NOT NULL CHECK (rol_nom IN ('benevole', 'adoptant', 'candidat', 'Famille_accueil'))
);

COMMENT ON TABLE ROLE IS 'Table des rôles des personnes de contact';
COMMENT ON COLUMN ROLE.rol_identifiant IS 'Identifiant unique du rôle';
COMMENT ON COLUMN ROLE.rol_nom IS 'Nom du rôle (benevole, adoptant, candidat, Famille_accueil)';

-- Table COULEUR
CREATE TABLE COULEUR (
    col_identifiant SERIAL PRIMARY KEY,
    nom_couleur VARCHAR(50) NOT NULL UNIQUE
);

COMMENT ON TABLE COULEUR IS 'Table des couleurs disponibles pour les animaux';
COMMENT ON COLUMN COULEUR.col_identifiant IS 'Identifiant unique de la couleur';
COMMENT ON COLUMN COULEUR.nom_couleur IS 'Nom de la couleur (unique)';

-- Table COMPATIBILITE
CREATE TABLE COMPATIBILITE (
    identifiant SERIAL PRIMARY KEY,
    type VARCHAR(50) NOT NULL UNIQUE CHECK (type IN ('chat', 'chien', 'jeune enfant', 'enfant', 'jardin', 'poney'))
);

COMMENT ON TABLE COMPATIBILITE IS 'Table des types de compatibilité';
COMMENT ON COLUMN COMPATIBILITE.identifiant IS 'Identifiant unique du type de compatibilité';
COMMENT ON COLUMN COMPATIBILITE.type IS 'Type de compatibilité (chat, chien, jeune enfant, enfant, jardin, poney)';

-- Table VACCIN
CREATE TABLE VACCIN (
    identifiant SERIAL PRIMARY KEY,
    nom VARCHAR(100) NOT NULL UNIQUE
);

COMMENT ON TABLE VACCIN IS 'Table des vaccins disponibles';
COMMENT ON COLUMN VACCIN.identifiant IS 'Identifiant unique du vaccin';
COMMENT ON COLUMN VACCIN.nom IS 'Nom du vaccin (unique)';

-- Table CONTACT
CREATE TABLE CONTACT (
    contact_identifiant SERIAL PRIMARY KEY,
    nom VARCHAR(100) NOT NULL CHECK (LENGTH(TRIM(nom)) >= 2),
    prenom VARCHAR(100) NOT NULL CHECK (LENGTH(TRIM(prenom)) >= 2),
    rue VARCHAR(200) NOT NULL,
    cp INTEGER NOT NULL CHECK (cp > 0),
    localite VARCHAR(100) NOT NULL,
    registre_national VARCHAR(15) NOT NULL UNIQUE CHECK (registre_national ~ '^[0-9]{2}\.[0-9]{2}\.[0-9]{2}-[0-9]{3}\.[0-9]{2}$'),
    GSM VARCHAR(20),
    telephone VARCHAR(20),
    email VARCHAR(255) CHECK (email IS NULL OR email ~ '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$'),
    CONSTRAINT contact_au_moins_un_contact CHECK (GSM IS NOT NULL OR telephone IS NOT NULL OR email IS NOT NULL)
);

COMMENT ON TABLE CONTACT IS 'Table des personnes de contact (adoptants, bénévoles, familles d''accueil)';
COMMENT ON COLUMN CONTACT.contact_identifiant IS 'Identifiant unique du contact';
COMMENT ON COLUMN CONTACT.registre_national IS 'Numéro de registre national belge (format: yy.mm.dd-999.99)';

-- Table PERSONNE_ROLE (table de liaison entre CONTACT et ROLE)
CREATE TABLE PERSONNE_ROLE (
    pers_identifiant INTEGER NOT NULL REFERENCES CONTACT(contact_identifiant) ON DELETE CASCADE,
    rol_identifiant INTEGER NOT NULL REFERENCES ROLE(rol_identifiant) ON DELETE CASCADE,
    PRIMARY KEY (pers_identifiant, rol_identifiant)
);

COMMENT ON TABLE PERSONNE_ROLE IS 'Table de liaison entre les contacts et leurs rôles';

-- Table ANIMAL
CREATE TABLE ANIMAL (
    identifiant VARCHAR(11) PRIMARY KEY CHECK (identifiant ~ '^[0-9]{11}$'),
    nom VARCHAR(100) NOT NULL,
    type VARCHAR(10) NOT NULL CHECK (type IN ('chat', 'chien')),
    sexe CHAR(1) NOT NULL CHECK (sexe IN ('M', 'F')),
    particularites TEXT,
    date_deces DATE,
    description TEXT,
    date_sterilisation DATE,
    sterilise BOOLEAN NOT NULL DEFAULT FALSE,
    date_naissance DATE NOT NULL CHECK (date_naissance <= CURRENT_DATE),
    CONSTRAINT animal_sterilise_coherence CHECK (
        (sterilise = FALSE AND date_sterilisation IS NULL) OR
        (sterilise = TRUE AND date_sterilisation IS NOT NULL)
    ),
    CONSTRAINT animal_date_sterilisation CHECK (
        date_sterilisation IS NULL OR date_sterilisation >= date_naissance
    ),
    CONSTRAINT animal_date_deces CHECK (
        date_deces IS NULL OR date_deces >= date_naissance
    )
);

COMMENT ON TABLE ANIMAL IS 'Table principale des animaux du refuge';
COMMENT ON COLUMN ANIMAL.identifiant IS 'Identifiant unique au format yymmdd99999 (11 chiffres)';
COMMENT ON COLUMN ANIMAL.type IS 'Type d''animal : chat ou chien';
COMMENT ON COLUMN ANIMAL.sexe IS 'Sexe de l''animal : M (mâle) ou F (femelle)';

-- Table ANIMAL_COULEUR (table de liaison entre ANIMAL et COULEUR)
CREATE TABLE ANIMAL_COULEUR (
    col_identifiant INTEGER NOT NULL REFERENCES COULEUR(col_identifiant) ON DELETE CASCADE,
    ani_identifiant VARCHAR(11) NOT NULL REFERENCES ANIMAL(identifiant) ON DELETE CASCADE,
    PRIMARY KEY (col_identifiant, ani_identifiant)
);

COMMENT ON TABLE ANIMAL_COULEUR IS 'Table de liaison entre les animaux et leurs couleurs';

-- Table ANI_COMPATIBILITE
CREATE TABLE ANI_COMPATIBILITE (
    valeur BOOLEAN NOT NULL,
    description TEXT,
    comp_identifiant INTEGER NOT NULL REFERENCES COMPATIBILITE(identifiant) ON DELETE CASCADE,
    ani_identifiant VARCHAR(11) NOT NULL REFERENCES ANIMAL(identifiant) ON DELETE CASCADE,
    PRIMARY KEY (comp_identifiant, ani_identifiant)
);

COMMENT ON TABLE ANI_COMPATIBILITE IS 'Table des compatibilités des animaux';
COMMENT ON COLUMN ANI_COMPATIBILITE.valeur IS 'Valeur de la compatibilité (TRUE/FALSE)';

-- Table VACCINATION
CREATE TABLE VACCINATION (
    vaccination_date DATE NOT NULL,
    vac_animal VARCHAR(11) NOT NULL REFERENCES ANIMAL(identifiant) ON DELETE CASCADE,
    id_vaccin INTEGER NOT NULL REFERENCES VACCIN(identifiant) ON DELETE CASCADE,
    PRIMARY KEY (vac_animal, id_vaccin, vaccination_date)
);

COMMENT ON TABLE VACCINATION IS 'Table des vaccinations administrées aux animaux';
COMMENT ON COLUMN VACCINATION.vaccination_date IS 'Date de la vaccination';

-- Table ANI_ENTREE
CREATE TABLE ANI_ENTREE (
    raison VARCHAR(50) NOT NULL CHECK (raison IN ('abandon', 'errant', 'deces_proprietaire', 'saisie', 'retour_adoption', 'retour_famille_accueil')),
    date_entree DATE NOT NULL,
    ani_identifiant VARCHAR(11) NOT NULL REFERENCES ANIMAL(identifiant) ON DELETE CASCADE,
    entree_contact INTEGER REFERENCES CONTACT(contact_identifiant) ON DELETE SET NULL,
    PRIMARY KEY (ani_identifiant, date_entree)
);

COMMENT ON TABLE ANI_ENTREE IS 'Table des entrées des animaux au refuge';
COMMENT ON COLUMN ANI_ENTREE.raison IS 'Raison de l''entrée au refuge';

-- Table ANI_SORTIE
CREATE TABLE ANI_SORTIE (
    raison VARCHAR(50) NOT NULL CHECK (raison IN ('adoption', 'retour_proprietaire', 'deces_animal', 'famille_accueil')),
    date_sortie DATE NOT NULL,
    ani_identifiant VARCHAR(11) NOT NULL REFERENCES ANIMAL(identifiant) ON DELETE CASCADE,
    sortie_contact INTEGER REFERENCES CONTACT(contact_identifiant) ON DELETE SET NULL,
    PRIMARY KEY (ani_identifiant, date_sortie)
);

COMMENT ON TABLE ANI_SORTIE IS 'Table des sorties des animaux du refuge';
COMMENT ON COLUMN ANI_SORTIE.raison IS 'Raison de la sortie du refuge';

-- Table ADOPTION
CREATE TABLE ADOPTION (
    statut VARCHAR(50) NOT NULL CHECK (statut IN ('demande', 'acceptee', 'rejet_environnement', 'rejet_comportement')),
    date_demande DATE NOT NULL,
    ani_identifiant VARCHAR(11) NOT NULL REFERENCES ANIMAL(identifiant) ON DELETE CASCADE,
    adop_contact INTEGER NOT NULL REFERENCES CONTACT(contact_identifiant) ON DELETE CASCADE,
    PRIMARY KEY (ani_identifiant, adop_contact, date_demande)
);

COMMENT ON TABLE ADOPTION IS 'Table des processus d''adoption';
COMMENT ON COLUMN ADOPTION.statut IS 'Statut de l''adoption (demande, acceptee, rejet_environnement, rejet_comportement)';

-- Table FAMILLE_ACCUEIL
CREATE TABLE FAMILLE_ACCUEIL (
    date_debut DATE NOT NULL,
    date_fin DATE,
    fa_ani_identifiant VARCHAR(11) NOT NULL REFERENCES ANIMAL(identifiant) ON DELETE CASCADE,
    fa_contact INTEGER NOT NULL REFERENCES CONTACT(contact_identifiant) ON DELETE CASCADE,
    PRIMARY KEY (fa_ani_identifiant, fa_contact, date_debut),
    CONSTRAINT famille_accueil_date_coherence CHECK (
        date_fin IS NULL OR date_fin >= date_debut
    )
);

COMMENT ON TABLE FAMILLE_ACCUEIL IS 'Table des familles d''accueil pour les animaux';
COMMENT ON COLUMN FAMILLE_ACCUEIL.date_fin IS 'Date de fin d''accueil (NULL si accueil en cours)';

-- ============================================
-- INDEX POUR AMÉLIORER LES PERFORMANCES
-- ============================================

-- Index sur les clés étrangères fréquemment utilisées
CREATE INDEX idx_animal_couleur_animal ON ANIMAL_COULEUR(ani_identifiant);
CREATE INDEX idx_animal_couleur_couleur ON ANIMAL_COULEUR(col_identifiant);
CREATE INDEX idx_ani_compatibilite_animal ON ANI_COMPATIBILITE(ani_identifiant);
CREATE INDEX idx_ani_compatibilite_comp ON ANI_COMPATIBILITE(comp_identifiant);
CREATE INDEX idx_vaccination_animal ON VACCINATION(vac_animal);
CREATE INDEX idx_vaccination_vaccin ON VACCINATION(id_vaccin);
CREATE INDEX idx_ani_entree_animal ON ANI_ENTREE(ani_identifiant);
CREATE INDEX idx_ani_entree_contact ON ANI_ENTREE(entree_contact);
CREATE INDEX idx_ani_sortie_animal ON ANI_SORTIE(ani_identifiant);
CREATE INDEX idx_ani_sortie_contact ON ANI_SORTIE(sortie_contact);
CREATE INDEX idx_adoption_animal ON ADOPTION(ani_identifiant);
CREATE INDEX idx_adoption_contact ON ADOPTION(adop_contact);
CREATE INDEX idx_famille_accueil_animal ON FAMILLE_ACCUEIL(fa_ani_identifiant);
CREATE INDEX idx_famille_accueil_contact ON FAMILLE_ACCUEIL(fa_contact);
CREATE INDEX idx_personne_role_contact ON PERSONNE_ROLE(pers_identifiant);
CREATE INDEX idx_personne_role_role ON PERSONNE_ROLE(rol_identifiant);

-- Index sur les colonnes fréquemment utilisées dans les requêtes
CREATE INDEX idx_animal_type ON ANIMAL(type);
CREATE INDEX idx_animal_date_naissance ON ANIMAL(date_naissance);
CREATE INDEX idx_animal_date_deces ON ANIMAL(date_deces) WHERE date_deces IS NOT NULL;
CREATE INDEX idx_animal_sterilise ON ANIMAL(sterilise);
CREATE INDEX idx_contact_nom ON CONTACT(nom);
CREATE INDEX idx_contact_prenom ON CONTACT(prenom);
CREATE INDEX idx_contact_registre_national ON CONTACT(registre_national);
CREATE INDEX idx_ani_entree_date ON ANI_ENTREE(date_entree);
CREATE INDEX idx_ani_entree_animal_date ON ANI_ENTREE(ani_identifiant, date_entree);
CREATE INDEX idx_ani_sortie_date ON ANI_SORTIE(date_sortie);
CREATE INDEX idx_ani_sortie_animal_date ON ANI_SORTIE(ani_identifiant, date_sortie);
CREATE INDEX idx_adoption_statut ON ADOPTION(statut);
CREATE INDEX idx_adoption_date_demande ON ADOPTION(date_demande);
CREATE INDEX idx_famille_accueil_date_debut ON FAMILLE_ACCUEIL(date_debut);
CREATE INDEX idx_famille_accueil_date_fin ON FAMILLE_ACCUEIL(date_fin) WHERE date_fin IS NOT NULL;
CREATE INDEX idx_vaccination_date ON VACCINATION(vaccination_date);
CREATE INDEX idx_vaccination_animal_date ON VACCINATION(vac_animal, vaccination_date);

-- ============================================
-- FONCTIONS ET TRIGGERS POUR LES CONTRAINTES COMPLEXES
-- ============================================

CREATE OR REPLACE FUNCTION check_entree_sans_sortie()
RETURNS TRIGGER AS $$
DECLARE
    derniere_sortie DATE;
BEGIN
    SELECT MAX(date_sortie) INTO derniere_sortie
    FROM ANI_SORTIE
    WHERE ani_identifiant = NEW.ani_identifiant
    AND date_sortie < NEW.date_entree;

    IF EXISTS (
        SELECT 1 FROM ANI_ENTREE e
        WHERE e.ani_identifiant = NEW.ani_identifiant
        AND e.date_entree < NEW.date_entree
        AND (derniere_sortie IS NULL OR e.date_entree > derniere_sortie)
        AND NOT EXISTS (
            SELECT 1 FROM ANI_SORTIE s
            WHERE s.ani_identifiant = e.ani_identifiant
            AND s.date_sortie >= e.date_entree
        )
    ) THEN
        RAISE EXCEPTION 'Un animal ne peut pas être entré plus d''une fois sans sortie depuis sa dernière entrée';
    END IF;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_check_entree_sans_sortie
BEFORE INSERT OR UPDATE ON ANI_ENTREE
FOR EACH ROW EXECUTE FUNCTION check_entree_sans_sortie();

CREATE OR REPLACE FUNCTION check_sortie_unique()
RETURNS TRIGGER AS $$
DECLARE
    derniere_entree DATE;
BEGIN
    SELECT MAX(date_entree) INTO derniere_entree
    FROM ANI_ENTREE
    WHERE ani_identifiant = NEW.ani_identifiant
    AND date_entree <= NEW.date_sortie;

    IF derniere_entree IS NOT NULL THEN
        IF EXISTS (
            SELECT 1 FROM ANI_SORTIE s
            WHERE s.ani_identifiant = NEW.ani_identifiant
            AND s.date_sortie > derniere_entree
            AND s.date_sortie < NEW.date_sortie
        ) THEN
            RAISE EXCEPTION 'Il ne peut y avoir qu''une seule sortie depuis la dernière entrée';
        END IF;
    END IF;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_check_sortie_unique
BEFORE INSERT OR UPDATE ON ANI_SORTIE
FOR EACH ROW EXECUTE FUNCTION check_sortie_unique();

CREATE OR REPLACE FUNCTION check_deces_coherence()
RETURNS TRIGGER AS $$
DECLARE
    derniere_sortie_raison VARCHAR(50);
BEGIN
    IF NEW.date_deces IS NOT NULL THEN
        SELECT raison INTO derniere_sortie_raison
        FROM ANI_SORTIE
        WHERE ani_identifiant = NEW.identifiant
        AND date_sortie = (
            SELECT MAX(date_sortie) FROM ANI_SORTIE
            WHERE ani_identifiant = NEW.identifiant
        )
        LIMIT 1;
        
        IF derniere_sortie_raison IS NOT NULL AND derniere_sortie_raison != 'deces_animal' THEN
            RAISE EXCEPTION 'Si date_deces est non null, la raison de la dernière sortie doit être ''deces_animal''';
        END IF;
    END IF;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_check_deces_coherence
AFTER UPDATE OF date_deces ON ANIMAL
FOR EACH ROW 
WHEN (NEW.date_deces IS NOT NULL)
EXECUTE FUNCTION check_deces_coherence();

CREATE OR REPLACE FUNCTION check_sortie_deces()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.raison = 'deces_animal' THEN
        IF NOT EXISTS (
            SELECT 1 FROM ANIMAL
            WHERE identifiant = NEW.ani_identifiant
            AND date_deces IS NOT NULL
        ) THEN
            RAISE EXCEPTION 'Si la raison de sortie est ''deces_animal'', date_deces doit être non null';
        END IF;
    END IF;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_check_sortie_deces
BEFORE INSERT OR UPDATE ON ANI_SORTIE
FOR EACH ROW 
WHEN (NEW.raison = 'deces_animal')
EXECUTE FUNCTION check_sortie_deces();

CREATE OR REPLACE FUNCTION check_retour_adoption()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.raison = 'retour_adoption' THEN
        IF NOT EXISTS (
            SELECT 1 FROM ANI_ENTREE
            WHERE ani_identifiant = NEW.ani_identifiant
        ) THEN
            RAISE EXCEPTION 'Un retour d''adoption ne peut pas être la première entrée de l''animal';
        END IF;

        IF NOT EXISTS (
            SELECT 1 FROM ANI_SORTIE
            WHERE ani_identifiant = NEW.ani_identifiant
            AND raison = 'adoption'
            AND date_sortie <= NEW.date_entree
        ) THEN
            RAISE EXCEPTION 'Un retour d''adoption nécessite une sortie avec raison ''adoption'' (date de sortie <= date de retour).';
        END IF;
    END IF;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_check_retour_adoption
BEFORE INSERT OR UPDATE ON ANI_ENTREE
FOR EACH ROW 
WHEN (NEW.raison = 'retour_adoption')
EXECUTE FUNCTION check_retour_adoption();

CREATE OR REPLACE FUNCTION check_accueil_unique_actif()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.date_fin IS NULL THEN
        IF EXISTS (
            SELECT 1 FROM FAMILLE_ACCUEIL
            WHERE fa_ani_identifiant = NEW.fa_ani_identifiant
            AND date_fin IS NULL
            AND (fa_contact != NEW.fa_contact OR date_debut != NEW.date_debut)
        ) THEN
            RAISE EXCEPTION 'Un animal ne peut pas avoir plusieurs accueils actifs simultanément';
        END IF;
    END IF;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_check_accueil_unique_actif
BEFORE INSERT OR UPDATE ON FAMILLE_ACCUEIL
FOR EACH ROW 
WHEN (NEW.date_fin IS NULL)
EXECUTE FUNCTION check_accueil_unique_actif();

CREATE OR REPLACE FUNCTION check_adoption_acceptee()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.raison = 'adoption' THEN
        IF NOT EXISTS (
            SELECT 1 FROM ADOPTION
            WHERE ani_identifiant = NEW.ani_identifiant
            AND statut = 'acceptee'
            AND date_demande <= NEW.date_sortie
        ) THEN
            RAISE EXCEPTION 'Si la raison de sortie est ''adoption'', il doit exister une ADOPTION avec statut ''acceptee''';
        END IF;
    END IF;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_check_adoption_acceptee
BEFORE INSERT OR UPDATE ON ANI_SORTIE
FOR EACH ROW 
WHEN (NEW.raison = 'adoption')
EXECUTE FUNCTION check_adoption_acceptee();

CREATE OR REPLACE FUNCTION check_vaccination_date()
RETURNS TRIGGER AS $$
DECLARE
    date_naissance_animal DATE;
BEGIN
    SELECT date_naissance INTO date_naissance_animal
    FROM ANIMAL
    WHERE identifiant = NEW.vac_animal;
    
    IF date_naissance_animal IS NOT NULL AND NEW.vaccination_date < date_naissance_animal THEN
        RAISE EXCEPTION 'La date de vaccination doit être supérieure ou égale à la date de naissance';
    END IF;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_check_vaccination_date
BEFORE INSERT OR UPDATE ON VACCINATION
FOR EACH ROW EXECUTE FUNCTION check_vaccination_date();

CREATE OR REPLACE FUNCTION check_vaccination_unique_jour()
RETURNS TRIGGER AS $$
BEGIN
    IF EXISTS (
        SELECT 1 FROM VACCINATION
        WHERE vac_animal = NEW.vac_animal
        AND id_vaccin = NEW.id_vaccin
        AND vaccination_date = NEW.vaccination_date
        AND (vac_animal, id_vaccin, vaccination_date) IS DISTINCT FROM (NEW.vac_animal, NEW.id_vaccin, NEW.vaccination_date)
    ) THEN
        RAISE EXCEPTION 'Un animal ne peut pas recevoir le même vaccin deux fois le même jour';
    END IF;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_check_vaccination_unique_jour
BEFORE INSERT OR UPDATE ON VACCINATION
FOR EACH ROW EXECUTE FUNCTION check_vaccination_unique_jour();

CREATE OR REPLACE FUNCTION check_entree_apres_mort()
RETURNS TRIGGER AS $$
DECLARE
    date_deces_animal DATE;
BEGIN
    SELECT date_deces INTO date_deces_animal
    FROM ANIMAL
    WHERE identifiant = NEW.ani_identifiant;
    
    IF date_deces_animal IS NOT NULL AND NEW.date_entree > date_deces_animal THEN
        RAISE EXCEPTION 'Un animal décédé ne peut pas avoir d''entrées après sa mort';
    END IF;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_check_entree_apres_mort
BEFORE INSERT OR UPDATE ON ANI_ENTREE
FOR EACH ROW EXECUTE FUNCTION check_entree_apres_mort();

CREATE OR REPLACE FUNCTION check_sortie_apres_mort()
RETURNS TRIGGER AS $$
DECLARE
    date_deces_animal DATE;
BEGIN
    SELECT date_deces INTO date_deces_animal
    FROM ANIMAL
    WHERE identifiant = NEW.ani_identifiant;
    
    IF date_deces_animal IS NOT NULL AND NEW.date_sortie > date_deces_animal THEN
        RAISE EXCEPTION 'Un animal décédé ne peut pas avoir de sorties après sa mort';
    END IF;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_check_sortie_apres_mort
BEFORE INSERT OR UPDATE ON ANI_SORTIE
FOR EACH ROW EXECUTE FUNCTION check_sortie_apres_mort();

CREATE OR REPLACE FUNCTION check_vaccination_apres_mort()
RETURNS TRIGGER AS $$
DECLARE
    date_deces_animal DATE;
BEGIN
    SELECT date_deces INTO date_deces_animal
    FROM ANIMAL
    WHERE identifiant = NEW.vac_animal;
    
    IF date_deces_animal IS NOT NULL AND NEW.vaccination_date > date_deces_animal THEN
        RAISE EXCEPTION 'Un animal décédé ne peut pas avoir de vaccinations après sa mort';
    END IF;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_check_vaccination_apres_mort
BEFORE INSERT OR UPDATE ON VACCINATION
FOR EACH ROW EXECUTE FUNCTION check_vaccination_apres_mort();

CREATE OR REPLACE FUNCTION check_accueil_apres_mort()
RETURNS TRIGGER AS $$
DECLARE
    date_deces_animal DATE;
BEGIN
    SELECT date_deces INTO date_deces_animal
    FROM ANIMAL
    WHERE identifiant = NEW.fa_ani_identifiant;
    
    IF date_deces_animal IS NOT NULL THEN
        IF NEW.date_debut > date_deces_animal OR (NEW.date_fin IS NOT NULL AND NEW.date_fin > date_deces_animal) THEN
            RAISE EXCEPTION 'Un animal décédé ne peut pas avoir d''accueils après sa mort';
        END IF;
    END IF;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_check_accueil_apres_mort
BEFORE INSERT OR UPDATE ON FAMILLE_ACCUEIL
FOR EACH ROW EXECUTE FUNCTION check_accueil_apres_mort();

