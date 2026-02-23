namespace Metier;

public class Sortie
{
    public string Raison { get; set; } = string.Empty;
    public DateTime DateSortie { get; set; } // Note: dans le diagramme c'est date_entree mais c'est probablement une erreur
    public Animal Animal { get; set; } = new Animal();
    public Contact Contact { get; set; } = new Contact();

    public Sortie() { }

    public Sortie(string raison, DateTime dateSortie, Animal animal, Contact contact)
    {
        Raison = raison;
        DateSortie = dateSortie;
        Animal = animal;
        Contact = contact;
    }

    public override string ToString()
    {
        return $"{DateSortie:dd/MM/yyyy} - {Raison}";
    }
}
