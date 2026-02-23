namespace Metier;

public class Vaccination
{
    public DateTime DateVaccination { get; set; }
    public Animal Animal { get; set; } = new Animal();
    public Vaccin Vaccin { get; set; } = new Vaccin();

    public Vaccination() { }

    public Vaccination(DateTime dateVaccination, Animal animal, Vaccin vaccin)
    {
        DateVaccination = dateVaccination;
        Animal = animal;
        Vaccin = vaccin;
    }

    public override string ToString()
    {
        return $"{Vaccin.Nom} - {DateVaccination:dd/MM/yyyy}";
    }
}
