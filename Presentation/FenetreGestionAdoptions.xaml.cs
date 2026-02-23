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

    private void Ajouter_Click(object sender, RoutedEventArgs e)
    {
        var win = new FenetreAjoutAdoption { Owner = this };
        if (win.ShowDialog() == true)
            Charger();
    }

    private void VoirDetail_Click(object sender, RoutedEventArgs e)
    {
        var a = ListeAdoptions.SelectedItem as Adoption;
        if (a == null) { MessageBox.Show("Sélectionnez une adoption.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        string msg = $"Animal: {a.AnimalAdopte.Nom} ({a.AnimalAdopte.Identifiant})\nAdoptant: {a.Adoptant.Nom} {a.Adoptant.Prenom}\nStatut: {a.Statut}\nDate demande: {a.DateDemande:dd/MM/yyyy}";
        MessageBox.Show(msg, "Détail adoption", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void ModifierStatut_Click(object sender, RoutedEventArgs e)
    {
        var a = ListeAdoptions.SelectedItem as Adoption;
        if (a == null) { MessageBox.Show("Sélectionnez une adoption.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        var statuts = new[] { "demande", "acceptee", "rejet_environnement", "rejet_comportement" };
        var win = new Window
        {
            Title = "Modifier le statut",
            Width = 300,
            Height = 200,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Owner = this,
            SizeToContent = SizeToContent.Manual
        };
        var sp = new System.Windows.Controls.StackPanel { Margin = new Thickness(15, 15, 15, 15) };
        sp.Children.Add(new System.Windows.Controls.TextBlock { Text = "Nouveau statut:" });
        var cb = new System.Windows.Controls.ComboBox { Margin = new Thickness(0, 6, 0, 16), MinWidth = 200 };
        foreach (var s in statuts) cb.Items.Add(s);
        cb.SelectedItem = a.Statut;
        sp.Children.Add(cb);
        var btnOk = new System.Windows.Controls.Button { Content = "Enregistrer", Padding = new Thickness(12, 4, 12, 4), Margin = new Thickness(0, 0, 8, 0), MinWidth = 90 };
        var btnAnnuler = new System.Windows.Controls.Button { Content = "Annuler", Padding = new Thickness(12, 4, 12, 4), MinWidth = 90 };
        var hp = new System.Windows.Controls.StackPanel { Orientation = System.Windows.Controls.Orientation.Horizontal, HorizontalAlignment = System.Windows.HorizontalAlignment.Right };
        hp.Children.Add(btnOk);
        hp.Children.Add(btnAnnuler);
        sp.Children.Add(hp);
        win.Content = sp;
        btnOk.Click += (_, __) => { win.DialogResult = true; win.Close(); };
        btnAnnuler.Click += (_, __) => { win.DialogResult = false; win.Close(); };
        if (win.ShowDialog() == true && cb.SelectedItem is string nouveauStatut)
        {
            try
            {
                AdoptionDAO.ModifierStatut(a.AnimalAdopte.Identifiant, nouveauStatut);
                MessageBox.Show("Statut mis à jour.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                Charger();
            }
            catch (System.Exception ex) { MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
    }

    private void Supprimer_Click(object sender, RoutedEventArgs e)
    {
        var a = ListeAdoptions.SelectedItem as Adoption;
        if (a == null) { MessageBox.Show("Sélectionnez une adoption à supprimer.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        if (MessageBox.Show(
                $"Supprimer l'adoption de {a.AnimalAdopte.Nom} par {a.Adoptant.Prenom} {a.Adoptant.Nom} (demande du {a.DateDemande:dd/MM/yyyy}) ?",
                "Confirmer la suppression",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) != MessageBoxResult.Yes)
            return;
        try
        {
            AdoptionDAO.Supprimer(a.AnimalAdopte.Identifiant, a.Adoptant.Identifiant, a.DateDemande);
            MessageBox.Show("Adoption supprimée.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            Charger();
        }
        catch (System.Exception ex) { MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void Fermer_Click(object sender, RoutedEventArgs e) => Close();
}
