-- ============================================
-- ANIMAL
-- ============================================

CREATE OR REPLACE FUNCTION animal_prochain_identifiant(p_date_entree DATE)
RETURNS VARCHAR(11) AS $$
DECLARE
    prefix VARCHAR(6);
    prochain_num INTEGER;
    dernier_id VARCHAR(11);
BEGIN
    prefix := TO_CHAR(p_date_entree, 'YYMMDD');
    SELECT identifiant INTO dernier_id
    FROM ANIMAL
    WHERE identifiant LIKE prefix || '%'
    ORDER BY identifiant DESC
    LIMIT 1;
    IF dernier_id IS NULL THEN
        prochain_num := 1;
    ELSE
        prochain_num := CAST(SUBSTRING(dernier_id FROM 7 FOR 5) AS INTEGER) + 1;
    END IF;
    RETURN prefix || LPAD(prochain_num::TEXT, 5, '0');
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION animal_inserer(
    p_identifiant VARCHAR(11),
    p_nom VARCHAR(100),
    p_type VARCHAR(10),
    p_sexe CHAR(1),
    p_particularites TEXT,
    p_date_deces DATE,
    p_description TEXT,
    p_date_sterilisation DATE,
    p_sterilise BOOLEAN,
    p_date_naissance DATE
)
RETURNS VOID AS $$
BEGIN
    INSERT INTO ANIMAL (identifiant, nom, type, sexe, particularites, date_deces,
                        description, date_sterilisation, sterilise, date_naissance)
    VALUES (p_identifiant, p_nom, p_type, p_sexe, p_particularites, p_date_deces,
            p_description, p_date_sterilisation, p_sterilise, p_date_naissance);
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION animal_consulter(p_identifiant VARCHAR(11))
RETURNS TABLE (
    identifiant VARCHAR(11),
    nom VARCHAR(100),
    type VARCHAR(10),
    sexe CHAR(1),
    particularites TEXT,
    date_deces DATE,
    description TEXT,
    date_sterilisation DATE,
    sterilise BOOLEAN,
    date_naissance DATE
) AS $$
BEGIN
    RETURN QUERY
    SELECT a.identifiant, a.nom, a.type, a.sexe, a.particularites, a.date_deces,
           a.description, a.date_sterilisation, a.sterilise, a.date_naissance
    FROM ANIMAL a
    WHERE a.identifiant = p_identifiant;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION animal_supprimer(p_identifiant VARCHAR(11))
RETURNS VOID AS $$
BEGIN
    DELETE FROM ANIMAL WHERE ANIMAL.identifiant = p_identifiant;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION animal_lister_tous()
RETURNS TABLE (
    identifiant VARCHAR(11),
    nom VARCHAR(100),
    type VARCHAR(10),
    sexe CHAR(1),
    particularites TEXT,
    date_deces DATE,
    description TEXT,
    date_sterilisation DATE,
    sterilise BOOLEAN,
    date_naissance DATE
) AS $$
BEGIN
    RETURN QUERY
    SELECT a.identifiant, a.nom, a.type, a.sexe, a.particularites, a.date_deces,
           a.description, a.date_sterilisation, a.sterilise, a.date_naissance
    FROM ANIMAL a
    ORDER BY a.nom;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION animal_lister_au_refuge()
RETURNS TABLE (
    identifiant VARCHAR(11),
    nom VARCHAR(100),
    type VARCHAR(10),
    sexe CHAR(1),
    particularites TEXT,
    date_deces DATE,
    description TEXT,
    date_sterilisation DATE,
    sterilise BOOLEAN,
    date_naissance DATE
) AS $$
BEGIN
    RETURN QUERY
    SELECT DISTINCT a.identifiant, a.nom, a.type, a.sexe, a.particularites,
           a.date_deces, a.description, a.date_sterilisation, a.sterilise, a.date_naissance
    FROM ANIMAL a
    WHERE a.date_deces IS NULL
    AND EXISTS (SELECT 1 FROM ANI_ENTREE e WHERE e.ani_identifiant = a.identifiant)
    AND (
        NOT EXISTS (SELECT 1 FROM ANI_SORTIE s WHERE s.ani_identifiant = a.identifiant)
        OR (
            (SELECT MAX(e2.date_entree) FROM ANI_ENTREE e2 WHERE e2.ani_identifiant = a.identifiant)
            >= COALESCE(
                (SELECT MAX(s2.date_sortie) FROM ANI_SORTIE s2 WHERE s2.ani_identifiant = a.identifiant),
                '1900-01-01'::date
            )
        )
    )
    ORDER BY a.nom;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION animal_charger_couleurs(p_identifiant VARCHAR(11))
