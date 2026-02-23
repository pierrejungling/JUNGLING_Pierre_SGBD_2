using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DAL;
using Metier;

namespace Presentation;

public partial class FenetreAjoutAnimal : Window
{
    public FenetreAjoutAnimal()
    {
        InitializeComponent();
        DpDateNaissance.SelectedDate = DateTime.Today;
        DpDateEntree.SelectedDate = DateTime.Today;
        ChargerContacts();
        ChargerCouleurs();
        ChargerMotifsEntree();
        DpDateEntree.SelectedDateChanged += (s, e) => { if (DpDateEntree.SelectedDate.HasValue) GenererIdentifiant(); };
        Loaded += (s, e) => GenererIdentifiant();
    }

    private void ChargerMotifsEntree()
    {
        var motifs = new List<RaisonItem>
        {
            new("Abandon", "abandon"),
            new("Errant", "errant"),
            new("Décès propriétaire", "deces_proprietaire"),
            new("Saisie", "saisie"),
            new("Retour adoption", "retour_adoption"),
            new("Retour famille d'accueil", "retour_famille_accueil")
        };
        CbRaison.ItemsSource = motifs;
        CbRaison.DisplayMemberPath = "Label";
        CbRaison.SelectedIndex = 0;
    }

    private sealed class RaisonItem
    {
        public string Label { get; }
        public string Value { get; }
        public RaisonItem(string label, string value) { Label = label; Value = value; }
    }

    private void ChargerCouleurs()
    {
        try
        {
            LbCouleurs.ItemsSource = CouleurDAO.ListerToutes();
        }
        catch { LbCouleurs.ItemsSource = new List<string>(); }
    }

    private void ChargerContacts()
    {
        var liste = new List<ContactItem> { new ContactItem(null) };
        try
        {
            foreach (var c in ContactDAO.ListerTous())
                liste.Add(new ContactItem(c));
        }
        catch { /* ignorer si pas de contacts */ }
        CbContact.ItemsSource = liste;
        CbContact.SelectedIndex = 0;
    }

    private sealed class ContactItem
    {
        public Contact? Contact { get; }
        public string DisplayText => Contact == null ? "Aucun" : Contact.ToString()!;
        public ContactItem(Contact? c) => Contact = c;
    }

    private void GenererIdentifiant_Click(object sender, RoutedEventArgs e)
    {
        GenererIdentifiant();
    }

