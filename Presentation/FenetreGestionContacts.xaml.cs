using System.Collections.Generic;
using System.Windows;
using DAL;
using Metier;

namespace Presentation;

public partial class FenetreGestionContacts : Window
{
    public FenetreGestionContacts()
    {
        InitializeComponent();
        Charger();
    }

    private void Charger()
    {
        try
        {
            List<Contact> contacts = ContactDAO.ListerTous();
            ListeContacts.ItemsSource = contacts;
        }
        catch (System.Exception ex)
        {
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ListeContacts_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) { }

    private void Ajouter_Click(object sender, RoutedEventArgs e)
    {
        var fenetre = new FenetreAjoutContact { Owner = this };
        if (fenetre.ShowDialog() == true)
            Charger();
    }

    private void Consulter_Click(object sender, RoutedEventArgs e)
    {
        var c = ListeContacts.SelectedItem as Contact;
        if (c == null) { MessageBox.Show("Sélectionnez un contact.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        var contact = ContactDAO.Consulter(c.Identifiant);
        if (contact == null) return;
        string msg = $"ID: {contact.Identifiant}\nNom: {contact.Nom} {contact.Prenom}\nAdresse: {contact.AdresseContact.Rue}, {contact.AdresseContact.Cp} {contact.AdresseContact.Localite}\nRN: {contact.RegistreNational}\nGSM: {contact.Gsm ?? "-"}\nTél: {contact.Telephone ?? "-"}\nEmail: {contact.Email ?? "-"}";
        MessageBox.Show(msg, "Détail contact", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void Modifier_Click(object sender, RoutedEventArgs e)
    {
        var c = ListeContacts.SelectedItem as Contact;
        if (c == null) { MessageBox.Show("Sélectionnez un contact à modifier.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        var fenetre = new FenetreAjoutContact(c) { Owner = this };
        if (fenetre.ShowDialog() == true)
            Charger();
    }

    private void Supprimer_Click(object sender, RoutedEventArgs e)
    {
        var c = ListeContacts.SelectedItem as Contact;
        if (c == null) { MessageBox.Show("Sélectionnez un contact à supprimer.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        if (MessageBox.Show($"Supprimer le contact {c.Prenom} {c.Nom} (ID: {c.Identifiant}) ?", "Confirmer la suppression", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            return;
        try
        {
            ContactDAO.Supprimer(c.Identifiant);
            MessageBox.Show("Contact supprimé.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            Charger();
        }
        catch (System.Exception ex)
        {
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Fermer_Click(object sender, RoutedEventArgs e) => Close();
}
