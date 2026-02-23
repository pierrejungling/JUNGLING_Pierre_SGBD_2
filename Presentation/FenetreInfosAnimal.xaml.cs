using System.Linq;
using System.Windows;
using DAL;
using Metier;

namespace Presentation;

public partial class FenetreInfosAnimal : Window
{
    private Animal _animal;
    private static readonly string[] TypesCompatibilite = { "chat", "chien", "jeune enfant", "enfant", "jardin", "poney" };

    public FenetreInfosAnimal(Animal animal)
    {
        InitializeComponent();
        _animal = animal;
        Charger();
    }

    private void Charger()
    {
        var a = AnimalDAO.Consulter(_animal.Identifiant);
        if (a == null) return;
        _animal = a;
        TxtTitre.Text = $"{a.Nom} ({a.Identifiant})";
        TbDescription.Text = a.Description ?? "";
        TbParticularite.Text = a.Particularite ?? "";
        ListeCouleurs.ItemsSource = null;
        ListeCouleurs.ItemsSource = _animal.Couleurs.ToList();
        ListeCompatibilites.ItemsSource = null;
        ListeCompatibilites.ItemsSource = _animal.Compatibilites.Select(c => c.ToString()).ToList();

        CbCouleur.ItemsSource = CouleurDAO.ListerToutes();
        if (CbCouleur.Items.Count > 0) CbCouleur.SelectedIndex = 0;
        CbTypeCompat.ItemsSource = TypesCompatibilite;
        if (CbTypeCompat.Items.Count > 0) CbTypeCompat.SelectedIndex = 0;
    }

    private void EnregistrerDescription_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            AnimalDAO.ModifierDescription(_animal.Identifiant, string.IsNullOrWhiteSpace(TbDescription.Text) ? null : TbDescription.Text.Trim());
            MessageBox.Show("Description enregistrée.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            Charger();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void EnregistrerParticularite_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            AnimalDAO.ModifierParticularites(_animal.Identifiant, string.IsNullOrWhiteSpace(TbParticularite.Text) ? null : TbParticularite.Text.Trim());
            MessageBox.Show("Particularités enregistrées.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            Charger();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void AjouterCouleur_Click(object sender, RoutedEventArgs e)
    {
        var nom = CbCouleur.SelectedItem?.ToString() ?? CbCouleur.Text?.Trim();
        if (string.IsNullOrWhiteSpace(nom)) { MessageBox.Show("Sélectionnez ou saisissez une couleur.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        try
        {
            AnimalDAO.AjouterCouleurPourAnimal(_animal.Identifiant, nom);
            Charger();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void SupprimerCouleur_Click(object sender, RoutedEventArgs e)
    {
        var sel = ListeCouleurs.SelectedItem?.ToString();
        if (string.IsNullOrEmpty(sel)) { MessageBox.Show("Sélectionnez une couleur à supprimer.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        try
        {
            AnimalDAO.SupprimerCouleur(_animal.Identifiant, sel);
            Charger();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void AjouterCompatibilite_Click(object sender, RoutedEventArgs e)
    {
        var type = CbTypeCompat.SelectedItem?.ToString();
        if (string.IsNullOrEmpty(type)) { MessageBox.Show("Sélectionnez un type de compatibilité.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        if (_animal.Compatibilites.Any(c => c.Type == type)) { MessageBox.Show("Cette compatibilité existe déjà pour cet animal.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        var desc = (TbCompatDesc.Text ?? "").Trim();
        var comp = new Compatibilite(type, CkCompatValeur.IsChecked == true, string.IsNullOrWhiteSpace(desc) ? null : desc);
        try
        {
            AnimalDAO.AjouterCompatibilitePourAnimal(_animal.Identifiant, comp);
            Charger();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void SupprimerCompatibilite_Click(object sender, RoutedEventArgs e)
    {
        var idx = ListeCompatibilites.SelectedIndex;
        if (idx < 0 || idx >= _animal.Compatibilites.Count) { MessageBox.Show("Sélectionnez une compatibilité à supprimer.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        var comp = _animal.Compatibilites[idx];
        try
        {
            AnimalDAO.SupprimerCompatibilite(_animal.Identifiant, comp.Type);
            Charger();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void Valider_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }
}
