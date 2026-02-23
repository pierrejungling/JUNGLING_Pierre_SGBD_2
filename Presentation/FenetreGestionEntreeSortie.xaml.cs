using System.Windows;
using DAL;

namespace Presentation;

public partial class FenetreGestionEntreeSortie : Window
{
    public FenetreGestionEntreeSortie()
    {
        InitializeComponent();
    }

    private void Entrees_Click(object sender, RoutedEventArgs e)
    {
        string? id = TxtIdAnimal.Text?.Trim();
        if (string.IsNullOrWhiteSpace(id)) { MessageBox.Show("Saisissez l'identifiant de l'animal.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        try
        {
            var entrees = EntreeDAO.ListerParAnimal(id);
            Liste.ItemsSource = entrees;
        }
        catch (System.Exception ex) { MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void Sorties_Click(object sender, RoutedEventArgs e)
    {
        string? id = TxtIdAnimal.Text?.Trim();
        if (string.IsNullOrWhiteSpace(id)) { MessageBox.Show("Saisissez l'identifiant de l'animal.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        try
        {
            var sorties = SortieDAO.ListerParAnimal(id);
            Liste.ItemsSource = sorties;
        }
        catch (System.Exception ex) { MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void Fermer_Click(object sender, RoutedEventArgs e) => Close();
}
