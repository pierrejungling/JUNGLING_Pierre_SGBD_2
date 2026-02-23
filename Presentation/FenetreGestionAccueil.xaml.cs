using System.Windows;
using DAL;

namespace Presentation;

public partial class FenetreGestionAccueil : Window
{
    public FenetreGestionAccueil()
    {
        InitializeComponent();
    }

    private void ListerParAnimal_Click(object sender, RoutedEventArgs e)
    {
        string? id = TxtIdAnimal.Text?.Trim();
        if (string.IsNullOrWhiteSpace(id)) { MessageBox.Show("Saisissez l'identifiant de l'animal.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        try
        {
            var accueils = AccueilDAO.ListerParAnimal(id);
            ListeAccueils.ItemsSource = accueils;
        }
        catch (System.Exception ex) { MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void ListerParFamille_Click(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(TxtIdContact.Text?.Trim(), out int contactId)) { MessageBox.Show("Saisissez l'identifiant du contact (nombre).", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        try
        {
            var accueils = AccueilDAO.ListerParFamille(contactId);
            ListeAccueils.ItemsSource = accueils;
        }
        catch (System.Exception ex) { MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void Fermer_Click(object sender, RoutedEventArgs e) => Close();
}
