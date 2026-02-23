using System.Collections.Generic;
using System.Windows;
using DAL;
using Metier;

namespace Presentation;

public partial class FenetreGestionAdoptions : Window
{
    public FenetreGestionAdoptions()
    {
        InitializeComponent();
        Charger();
    }

    private void Charger()
    {
        try
        {
            var adoptions = AdoptionDAO.ListerToutes();
            ListeAdoptions.ItemsSource = adoptions;
        }
        catch (System.Exception ex)
        {
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ListeAdoptions_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) { }

    private void Rafraichir_Click(object sender, RoutedEventArgs e) => Charger();

    private void VoirDetail_Click(object sender, RoutedEventArgs e)
    {
        var a = ListeAdoptions.SelectedItem as Adoption;
        if (a == null) { MessageBox.Show("Sélectionnez une adoption.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        string msg = $"Animal: {a.AnimalAdopte.Nom} ({a.AnimalAdopte.Identifiant})\nAdoptant: {a.Adoptant.Nom} {a.Adoptant.Prenom}\nStatut: {a.Statut}\nDate demande: {a.DateDemande:dd/MM/yyyy}";
        MessageBox.Show(msg, "Détail adoption", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void Fermer_Click(object sender, RoutedEventArgs e) => Close();
}
