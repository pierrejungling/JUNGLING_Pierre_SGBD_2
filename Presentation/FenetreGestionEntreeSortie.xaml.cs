using System.Globalization;
using System.Windows;
using System.Windows.Data;
using DAL;
using Metier;

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

    private void AjouterEntree_Click(object sender, RoutedEventArgs e)
    {
        var win = new FenetreAjoutEntree { Owner = this };
        if (win.ShowDialog() == true && !string.IsNullOrWhiteSpace(TxtIdAnimal.Text))
            Entrees_Click(null!, null!);
    }

    private void AjouterSortie_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var win = new FenetreAjoutSortie { Owner = this };
            if (win.ShowDialog() == true && !string.IsNullOrWhiteSpace(TxtIdAnimal.Text))
                Sorties_Click(null!, null!);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Fermer_Click(object sender, RoutedEventArgs e) => Close();
}

public sealed class EntreeSortieDisplayConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Entree e)
            return $"{e.DateEntree:dd/MM/yyyy} - {e.Raison}\nContact: {e.Contact?.Prenom} {e.Contact?.Nom}".TrimEnd();
        if (value is Sortie s)
            return $"{s.DateSortie:dd/MM/yyyy} - {s.Raison}\nContact: {s.Contact?.Prenom} {s.Contact?.Nom}".TrimEnd();
        return value?.ToString() ?? "";
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
