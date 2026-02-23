using System.Windows;
using DAL;
using Metier;

namespace Presentation;

public partial class FenetreAjoutAdoption : Window
{
    public FenetreAjoutAdoption()
    {
        InitializeComponent();
        DpDateDemande.SelectedDate = DateTime.Today;
        ChargerAnimaux();
        ChargerContacts();
    }

    private void ChargerAnimaux()
    {
        var liste = new List<AnimalItem>();
        try
        {
            var auRefuge = AnimalDAO.ListerAnimauxAuRefuge();
            foreach (var a in auRefuge)
            {
                if (AdoptionDAO.ConsulterParAnimal(a.Identifiant) == null)
                    liste.Add(new AnimalItem(a));
            }
        }
        catch { /* ignorer */ }
        CbAnimal.ItemsSource = liste;
        if (liste.Count > 0)
            CbAnimal.SelectedIndex = 0;
    }

    private void ChargerContacts()
    {
        var liste = new List<ContactItem>();
        try
        {
            foreach (var c in ContactDAO.ListerTous())
                liste.Add(new ContactItem(c));
        }
        catch { /* ignorer */ }
        CbAdoptant.ItemsSource = liste;
        if (liste.Count > 0)
            CbAdoptant.SelectedIndex = 0;
    }

    private sealed class AnimalItem
    {
        public Animal Animal { get; }
        public string DisplayText => $"{Animal.Identifiant} - {Animal.Nom} ({Animal.Type})";
        public AnimalItem(Animal a) => Animal = a;
    }

    private sealed class ContactItem
    {
        public Contact Contact { get; }
        public string DisplayText => $"{Contact.Prenom} {Contact.Nom} (ID: {Contact.Identifiant})";
        public ContactItem(Contact c) => Contact = c;
    }

    private static void SetErreur(System.Windows.Controls.TextBlock block, string? message)
    {
        block.Text = message ?? "";
        block.Visibility = string.IsNullOrWhiteSpace(block.Text) ? Visibility.Collapsed : Visibility.Visible;
    }

    private void EffacerErreurs()
    {
        SetErreur(ErrAnimal, null);
        SetErreur(ErrAdoptant, null);
        SetErreur(ErrDateDemande, null);
    }

    private bool Valider(out Adoption adoption)
    {
        EffacerErreurs();
        adoption = new Adoption();
        var ok = true;

        var animalItem = CbAnimal.SelectedItem as AnimalItem;
        if (animalItem?.Animal == null)
        {
            SetErreur(ErrAnimal, "Sélectionnez un animal.");
            ok = false;
        }

        var contactItem = CbAdoptant.SelectedItem as ContactItem;
        if (contactItem?.Contact == null)
        {
            SetErreur(ErrAdoptant, "Sélectionnez un contact adoptant.");
            ok = false;
        }

        if (!DpDateDemande.SelectedDate.HasValue)
        {
            SetErreur(ErrDateDemande, "Date de demande obligatoire.");
            ok = false;
        }

        var statut = (CbStatut.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content?.ToString() ?? "demande";
        if (string.IsNullOrWhiteSpace(statut))
        {
            ok = false;
        }

        if (!ok) return false;

        adoption.AnimalAdopte = animalItem!.Animal;
        adoption.Adoptant = contactItem!.Contact;
        adoption.Statut = statut;
        adoption.DateDemande = DpDateDemande.SelectedDate!.Value.Date;
        return true;
    }

    private void Enregistrer_Click(object sender, RoutedEventArgs e)
    {
        if (!Valider(out var adoption)) return;

        try
        {
            AdoptionDAO.Ajouter(adoption);
            DialogResult = true;
            Close();
        }
        catch (Exception ex)
        {
            var msg = ex.Message ?? "";
            if (msg.Contains("déjà une adoption") || msg.Contains("relation 1-1"))
                SetErreur(ErrAnimal, "Cet animal a déjà une adoption.");
            else
                MessageBox.Show(msg, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Annuler_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