RETURNS TABLE(nom_couleur VARCHAR(50)) AS $$
BEGIN
    RETURN QUERY
    SELECT c.nom_couleur
    FROM COULEUR c
    INNER JOIN ANIMAL_COULEUR ac ON c.col_identifiant = ac.col_identifiant
    WHERE ac.ani_identifiant = p_identifiant;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION animal_charger_compatibilites(p_identifiant VARCHAR(11))
RETURNS TABLE(valeur BOOLEAN, description TEXT, type VARCHAR(50)) AS $$
BEGIN
    RETURN QUERY
    SELECT ac.valeur, ac.description, c.type
    FROM ANI_COMPATIBILITE ac
    INNER JOIN COMPATIBILITE c ON ac.comp_identifiant = c.identifiant
    WHERE ac.ani_identifiant = p_identifiant;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION couleur_lister_toutes()
RETURNS TABLE(nom_couleur VARCHAR(50)) AS $$
BEGIN
    RETURN QUERY SELECT c.nom_couleur FROM COULEUR c ORDER BY c.nom_couleur;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION animal_ajouter_couleur(p_ani_identifiant VARCHAR(11), p_nom_couleur VARCHAR(50))
RETURNS VOID AS $$
DECLARE
    v_col_id INTEGER;
BEGIN
    SELECT col_identifiant INTO v_col_id FROM COULEUR WHERE nom_couleur = p_nom_couleur;
    IF v_col_id IS NULL THEN
        INSERT INTO COULEUR (nom_couleur) VALUES (p_nom_couleur) RETURNING col_identifiant INTO v_col_id;
    END IF;
    INSERT INTO ANIMAL_COULEUR (col_identifiant, ani_identifiant)
    VALUES (v_col_id, p_ani_identifiant)
    ON CONFLICT DO NOTHING;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION animal_ajouter_compatibilite(
    p_ani_identifiant VARCHAR(11),
    p_type VARCHAR(50),
    p_valeur BOOLEAN,
    p_description TEXT
)
RETURNS VOID AS $$
DECLARE
    v_comp_id INTEGER;
BEGIN
    SELECT identifiant INTO v_comp_id FROM COMPATIBILITE WHERE type = p_type;
    IF v_comp_id IS NULL THEN
        INSERT INTO COMPATIBILITE (type) VALUES (p_type) RETURNING identifiant INTO v_comp_id;
    END IF;
    INSERT INTO ANI_COMPATIBILITE (valeur, description, comp_identifiant, ani_identifiant)
    VALUES (p_valeur, p_description, v_comp_id, p_ani_identifiant)
    ON CONFLICT DO NOTHING;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION animal_modifier_description(p_identifiant VARCHAR(11), p_description TEXT)
RETURNS VOID AS $$
BEGIN
    UPDATE ANIMAL SET description = p_description WHERE identifiant = p_identifiant;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION animal_modifier_particularites(p_identifiant VARCHAR(11), p_particularites TEXT)
RETURNS VOID AS $$
BEGIN
    UPDATE ANIMAL SET particularites = p_particularites WHERE identifiant = p_identifiant;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION animal_modifier_date_deces(p_identifiant VARCHAR(11), p_date_deces DATE)
RETURNS VOID AS $$
BEGIN
    UPDATE ANIMAL SET date_deces = p_date_deces WHERE identifiant = p_identifiant;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION animal_supprimer_compatibilite(p_ani_identifiant VARCHAR(11), p_type VARCHAR(50))
