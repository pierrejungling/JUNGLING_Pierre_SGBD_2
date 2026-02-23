namespace Metier;

public class Vaccin
{
    public string Nom { get; set; } = string.Empty;

    public Vaccin() { }

    public Vaccin(string nom)
    {
        Nom = nom;
    }

    public override string ToString()
    {
        return Nom;
    }
}
