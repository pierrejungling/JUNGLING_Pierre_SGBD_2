using System.Windows;
using DAL;
using Metier;

namespace Presentation;

public partial class FenetreAjoutAccueil : Window
{
    public FenetreAjoutAccueil()
    {
        InitializeComponent();
        DpDateDebut.SelectedDate = DateTime.Today;
        ChargerAnimaux();
        ChargerContacts();
    }

    private void ChargerAnimaux()
    {
        var liste = new List<AnimalItem>();
        try
        {
            foreach (var a in AnimalDAO.ListerTous())
                liste.Add(new AnimalItem(a));
        }
        catch { }
        CbAnimal.ItemsSource = liste;
        if (liste.Count > 0) CbAnimal.SelectedIndex = 0;
    }

    private void ChargerContacts()
    {
        var liste = new List<ContactItem>();
        try
        {
            foreach (var c in ContactDAO.ListerTous())
                liste.Add(new ContactItem(c));
        }
        catch { }
        CbContact.ItemsSource = liste;
        if (liste.Count > 0) CbContact.SelectedIndex = 0;
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

    private static void SetErr(System.Windows.Controls.TextBlock b, string? msg)
    {
        b.Text = msg ?? "";
        b.Visibility = string.IsNullOrWhiteSpace(b.Text) ? Visibility.Collapsed : Visibility.Visible;
    }

    private void Enregistrer_Click(object sender, RoutedEventArgs e)
    {
        SetErr(ErrDateDebut, null);
        var animalItem = CbAnimal.SelectedItem as AnimalItem;
        var contactItem = CbContact.SelectedItem as ContactItem;
        if (animalItem?.Animal == null) { MessageBox.Show("Sélectionnez un animal.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        if (contactItem?.Contact == null) { MessageBox.Show("Sélectionnez un contact (famille d'accueil).", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        if (!DpDateDebut.SelectedDate.HasValue) { SetErr(ErrDateDebut, "Date début obligatoire."); return; }
        var dateFin = DpDateFin.SelectedDate;
        if (dateFin.HasValue && dateFin.Value < DpDateDebut.SelectedDate!.Value)
        { SetErr(ErrDateDebut, "La date fin doit être >= date début."); return; }
        var accueil = new Accueil(
            DpDateDebut.SelectedDate!.Value.Date,
            dateFin?.Date,
            animalItem.Animal,
            contactItem.Contact);
        try
        {
            AccueilDAO.Ajouter(accueil);
            DialogResult = true;
            Close();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void Annuler_Click(object sender, RoutedEventArgs e) { DialogResult = false; Close(); }
}