RETURNS VOID AS $$
BEGIN
    DELETE FROM ANI_COMPATIBILITE
    WHERE ani_identifiant = p_ani_identifiant
    AND comp_identifiant = (SELECT identifiant FROM COMPATIBILITE WHERE type = p_type);
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION animal_supprimer_couleur(p_ani_identifiant VARCHAR(11), p_nom_couleur VARCHAR(50))
RETURNS VOID AS $$
BEGIN
    DELETE FROM ANIMAL_COULEUR
    WHERE ani_identifiant = p_ani_identifiant
    AND col_identifiant = (SELECT col_identifiant FROM COULEUR WHERE nom_couleur = p_nom_couleur);
END;
$$ LANGUAGE plpgsql;

-- ============================================
-- CONTACT
-- ============================================

CREATE OR REPLACE FUNCTION contact_inserer(
    p_nom VARCHAR(100),
    p_prenom VARCHAR(100),
    p_rue VARCHAR(200),
    p_cp INTEGER,
    p_localite VARCHAR(100),
    p_registre_national VARCHAR(15),
    p_gsm VARCHAR(20),
    p_telephone VARCHAR(20),
    p_email VARCHAR(255)
)
RETURNS INTEGER AS $$
DECLARE
    v_id INTEGER;
BEGIN
    INSERT INTO CONTACT (nom, prenom, rue, cp, localite, registre_national, gsm, telephone, email)
    VALUES (p_nom, p_prenom, p_rue, p_cp, p_localite, p_registre_national, p_gsm, p_telephone, p_email)
    RETURNING contact_identifiant INTO v_id;
    RETURN v_id;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION contact_consulter(p_id INTEGER)
RETURNS TABLE (
    contact_identifiant INTEGER,
    nom VARCHAR(100),
    prenom VARCHAR(100),
    rue VARCHAR(200),
    cp INTEGER,
    localite VARCHAR(100),
    registre_national VARCHAR(15),
    gsm VARCHAR(20),
    telephone VARCHAR(20),
    email VARCHAR(255)
) AS $$
BEGIN
    RETURN QUERY
    SELECT c.contact_identifiant, c.nom, c.prenom, c.rue, c.cp, c.localite,
           c.registre_national, c.gsm, c.telephone, c.email
    FROM CONTACT c
    WHERE c.contact_identifiant = p_id;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION contact_supprimer(p_id INTEGER)
RETURNS VOID AS $$
BEGIN
    DELETE FROM CONTACT WHERE contact_identifiant = p_id;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION contact_modifier(
    p_id INTEGER,
    p_nom VARCHAR(100),
    p_prenom VARCHAR(100),
    p_rue VARCHAR(200),
    p_cp INTEGER,
    p_localite VARCHAR(100),
    p_registre_national VARCHAR(15),
    p_gsm VARCHAR(20),
    p_telephone VARCHAR(20),
    p_email VARCHAR(255)
)
RETURNS VOID AS $$
BEGIN
    UPDATE CONTACT
    SET nom = p_nom, prenom = p_prenom, rue = p_rue, cp = p_cp,
        localite = p_localite, registre_national = p_registre_national,
        gsm = p_gsm, telephone = p_telephone, email = p_email
    WHERE contact_identifiant = p_id;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION contact_lister_tous()
RETURNS TABLE (
    contact_identifiant INTEGER,
    nom VARCHAR(100),
    prenom VARCHAR(100),
    rue VARCHAR(200),
    cp INTEGER,
    localite VARCHAR(100),
    registre_national VARCHAR(15),
    gsm VARCHAR(20),
    telephone VARCHAR(20),
    email VARCHAR(255)
) AS $$
BEGIN
    RETURN QUERY
    SELECT c.contact_identifiant, c.nom, c.prenom, c.rue, c.cp, c.localite,
           c.registre_national, c.gsm, c.telephone, c.email
    FROM CONTACT c
    ORDER BY c.nom, c.prenom;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION contact_charger_roles(p_contact_id INTEGER)
