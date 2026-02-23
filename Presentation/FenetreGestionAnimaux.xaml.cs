using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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
        MessageBox.Show("Pour ajouter un animal avec entrée au refuge, utilisez les procédures stockées (animal_inserer puis entree_ajouter) ou une fenêtre dédiée. Ici : rafraîchissez après ajout en base.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        Charger();
    }

    private void Consulter_Click(object sender, RoutedEventArgs e)
    {
        var animal = ListeAnimaux.SelectedItem as Animal;
        if (animal == null) { MessageBox.Show("Sélectionnez un animal.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        var a = AnimalDAO.Consulter(animal.Identifiant);
        if (a == null) return;
        string msg = $"ID: {a.Identifiant}\nNom: {a.Nom}\nType: {a.Type}\nSexe: {a.Sexe}\nNaissance: {a.DateNaissance:dd/MM/yyyy}\nStérilisé: {a.Sterilise}\nDescription: {a.Description ?? "-"}\nParticularités: {a.Particularite ?? "-"}\nCouleurs: {string.Join(", ", a.Couleurs)}";
        MessageBox.Show(msg, "Détail animal", MessageBoxButton.OK, MessageBoxImage.Information);
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
