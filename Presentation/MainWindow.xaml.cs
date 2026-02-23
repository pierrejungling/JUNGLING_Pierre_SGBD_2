using System.Windows;
using System.Windows.Controls;

namespace Presentation;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void MenuGestionAnimaux_Click(object sender, RoutedEventArgs e) => OuvrirFenetre(new FenetreGestionAnimaux());
    private void MenuGestionContacts_Click(object sender, RoutedEventArgs e) => OuvrirFenetre(new FenetreGestionContacts());
    private void MenuGestionAdoptions_Click(object sender, RoutedEventArgs e) => OuvrirFenetre(new FenetreGestionAdoptions());
    private void MenuGestionAccueil_Click(object sender, RoutedEventArgs e) => OuvrirFenetre(new FenetreGestionAccueil());
    private void MenuGestionVaccinations_Click(object sender, RoutedEventArgs e) => OuvrirFenetre(new FenetreGestionVaccinations());
    private void MenuGestionEntreeSortie_Click(object sender, RoutedEventArgs e) => OuvrirFenetre(new FenetreGestionEntreeSortie());
    private void MenuListerAnimauxRefuge_Click(object sender, RoutedEventArgs e) => OuvrirFenetre(new FenetreListeAnimauxRefuge());

    private void Quitter_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

    private void OuvrirFenetre(Window fenetre)
    {
        fenetre.Owner = this;
        fenetre.ShowDialog();
    }
}
