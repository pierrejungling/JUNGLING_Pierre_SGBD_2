using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DAL;
using Metier;

namespace Presentation;

public partial class FenetreGestionAnimaux : Window
{
    public FenetreGestionAnimaux()
    {
        InitializeComponent();
        Charger();
    }

    private void Charger()
    {
        try
        {
            var animaux = AnimalDAO.ListerTous();
            ListeAnimaux.ItemsSource = animaux;
        }
        catch (System.Exception ex)
        {
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ListeAnimaux_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

    private void Ajouter_Click(object sender, RoutedEventArgs e)
    {
        var fenetre = new FenetreAjoutAnimal { Owner = this };
        if (fenetre.ShowDialog() == true)
            Charger();
    }

    private void Consulter_Click(object sender, RoutedEventArgs e)
    {
        var animal = ListeAnimaux.SelectedItem as Animal;
        if (animal == null) { MessageBox.Show("Sélectionnez un animal.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        var a = AnimalDAO.Consulter(animal.Identifiant);
        if (a == null) return;
        var lignes = new List<string>
        {
            $"ID: {a.Identifiant}",
            $"Nom: {a.Nom}",
            $"Type: {a.Type}",
            $"Sexe: {a.Sexe}",
            $"Date de naissance: {a.DateNaissance:dd/MM/yyyy}",
            $"Stérilisé: {(a.Sterilise ? "Oui" : "Non")}",
            a.DateSterilisation.HasValue ? $"Date stérilisation: {a.DateSterilisation:dd/MM/yyyy}" : null,
            a.DateDeces.HasValue ? $"Date de décès: {a.DateDeces:dd/MM/yyyy}" : null,
            $"Description: {a.Description ?? "-"}",
            $"Particularités: {a.Particularite ?? "-"}",
            $"Couleurs: {(a.Couleurs.Count > 0 ? string.Join(", ", a.Couleurs) : "-")}",
            "Compatibilités: " + (a.Compatibilites.Count > 0
                ? string.Join(" ; ", a.Compatibilites.Select(c => $"{c.Type}: {(c.Valeur ? "Oui" : "Non")}" + (string.IsNullOrEmpty(c.Description) ? "" : $" ({c.Description})")))
                : "-")
        };
        string msg = string.Join("\n", lignes.Where(s => s != null));
        MessageBox.Show(msg, "Détail animal", MessageBoxButton.OK, MessageBoxImage.Information);
        Charger();
    }

    private void ModifierInfos_Click(object sender, RoutedEventArgs e)
    {
        var animal = ListeAnimaux.SelectedItem as Animal;
        if (animal == null) { MessageBox.Show("Sélectionnez un animal.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        var win = new FenetreInfosAnimal(animal) { Owner = this };
        win.ShowDialog();
        Charger();
    }

    private void Supprimer_Click(object sender, RoutedEventArgs e)
    {
        var animal = ListeAnimaux.SelectedItem as Animal;
        if (animal == null) { MessageBox.Show("Sélectionnez un animal.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        if (MessageBox.Show($"Supprimer l'animal {animal.Nom} ?", "Confirmer", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
        try
        {
            AnimalDAO.Supprimer(animal.Identifiant);
            MessageBox.Show("Animal supprimé.");
            Charger();
        }
        catch (System.Exception ex) { MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void Fermer_Click(object sender, RoutedEventArgs e) => Close();
}

public sealed class BoolToOuiNonConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is true ? "Oui" : "Non";
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
