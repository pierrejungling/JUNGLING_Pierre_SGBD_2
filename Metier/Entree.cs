namespace Metier;

public class Entree
{
    public string Raison { get; set; } = string.Empty;
    public DateTime DateEntree { get; set; }
    public Animal Animal { get; set; } = new Animal();
    public Contact Contact { get; set; } = new Contact();

    public Entree() { }

    public Entree(string raison, DateTime dateEntree, Animal animal, Contact contact)
    {
        Raison = raison;
        DateEntree = dateEntree;
        Animal = animal;
        Contact = contact;
    }

    public override string ToString()
    {
        return $"{DateEntree:dd/MM/yyyy} - {Raison}";
    }
}