RETURNS TABLE(rol_identifiant INTEGER, rol_nom VARCHAR(50)) AS $$
BEGIN
    RETURN QUERY
    SELECT r.rol_identifiant, r.rol_nom
    FROM ROLE r
    INNER JOIN PERSONNE_ROLE pr ON r.rol_identifiant = pr.rol_identifiant
    WHERE pr.pers_identifiant = p_contact_id;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION contact_ajouter_role(p_contact_id INTEGER, p_role_id INTEGER)
RETURNS VOID AS $$
BEGIN
    INSERT INTO PERSONNE_ROLE (pers_identifiant, rol_identifiant)
    VALUES (p_contact_id, p_role_id);
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION contact_supprimer_tous_roles(p_contact_id INTEGER)
RETURNS VOID AS $$
BEGIN
    DELETE FROM PERSONNE_ROLE WHERE pers_identifiant = p_contact_id;
END;
$$ LANGUAGE plpgsql;

-- ============================================
-- ROLE
-- ============================================

CREATE OR REPLACE FUNCTION role_lister_tous()
RETURNS TABLE(rol_identifiant INTEGER, rol_nom VARCHAR(50)) AS $$
BEGIN
    RETURN QUERY
    SELECT r.rol_identifiant, r.rol_nom
    FROM ROLE r
    ORDER BY r.rol_nom;
END;
$$ LANGUAGE plpgsql;

-- ============================================
-- ADOPTION
-- ============================================

CREATE OR REPLACE FUNCTION adoption_ajouter(
    p_statut VARCHAR(50),
    p_date_demande DATE,
    p_ani_identifiant VARCHAR(11),
    p_adop_contact INTEGER
)
RETURNS VOID AS $$
DECLARE
    v_count INTEGER;
BEGIN
    SELECT COUNT(*)::INTEGER INTO v_count FROM ADOPTION WHERE ani_identifiant = p_ani_identifiant;
    IF v_count > 0 THEN
        RAISE EXCEPTION 'Cet animal a déjà une adoption (relation 1-1).';
    END IF;
    INSERT INTO ADOPTION (statut, date_demande, ani_identifiant, adop_contact)
    VALUES (p_statut, p_date_demande, p_ani_identifiant, p_adop_contact);
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION adoption_modifier_statut(p_ani_identifiant VARCHAR(11), p_statut VARCHAR(50))
RETURNS VOID AS $$
BEGIN
    UPDATE ADOPTION SET statut = p_statut WHERE ani_identifiant = p_ani_identifiant;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION adoption_supprimer(
    p_ani_identifiant VARCHAR(11),
    p_adop_contact INTEGER,
    p_date_demande DATE
)
RETURNS VOID AS $$
BEGIN
    DELETE FROM ADOPTION
    WHERE ani_identifiant = p_ani_identifiant
      AND adop_contact = p_adop_contact
      AND date_demande = p_date_demande;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION adoption_consulter_par_animal(p_ani_identifiant VARCHAR(11))
RETURNS TABLE(statut VARCHAR(50), date_demande DATE, ani_identifiant VARCHAR(11), adop_contact INTEGER) AS $$
BEGIN
    RETURN QUERY
    SELECT a.statut, a.date_demande, a.ani_identifiant, a.adop_contact
    FROM ADOPTION a
    WHERE a.ani_identifiant = p_ani_identifiant;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION adoption_lister_toutes()
RETURNS TABLE(statut VARCHAR(50), date_demande DATE, ani_identifiant VARCHAR(11), adop_contact INTEGER) AS $$
BEGIN
    RETURN QUERY
    SELECT a.statut, a.date_demande, a.ani_identifiant, a.adop_contact
    FROM ADOPTION a
    ORDER BY a.date_demande DESC;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION creer_sortie_adoption_acceptee()
RETURNS TRIGGER AS $$
DECLARE
    v_date_acceptation DATE := NEW.date_demande;
