using System.Windows;
using DAL;
using Metier;

namespace Presentation;

public partial class FenetreAjoutEntree : Window
{
    public FenetreAjoutEntree()
    {
        InitializeComponent();
        DpDate.SelectedDate = DateTime.Today;
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
        if (!DpDate.SelectedDate.HasValue) { MessageBox.Show("Sélectionnez une date.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        var raison = (CbRaison.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content?.ToString() ?? "saisie";
        var contactItem = CbContact.SelectedItem as ContactItem;
        var entree = new Entree
        {
            Animal = animalItem.Animal,
            DateEntree = DpDate.SelectedDate!.Value.Date,
            Raison = raison,
            Contact = contactItem?.Contact ?? new Contact()
        };
        try
        {
            EntreeDAO.Ajouter(entree);
            DialogResult = true;
            Close();
        }
        catch (Exception ex)
        {
            var msg = ex.Message ?? "";
            if (msg.Contains("23505") || msg.Contains("ani_entree_pkey") || msg.Contains("dupliquée"))
                MessageBox.Show("Une entrée existe déjà pour cet animal à cette date.\nChoisissez une autre date.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                MessageBox.Show(msg, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Annuler_Click(object sender, RoutedEventArgs e) { DialogResult = false; Close(); }
}
