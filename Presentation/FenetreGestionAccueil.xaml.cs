using System.Globalization;
using System.Windows;
using System.Windows.Data;
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

    private void Ajouter_Click(object sender, RoutedEventArgs e)
    {
        var win = new FenetreAjoutAccueil { Owner = this };
        if (win.ShowDialog() == true)
        {
            if (!string.IsNullOrWhiteSpace(TxtIdAnimal.Text)) ListerParAnimal_Click(null!, null!);
            else if (int.TryParse(TxtIdContact.Text?.Trim(), out int id)) { TxtIdContact.Text = id.ToString(); ListerParFamille_Click(null!, null!); }
        }
    }

    private void Rafraichir_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(TxtIdAnimal.Text)) ListerParAnimal_Click(null!, null!);
        else if (int.TryParse(TxtIdContact.Text?.Trim(), out int id)) { TxtIdContact.Text = id.ToString(); ListerParFamille_Click(null!, null!); }
        else ListeAccueils.ItemsSource = null;
    }

    private void Fermer_Click(object sender, RoutedEventArgs e) => Close();
}

public sealed class NullableDateToEnCoursConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is DateTime d ? d.ToString("dd/MM/yyyy", culture) : "En cours";
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
