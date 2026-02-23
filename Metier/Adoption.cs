namespace Metier;

public class Adoption
{
    public string Statut { get; set; } = string.Empty;
    public DateTime DateDemande { get; set; }
    public Animal AnimalAdopte { get; set; } = new Animal(); // Relation 1-1 avec Animal
    public Contact Adoptant { get; set; } = new Contact(); // Relation 1-1 avec Contact

    public Adoption() { }

    public Adoption(string statut, DateTime dateDemande, Animal animalAdopte, Contact adoptant)
    {
        Statut = statut;
        DateDemande = dateDemande;
        AnimalAdopte = animalAdopte;
        Adoptant = adoptant;
    }

    public override string ToString()
    {
        return $"{AnimalAdopte.Nom} - {Adoptant.Prenom} {Adoptant.Nom} - Statut: {Statut}";
    }
}
