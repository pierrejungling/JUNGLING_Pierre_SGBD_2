\echo 'Vérification de l''existence des tables...'
\echo ''

SELECT 
    table_name,
    CASE 
        WHEN EXISTS (
            SELECT 1 FROM information_schema.tables 
            WHERE table_schema = 'public' 
            AND table_name = t.table_name
        ) THEN '✓ Existe'
        ELSE '✗ Manquante'
    END as statut
FROM (
    VALUES 
        ('ROLE'),
        ('COULEUR'),
        ('COMPATIBILITE'),
        ('VACCIN'),
        ('CONTACT'),
        ('ANIMAL'),
        ('COULEUR'),
        ('ANI_ENTREE'),
        ('ANI_SORTIE'),
        ('ADOPTION'),
        ('FAMILLE_ACCUEIL'),
        ('VACCINATION'),
        ('ANIMAL_COULEUR'),
        ('ANI_COMPATIBILITE'),
        ('PERSONNE_ROLE')
) AS t(table_name)
ORDER BY statut, table_name;

\echo ''
\echo 'Pour créer les tables manquantes, exécutez:'
\echo 'psql -U refuge_animaux_user -d refuge_animaux -f creertables_JUNGLING_Pierre.sql'
\echo ''