BEGIN
    IF NEW.statut = 'acceptee' THEN
        IF (TG_OP = 'INSERT') OR (OLD.statut IS NOT NULL AND OLD.statut <> 'acceptee') THEN
            INSERT INTO ANI_SORTIE (raison, date_sortie, ani_identifiant, sortie_contact)
            VALUES ('adoption', v_date_acceptation, NEW.ani_identifiant, NEW.adop_contact);
            UPDATE FAMILLE_ACCUEIL SET date_fin = v_date_acceptation
            WHERE fa_ani_identifiant = NEW.ani_identifiant AND date_fin IS NULL;
            IF NOT EXISTS (
                SELECT 1 FROM FAMILLE_ACCUEIL
                WHERE fa_ani_identifiant = NEW.ani_identifiant AND fa_contact = NEW.adop_contact AND date_fin IS NULL
            ) THEN
                INSERT INTO FAMILLE_ACCUEIL (date_debut, date_fin, fa_ani_identifiant, fa_contact)
                VALUES (v_date_acceptation, NULL, NEW.ani_identifiant, NEW.adop_contact);
            END IF;
        END IF;
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS trigger_creer_sortie_adoption_acceptee_update ON ADOPTION;
DROP TRIGGER IF EXISTS trigger_creer_sortie_adoption_acceptee_insert ON ADOPTION;
CREATE TRIGGER trigger_creer_sortie_adoption_acceptee_insert
AFTER INSERT ON ADOPTION
FOR EACH ROW
EXECUTE FUNCTION creer_sortie_adoption_acceptee();
CREATE TRIGGER trigger_creer_sortie_adoption_acceptee_update
AFTER UPDATE ON ADOPTION
FOR EACH ROW
EXECUTE FUNCTION creer_sortie_adoption_acceptee();

-- ============================================
-- ACCUEIL (FAMILLE_ACCUEIL)
-- ============================================

CREATE OR REPLACE FUNCTION accueil_ajouter(
    p_date_debut DATE,
    p_date_fin DATE,
    p_fa_ani_identifiant VARCHAR(11),
    p_fa_contact INTEGER
)
RETURNS VOID AS $$
BEGIN
    INSERT INTO FAMILLE_ACCUEIL (date_debut, date_fin, fa_ani_identifiant, fa_contact)
    VALUES (p_date_debut, p_date_fin, p_fa_ani_identifiant, p_fa_contact);
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION accueil_lister_par_animal(p_ani_identifiant VARCHAR(11))
RETURNS TABLE(date_debut DATE, date_fin DATE, fa_ani_identifiant VARCHAR(11), fa_contact INTEGER) AS $$
BEGIN
    RETURN QUERY
    SELECT fa.date_debut, fa.date_fin, fa.fa_ani_identifiant, fa.fa_contact
    FROM FAMILLE_ACCUEIL fa
    WHERE fa.fa_ani_identifiant = p_ani_identifiant
    ORDER BY fa.date_debut DESC;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION accueil_lister_par_famille(p_contact_id INTEGER)
RETURNS TABLE(date_debut DATE, date_fin DATE, fa_ani_identifiant VARCHAR(11), fa_contact INTEGER) AS $$
BEGIN
    RETURN QUERY
    SELECT fa.date_debut, fa.date_fin, fa.fa_ani_identifiant, fa.fa_contact
    FROM FAMILLE_ACCUEIL fa
    WHERE fa.fa_contact = p_contact_id
    ORDER BY fa.date_debut DESC;
END;
$$ LANGUAGE plpgsql;

-- ============================================
-- ENTRÉE (ANI_ENTREE)
-- ============================================

CREATE OR REPLACE FUNCTION entree_ajouter(
    p_raison VARCHAR(50),
    p_date_entree DATE,
    p_ani_identifiant VARCHAR(11),
    p_entree_contact INTEGER
)
RETURNS VOID AS $$
BEGIN
    INSERT INTO ANI_ENTREE (raison, date_entree, ani_identifiant, entree_contact)
    VALUES (p_raison, p_date_entree, p_ani_identifiant, p_entree_contact);
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION entree_lister_par_animal(p_ani_identifiant VARCHAR(11))
RETURNS TABLE(raison VARCHAR(50), date_entree DATE, ani_identifiant VARCHAR(11), entree_contact INTEGER) AS $$
BEGIN
    RETURN QUERY
    SELECT e.raison, e.date_entree, e.ani_identifiant, e.entree_contact
    FROM ANI_ENTREE e
    WHERE e.ani_identifiant = p_ani_identifiant
    ORDER BY e.date_entree DESC;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION cloturer_accueil_retour_adoption()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.raison = 'retour_adoption' THEN
        UPDATE FAMILLE_ACCUEIL
        SET date_fin = NEW.date_entree
        WHERE fa_ani_identifiant = NEW.ani_identifiant AND date_fin IS NULL;
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS trigger_cloturer_accueil_retour_adoption ON ANI_ENTREE;
CREATE TRIGGER trigger_cloturer_accueil_retour_adoption
AFTER INSERT ON ANI_ENTREE
FOR EACH ROW
WHEN (NEW.raison = 'retour_adoption')
EXECUTE FUNCTION cloturer_accueil_retour_adoption();

