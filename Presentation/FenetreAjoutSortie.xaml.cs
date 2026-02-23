using System.Windows;
using DAL;
using Metier;

namespace Presentation;

public partial class FenetreAjoutSortie : Window
{
    public FenetreAjoutSortie()
    {
        InitializeComponent();
        try
        {
            DpDate.SelectedDate = DateTime.Today;
            ChargerAnimaux();
            ChargerContacts();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur au chargement: {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
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
        var liste = new List<ContactItem> { new ContactItem(null) };
        try
        {
            foreach (var c in ContactDAO.ListerTous())
                liste.Add(new ContactItem(c));
        }
        catch { }
        CbContact.ItemsSource = liste;
        CbContact.SelectedIndex = 0;
    }

    private void CbRaison_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (RowDateDeces == null || DpDate == null || DpDateDeces == null) return;
        var raison = (CbRaison.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content?.ToString();
        RowDateDeces.Visibility = raison == "deces_animal" ? Visibility.Visible : Visibility.Collapsed;
        if (raison == "deces_animal" && !DpDateDeces.SelectedDate.HasValue)
            DpDateDeces.SelectedDate = DpDate.SelectedDate ?? DateTime.Today;
    }

    private sealed class AnimalItem
    {
        public Animal Animal { get; }
        public string DisplayText => $"{Animal.Identifiant} - {Animal.Nom} ({Animal.Type})";
        public AnimalItem(Animal a) => Animal = a;
    }

    private sealed class ContactItem
    {
        public Contact? Contact { get; }
        public string DisplayText => Contact == null ? "Aucun" : $"{Contact.Prenom} {Contact.Nom} (ID: {Contact.Identifiant})";
        public ContactItem(Contact? c) => Contact = c;
    }

    private void Enregistrer_Click(object sender, RoutedEventArgs e)
    {
        var animalItem = CbAnimal.SelectedItem as AnimalItem;
        if (animalItem?.Animal == null) { MessageBox.Show("Sélectionnez un animal.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        if (!DpDate.SelectedDate.HasValue) { MessageBox.Show("Sélectionnez une date de sortie.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        var raison = (CbRaison.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content?.ToString() ?? "adoption";
        var contactItem = CbContact.SelectedItem as ContactItem;
        DateTime? dateDeces = null;
        if (raison == "deces_animal" && DpDateDeces.SelectedDate.HasValue)
            dateDeces = DpDateDeces.SelectedDate!.Value.Date;
        var sortie = new Sortie
        {
            Animal = animalItem.Animal,
            DateSortie = DpDate.SelectedDate!.Value.Date,
            Raison = raison,
            Contact = contactItem?.Contact ?? new Contact()
        };
        try
        {
            SortieDAO.Ajouter(sortie, dateDeces);
            DialogResult = true;
            Close();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void Annuler_Click(object sender, RoutedEventArgs e) { DialogResult = false; Close(); }
}
