using System.Text.RegularExpressions;
using System.Windows;
using DAL;
using Metier;

namespace Presentation;

public partial class FenetreAjoutContact : Window
{
    private static readonly Regex RegistreNationalRegex = new(@"^[0-9]{2}\.[0-9]{2}\.[0-9]{2}-[0-9]{3}\.[0-9]{2}$", RegexOptions.Compiled);
    private static readonly Regex EmailRegex = new(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$", RegexOptions.Compiled);
    private readonly int? _contactIdModif;

    public FenetreAjoutContact()
    {
        InitializeComponent();
        ChargerRoles();
    }

    public FenetreAjoutContact(Contact contactAModifier) : this()
    {
        _contactIdModif = contactAModifier.Identifiant;
        Title = "Modifier le contact";
        var c = ContactDAO.Consulter(contactAModifier.Identifiant);
        if (c == null) return;
        TbNom.Text = c.Nom;
        TbPrenom.Text = c.Prenom;
        TbRue.Text = c.AdresseContact.Rue;
        TbCp.Text = c.AdresseContact.Cp.ToString();
        TbLocalite.Text = c.AdresseContact.Localite;
        TbRN.Text = c.RegistreNational;
        TbGsm.Text = c.Gsm ?? "";
        TbTelephone.Text = c.Telephone ?? "";
        TbEmail.Text = c.Email ?? "";
        foreach (var r in c.Role)
        {
            foreach (var item in LbRoles.Items)
                if (item is Role lr && lr.IdRole == r.IdRole)
                { LbRoles.SelectedItems.Add(item); break; }
        }
    }

    private void ChargerRoles()
    {
        try
        {
            LbRoles.ItemsSource = RoleDAO.ListerTous();
        }
        catch
        {
            LbRoles.ItemsSource = null;
        }
    }

    private static void SetErreur(System.Windows.Controls.TextBlock block, string? message)
    {
        block.Text = message ?? "";
        block.Visibility = string.IsNullOrWhiteSpace(block.Text) ? Visibility.Collapsed : Visibility.Visible;
    }

    private void EffacerErreurs()
    {
        SetErreur(ErrNom, null);
        SetErreur(ErrPrenom, null);
        SetErreur(ErrRue, null);
        SetErreur(ErrCp, null);
        SetErreur(ErrLocalite, null);
        SetErreur(ErrRN, null);
        SetErreur(ErrEmail, null);
        SetErreur(ErrCoordonnees, null);
    }

    private bool Valider(out Contact contact)
    {
        EffacerErreurs();
        contact = new Contact();
        var ok = true;

        var nom = (TbNom.Text ?? "").Trim();
        if (nom.Length < 2) { SetErreur(ErrNom, "Minimum 2 caractères."); ok = false; }

        var prenom = (TbPrenom.Text ?? "").Trim();
        if (prenom.Length < 2) { SetErreur(ErrPrenom, "Minimum 2 caractères."); ok = false; }

        var rue = (TbRue.Text ?? "").Trim();
        if (string.IsNullOrWhiteSpace(rue)) { SetErreur(ErrRue, "Champ obligatoire."); ok = false; }

        var localite = (TbLocalite.Text ?? "").Trim();
        if (string.IsNullOrWhiteSpace(localite)) { SetErreur(ErrLocalite, "Champ obligatoire."); ok = false; }

        if (!int.TryParse((TbCp.Text ?? "").Trim(), out var cp) || cp <= 0)
        {
            SetErreur(ErrCp, "Code postal invalide.");
            ok = false;
        }

        var rn = (TbRN.Text ?? "").Trim();
        if (!RegistreNationalRegex.IsMatch(rn))
        {
            SetErreur(ErrRN, "Format attendu : yy.mm.dd-999.99");
            ok = false;
        }

        var gsm = (TbGsm.Text ?? "").Trim();
        var tel = (TbTelephone.Text ?? "").Trim();
        var email = (TbEmail.Text ?? "").Trim();

        if (string.IsNullOrWhiteSpace(gsm) && string.IsNullOrWhiteSpace(tel) && string.IsNullOrWhiteSpace(email))
        {
            SetErreur(ErrCoordonnees, "Au moins une coordonnée est obligatoire (GSM, téléphone ou email).");
            ok = false;
        }

        if (!string.IsNullOrWhiteSpace(email) && !EmailRegex.IsMatch(email))
        {
            SetErreur(ErrEmail, "Adresse email invalide.");
            ok = false;
        }

        if (!ok) return false;

        if (_contactIdModif.HasValue)
            contact.Identifiant = _contactIdModif.Value;
        contact.Nom = nom;
        contact.Prenom = prenom;
        contact.RegistreNational = rn;
        contact.Gsm = string.IsNullOrWhiteSpace(gsm) ? null : gsm;
        contact.Telephone = string.IsNullOrWhiteSpace(tel) ? null : tel;
        contact.Email = string.IsNullOrWhiteSpace(email) ? null : email;
        contact.AdresseContact = new Adresse { Rue = rue, Cp = cp, Localite = localite };

        contact.Role.Clear();
        foreach (var item in LbRoles.SelectedItems)
            if (item is Role r)
                contact.Role.Add(r);

        return true;
    }

    private void Enregistrer_Click(object sender, RoutedEventArgs e)
    {
        if (!Valider(out var contact)) return;

        try
        {
            if (_contactIdModif.HasValue)
                ContactDAO.Modifier(contact);
            else
                ContactDAO.Ajouter(contact);
            DialogResult = true;
            Close();
        }
        catch (Exception ex)
        {
            AppliquerErreurBase(ex);
        }
    }

    private void AppliquerErreurBase(Exception ex)
    {
        var msg = ex.Message ?? "";
        if (msg.Contains("registre_national") || msg.Contains("CONTACT_registre_national") || msg.Contains("23505"))
        {
            SetErreur(ErrRN, "Ce registre national existe déjà.");
            return;
        }
        if (msg.Contains("contact_au_moins_un_contact"))
        {
            SetErreur(ErrCoordonnees, "Au moins une coordonnée est obligatoire (GSM, téléphone ou email).");
            return;
        }
        if (msg.Contains("email"))
        {
            SetErreur(ErrEmail, "Adresse email invalide.");
            return;
        }

        SetErreur(ErrRN, msg.Length > 140 ? msg.Substring(0, 137) + "…" : msg);
    }

    private void Annuler_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}

