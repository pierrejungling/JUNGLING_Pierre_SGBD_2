namespace Metier;

public class Adresse
{
    public string Rue { get; set; } = string.Empty;
    public int Cp { get; set; }
    public string Localite { get; set; } = string.Empty;

    public Adresse() { }

    public Adresse(string rue, int cp, string localite)
    {
        Rue = rue;
        Cp = cp;
        Localite = localite;
    }

    public override string ToString()
    {
        return $"{Rue}, {Cp} {Localite}";
    }
}
