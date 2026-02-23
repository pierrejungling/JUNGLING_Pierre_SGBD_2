namespace Metier;

public class Contact
{
    public int Identifiant { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public Adresse AdresseContact { get; set; } = new Adresse();
    public string RegistreNational { get; set; } = string.Empty; // char[15]
    public string? Gsm { get; set; }
    public string? Telephone { get; set; }
    public string? Email { get; set; }
    public List<Role> Role { get; set; } = new List<Role>();

    public Contact() { }

    public Contact(int identifiant, string nom, string prenom, Adresse adresseContact, 
                   string registreNational, string? gsm, string? telephone, string? email)
    {
        Identifiant = identifiant;
        Nom = nom;
        Prenom = prenom;
        AdresseContact = adresseContact;
        RegistreNational = registreNational;
        Gsm = gsm;
        Telephone = telephone;
        Email = email;
    }

    public override string ToString()
    {
        return $"{Prenom} {Nom} ({RegistreNational})";
    }
}
