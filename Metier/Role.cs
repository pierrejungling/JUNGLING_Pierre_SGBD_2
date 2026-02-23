namespace Metier;

public class Role
{
    public int IdRole { get; set; }
    public string NomRole { get; set; } = string.Empty;

    public Role() { }

    public Role(int idRole, string nomRole)
    {
        IdRole = idRole;
        NomRole = nomRole;
    }

    public override string ToString()
    {
        return NomRole;
    }
}
