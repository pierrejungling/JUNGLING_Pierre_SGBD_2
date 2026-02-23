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

    private void Rafraichir_Click(object sender, RoutedEventArgs e) => Charger();

    private void Consulter_Click(object sender, RoutedEventArgs e)
    {
        var c = ListeContacts.SelectedItem as Contact;
        if (c == null) { MessageBox.Show("Sélectionnez un contact.", "Info", MessageBoxButton.OK, MessageBoxImage.Information); return; }
        var contact = ContactDAO.Consulter(c.Identifiant);
        if (contact == null) return;
        string msg = $"ID: {contact.Identifiant}\nNom: {contact.Nom} {contact.Prenom}\nAdresse: {contact.AdresseContact.Rue}, {contact.AdresseContact.Cp} {contact.AdresseContact.Localite}\nRN: {contact.RegistreNational}\nGSM: {contact.Gsm ?? "-"}\nTél: {contact.Telephone ?? "-"}\nEmail: {contact.Email ?? "-"}";
        MessageBox.Show(msg, "Détail contact", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void Fermer_Click(object sender, RoutedEventArgs e) => Close();
}