CREATE OR REPLACE FUNCTION rendre_animal_disponible_adoption_entree()
RETURNS TRIGGER AS $$
BEGIN
    DELETE FROM ADOPTION WHERE ani_identifiant = NEW.ani_identifiant;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS trigger_rendre_disponible_adoption_entree ON ANI_ENTREE;
CREATE TRIGGER trigger_rendre_disponible_adoption_entree
AFTER INSERT ON ANI_ENTREE
FOR EACH ROW
EXECUTE FUNCTION rendre_animal_disponible_adoption_entree();

-- ============================================
-- SORTIE
-- ============================================

CREATE OR REPLACE FUNCTION sortie_ajouter(
    p_raison VARCHAR(50),
    p_date_sortie DATE,
    p_ani_identifiant VARCHAR(11),
    p_sortie_contact INTEGER,
    p_date_deces_animal DATE DEFAULT NULL
)
RETURNS VOID AS $$
BEGIN
    IF p_raison = 'deces_animal' AND p_date_deces_animal IS NOT NULL THEN
        UPDATE ANIMAL SET date_deces = p_date_deces_animal WHERE identifiant = p_ani_identifiant;
    END IF;
    INSERT INTO ANI_SORTIE (raison, date_sortie, ani_identifiant, sortie_contact)
    VALUES (p_raison, p_date_sortie, p_ani_identifiant, p_sortie_contact);
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sortie_lister_par_animal(p_ani_identifiant VARCHAR(11))
RETURNS TABLE(raison VARCHAR(50), date_sortie DATE, ani_identifiant VARCHAR(11), sortie_contact INTEGER) AS $$
BEGIN
    RETURN QUERY
    SELECT s.raison, s.date_sortie, s.ani_identifiant, s.sortie_contact
    FROM ANI_SORTIE s
    WHERE s.ani_identifiant = p_ani_identifiant
    ORDER BY s.date_sortie DESC;
END;
$$ LANGUAGE plpgsql;

-- ============================================
-- VACCINATION (vaccin get or create + vaccination)
-- ============================================

CREATE OR REPLACE FUNCTION vaccination_ajouter(
    p_vaccination_date DATE,
    p_vac_animal VARCHAR(11),
    p_nom_vaccin VARCHAR(100)
)
RETURNS VOID AS $$
DECLARE
    v_vaccin_id INTEGER;
BEGIN
    SELECT identifiant INTO v_vaccin_id FROM VACCIN WHERE nom = p_nom_vaccin;
    IF v_vaccin_id IS NULL THEN
        INSERT INTO VACCIN (nom) VALUES (p_nom_vaccin) RETURNING identifiant INTO v_vaccin_id;
    END IF;
    INSERT INTO VACCINATION (vaccination_date, vac_animal, id_vaccin)
    VALUES (p_vaccination_date, p_vac_animal, v_vaccin_id);
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION vaccination_lister_par_animal(p_ani_identifiant VARCHAR(11))
RETURNS TABLE(vaccination_date DATE, vac_animal VARCHAR(11), nom_vaccin VARCHAR(100)) AS $$
BEGIN
    RETURN QUERY
    SELECT vac.vaccination_date, vac.vac_animal, v.nom
    FROM VACCINATION vac
    INNER JOIN VACCIN v ON vac.id_vaccin = v.identifiant
    WHERE vac.vac_animal = p_ani_identifiant
    ORDER BY vac.vaccination_date DESC;
END;
$$ LANGUAGE plpgsql;
