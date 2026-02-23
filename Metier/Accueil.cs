namespace Metier;

public class Accueil
{
    public DateTime DateDebut { get; set; }
    public DateTime? DateFin { get; set; }
    public Animal AnimalAccueilli { get; set; } = new Animal();
    public Contact FamilleAccueil { get; set; } = new Contact();

    public Accueil() { }

    public Accueil(DateTime dateDebut, DateTime? dateFin, Animal animalAccueilli, Contact familleAccueil)
    {
        DateDebut = dateDebut;
        DateFin = dateFin;
        AnimalAccueilli = animalAccueilli;
        FamilleAccueil = familleAccueil;
    }

    public bool EstActif()
    {
        return DateFin == null;
    }

    public override string ToString()
    {
        string dateFinStr = DateFin.HasValue ? DateFin.Value.ToString("dd/MM/yyyy") : "En cours";
        return $"{AnimalAccueilli.Nom} - {FamilleAccueil.Prenom} {FamilleAccueil.Nom} ({DateDebut:dd/MM/yyyy} - {dateFinStr})";
    }
}
