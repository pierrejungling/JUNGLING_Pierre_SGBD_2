namespace Metier;

public class Animal
{
    public bool Sterilise { get; set; }
    public string Identifiant { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // "chat" ou "chien"
    public string Sexe { get; set; } = string.Empty; // "M" ou "F"
    public string? Particularite { get; set; }
    public string? Description { get; set; }
    public DateTime? DateDeces { get; set; }
    public DateTime? DateSterilisation { get; set; }
    public DateTime DateNaissance { get; set; }
    public List<string> Couleurs { get; set; } = new List<string>(); // string[] selon le diagramme
    public List<Compatibilite> Compatibilites { get; set; } = new List<Compatibilite>();

    public Animal() { }

    public Animal(string identifiant, string nom, string type, string sexe, DateTime dateNaissance, bool sterilise)
    {
        Identifiant = identifiant;
        Nom = nom;
        Type = type;
        Sexe = sexe;
        DateNaissance = dateNaissance;
        Sterilise = sterilise;
    }

    public override string ToString()
    {
        return $"{Nom} ({Type}) - ID: {Identifiant}";
    }
}
