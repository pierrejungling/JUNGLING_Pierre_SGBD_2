using System.Windows;
using DAL;
using Metier;

namespace Presentation;

public partial class FenetreAjoutVaccination : Window
{
    public FenetreAjoutVaccination()
    {
        InitializeComponent();
        DpDate.SelectedDate = DateTime.Today;
        ChargerAnimaux();
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

    private sealed class AnimalItem
    {
        public Animal Animal { get; }
        public string DisplayText => $"{Animal.Identifiant} - {Animal.Nom} ({Animal.Type})";
        public AnimalItem(Animal a) => Animal = a;
    }

    private static void SetErr(System.Windows.Controls.TextBlock b, string? msg)
    {
        b.Text = msg ?? "";
        b.Visibility = string.IsNullOrWhiteSpace(b.Text) ? Visibility.Collapsed : Visibility.Visible;
    }

    private void Enregistrer_Click(object sender, RoutedEventArgs e)
    {
        SetErr(ErrNom, null);
        var animalItem = CbAnimal.SelectedItem as AnimalItem;
        var nom = (TbNomVaccin.Text ?? "").Trim();
        if (animalItem?.Animal == null) { MessageBox.Show("Sélectionnez un animal.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        if (string.IsNullOrWhiteSpace(nom)) { SetErr(ErrNom, "Nom du vaccin obligatoire."); return; }
        if (!DpDate.SelectedDate.HasValue) { MessageBox.Show("Sélectionnez une date.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        var vacc = new Vaccination
        {
            Animal = animalItem.Animal,
            Vaccin = new Vaccin(nom),
            DateVaccination = DpDate.SelectedDate!.Value.Date
        };
        try
        {
            VaccinationDAO.Ajouter(vacc);
            DialogResult = true;
            Close();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void Annuler_Click(object sender, RoutedEventArgs e) { DialogResult = false; Close(); }
}
