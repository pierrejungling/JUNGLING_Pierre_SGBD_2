using System.Windows;
using DAL;

namespace Presentation;

public partial class FenetreGestionVaccinations : Window
{
    public FenetreGestionVaccinations()
    {
        InitializeComponent();
    }

    private void Afficher_Click(object sender, RoutedEventArgs e)
    {
        string? id = TxtIdAnimal.Text?.Trim();
        if (string.IsNullOrWhiteSpace(id)) { MessageBox.Show("Saisissez l'identifiant de l'animal.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        try
        {
            var vaccinations = VaccinationDAO.ListerParAnimal(id);
            ListeVaccinations.ItemsSource = vaccinations;
        }
        catch (System.Exception ex) { MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void Fermer_Click(object sender, RoutedEventArgs e) => Close();
}
