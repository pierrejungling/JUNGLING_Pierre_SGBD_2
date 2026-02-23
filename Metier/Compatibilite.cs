namespace Metier;

public class Compatibilite
{
    public string Type { get; set; } = string.Empty;
    public bool Valeur { get; set; }
    public string? Description { get; set; }

    public Compatibilite() { }

    public Compatibilite(string type, bool valeur, string? description = null)
    {
        Type = type;
        Valeur = valeur;
        Description = description;
    }

    public override string ToString()
    {
        string valeurStr = Valeur ? "Oui" : "Non";
        return $"{Type}: {valeurStr}" + (Description != null ? $" - {Description}" : "");
    }
}