    private void GenererIdentifiant()
    {
        if (!DpDateEntree.SelectedDate.HasValue) return;
        try
        {
            TbIdentifiant.Text = AnimalDAO.ProchainIdentifiant(DpDateEntree.SelectedDate.Value);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void CbSterilise_Changed(object sender, RoutedEventArgs e)
    {
        RowDateSterilisation.Visibility = CbSterilise.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
        if (CbSterilise.IsChecked == true && !DpDateSterilisation.SelectedDate.HasValue)
            DpDateSterilisation.SelectedDate = DateTime.Today;
    }

    private void EffacerErreurs()
    {
        AfficherErreur(ErrIdentifiant, null);
        AfficherErreur(ErrNom, null);
        AfficherErreur(ErrDateNaissance, null);
        AfficherErreur(ErrDateSterilisation, null);
        AfficherErreur(ErrRaison, null);
        AfficherErreur(ErrDateEntree, null);
    }

    private static void AfficherErreur(System.Windows.Controls.TextBlock block, string? message)
    {
        block.Text = message ?? "";
        block.Visibility = string.IsNullOrEmpty(block.Text) ? Visibility.Collapsed : Visibility.Visible;
    }

    private bool ValiderChamps(out string? id, out string? nom, out DateTime? dateNaissance, out DateTime? dateSterilisation, out string? raison)
    {
        id = null;
        nom = null;
        dateNaissance = null;
        dateSterilisation = null;
        raison = null;
        EffacerErreurs();
        var ok = true;

        id = (TbIdentifiant.Text ?? "").Trim();
        if (id.Length != 11 || !id.All(char.IsDigit))
        {
            AfficherErreur(ErrIdentifiant, "Exactement 11 chiffres (yymmdd99999).");
            ok = false;
        }
        else if (!DpDateEntree.SelectedDate.HasValue)
        {
            AfficherErreur(ErrDateEntree, "Saisissez la date d'entrée.");
            ok = false;
        }
        else
        {
            var prefixAttendu = DpDateEntree.SelectedDate.Value.ToString("yyMMdd");
            if (id.Length >= 6 && id.Substring(0, 6) != prefixAttendu)
            {
                AfficherErreur(ErrIdentifiant, "L'identifiant doit commencer par la date d'entrée (yymmdd).");
                ok = false;
            }
        }

        nom = (TbNom.Text ?? "").Trim();
        if (string.IsNullOrEmpty(nom))
        {
            AfficherErreur(ErrNom, "Saisissez le nom.");
            ok = false;
        }

        if (!DpDateNaissance.SelectedDate.HasValue)
        {
            AfficherErreur(ErrDateNaissance, "Saisissez la date de naissance.");
            ok = false;
        }
        else
        {
            dateNaissance = DpDateNaissance.SelectedDate.Value;
            if (dateNaissance.Value.Date > DateTime.Today)
            {
                AfficherErreur(ErrDateNaissance, "La date de naissance ne peut pas être dans le futur.");
                ok = false;
            }
        }

        var sterilise = CbSterilise.IsChecked == true;
        if (sterilise)
        {
            if (!DpDateSterilisation.SelectedDate.HasValue)
            {
                AfficherErreur(ErrDateSterilisation, "Indiquez la date de stérilisation.");
                ok = false;
            }
            else
            {
                dateSterilisation = DpDateSterilisation.SelectedDate.Value;
                if (dateNaissance.HasValue && dateSterilisation.Value.Date < dateNaissance.Value.Date)
                {
                    AfficherErreur(ErrDateSterilisation, "La date de stérilisation doit être ≥ date de naissance.");
                    ok = false;
                }
                if (dateSterilisation.Value.Date > DateTime.Today)
                {
                    AfficherErreur(ErrDateSterilisation, "La date de stérilisation ne peut pas être dans le futur.");
                    ok = false;
                }
            }
        }

        var raisonItem = CbRaison.SelectedItem as RaisonItem;
        if (raisonItem == null)
        {
            AfficherErreur(ErrRaison, "Sélectionnez un motif d'entrée.");
            ok = false;
        }
        else
            raison = raisonItem.Value;

        if (!DpDateEntree.SelectedDate.HasValue && string.IsNullOrEmpty(ErrDateEntree.Text))
            AfficherErreur(ErrDateEntree, "Saisissez la date d'entrée.");
        if (!DpDateEntree.SelectedDate.HasValue) ok = false;

        return ok;
    }

    private void Enregistrer_Click(object sender, RoutedEventArgs e)
    {
        if (!ValiderChamps(out var id, out var nom, out var dateNaissance, out var dateSterilisation, out var raison))
            return;

        var sterilise = CbSterilise.IsChecked == true;
        var animal = new Animal
        {
            Identifiant = id!,
            Nom = nom!,
            Type = ((ComboBoxItem)CbType.SelectedItem).Content.ToString()!,
            Sexe = ((ComboBoxItem)CbSexe.SelectedItem).Content.ToString()!,
            DateNaissance = dateNaissance!.Value,
            Sterilise = sterilise,
            DateSterilisation = dateSterilisation,
            Description = string.IsNullOrWhiteSpace(TbDescription.Text) ? null : TbDescription.Text.Trim(),
            Particularite = string.IsNullOrWhiteSpace(TbParticularites.Text) ? null : TbParticularites.Text.Trim()
        };
        foreach (var item in LbCouleurs.SelectedItems)
            if (item is string couleur)
                animal.Couleurs.Add(couleur);

        try
        {
            AnimalDAO.Ajouter(animal);
            var contactItem = CbContact.SelectedItem as ContactItem;
            var contact = contactItem?.Contact;
            var dateEntree = DpDateEntree.SelectedDate!.Value.Date;
            var entree = new Entree
            {
                Raison = raison!,
                DateEntree = dateEntree,
                Animal = animal,
                Contact = contact ?? new Contact { Identifiant = 0 }
            };
            EntreeDAO.Ajouter(entree);
            MessageBox.Show($"Animal et entrée enregistrés.\nEntrée au refuge le {dateEntree:dd/MM/yyyy} (motif : {raison}).", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
            Close();
        }
        catch (Exception ex)
        {
            AppliquerErreurContrainte(ex);
        }
    }

    private void AppliquerErreurContrainte(Exception ex)
    {
        EffacerErreurs();
        var msg = ex.Message ?? "";
        if (msg.Contains("animal_date_sterilisation"))
        {
            RowDateSterilisation.Visibility = Visibility.Visible;
            AfficherErreur(ErrDateSterilisation, "La date de stérilisation doit être ≥ date de naissance.");
            return;
        }
        if (msg.Contains("animal_sterilise_coherence"))
        {
            RowDateSterilisation.Visibility = Visibility.Visible;
            AfficherErreur(ErrDateSterilisation, "Si stérilisé, indiquez la date de stérilisation (≥ date de naissance).");
            return;
        }
        if (msg.Contains("animal_date_deces"))
        {
            AfficherErreur(ErrDateNaissance, "Incohérence avec la date de décès (non gérée dans ce formulaire).");
            return;
        }
        if (msg.Contains("ani_entree_pkey") || (msg.Contains("23505") && msg.Contains("entree")))
        {
            AfficherErreur(ErrDateEntree, "Une entrée existe déjà pour cette date. Choisissez une autre date d'entrée.");
            return;
        }
        if (msg.Contains("animal_identifiant") || msg.Contains("23505"))
        {
            AfficherErreur(ErrIdentifiant, "Cet identifiant existe déjà. Cliquez sur « Générer » pour en obtenir un nouveau.");
            return;
        }
        if (msg.Contains("retour_adoption") && msg.Contains("première entrée"))
        {
            AfficherErreur(ErrRaison, "Retour adoption : l'animal doit d'abord avoir une entrée et une sortie adoption. Utilisez un autre motif pour la première entrée (ex. saisie, errant).");
            return;
        }
        if (msg.Contains("date_naissance") || msg.Contains("date_naissance <= CURRENT_DATE"))
        {
            AfficherErreur(ErrDateNaissance, "La date de naissance doit être ≤ aujourd'hui.");
            return;
        }
        AfficherErreur(ErrIdentifiant, msg.Length > 120 ? msg.Substring(0, 117) + "…" : msg);
    }

    private void Annuler_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
