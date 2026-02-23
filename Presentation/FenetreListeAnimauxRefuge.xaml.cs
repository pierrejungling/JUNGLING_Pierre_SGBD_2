using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using DAL;
using Metier;

namespace Presentation;

public partial class FenetreListeAnimauxRefuge : Window
{
    public FenetreListeAnimauxRefuge()
    {
        InitializeComponent();
        Charger();
    }

    private void Charger()
    {
        try
        {
            List<Animal> animaux = AnimalDAO.ListerAnimauxAuRefuge();
            ListeAnimaux.ItemsSource = animaux;
        }
        catch (System.Exception ex)
        {
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Rafraichir_Click(object sender, RoutedEventArgs e) => Charger();
}
