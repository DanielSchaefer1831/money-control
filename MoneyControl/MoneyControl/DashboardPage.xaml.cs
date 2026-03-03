namespace MoneyControl;

public partial class DashboardPage : ContentPage
{
	public DashboardPage(string userName)
	{
		InitializeComponent();

		lblHelloUsername.Text = userName;

        _ = UpdateTotalExpenses();
    }

	
    
    // Methode: Abmeldefunktion
	private async void OnLogoutClicked(object sender, EventArgs e)
	{
		bool answer = await DisplayAlert("Abmelden", "Möchtest du dich wirklick abmelden?", "Ja", "Nein");

		if(answer)
		{
			App.Current.MainPage = new NavigationPage(new MainPage());
		}
	}



    // Methode: Benutzeraccount löschen
	private async void OnDeleteAccountClicked(object sender, EventArgs e)
	{
		bool confirm = await DisplayAlert("Achtung!", "Willst du dein Konto wirklick löschen?", "Ja", "Abbrechen");

		if(confirm)
		{
			string currentUser = lblHelloUsername.Text;

			await App.Database.DeleteUserAsync(currentUser);

			await DisplayAlert("Gelöscht!", "Dein Account wurde erfolgreich entfernt.", "OK");
			App.Current.MainPage = new NavigationPage(new MainPage());
		}
	}



    // Methode: Ausgabe einfügen
    private async void OnAddEntryClicked(object sender, EventArgs e)
    {
        // 1. Sicherheitsschranke: Ist überhaupt ein Betrag eingegeben?
        if (string.IsNullOrWhiteSpace(entAmount.Text))
        {
            await DisplayAlert("Fehler", "Bitte gib einen Betrag ein!", "OK");
            return;
        }

        // 2. Kategorie prüfen: Wurde etwas im Picker ausgewählt?
        if (picCategory.SelectedItem == null)
        {
            await DisplayAlert("Fehler", "Bitte wähle eine Kategorie aus!", "OK");
            return;
        }

        try
        {
            // 3. Die Daten in "Haushaltsausgaben" packen
            var neueAusgabe = new Haushaltsausgaben
            {
                Amount = double.Parse(entAmount.Text), // Verwandelt Text in eine Zahl
                Description = entDescription.Text ?? "Keine Beschreibung",
                Category = picCategory.SelectedItem.ToString(),
                Username = lblHelloUsername.Text, // Den Namen oben aus dem Label nutzen
                Date = DateTime.Now
            };

            // 4. In die Datenbank einfügen         
            await App.Database.SaveAusgabeAsync(neueAusgabe);

            await UpdateTotalExpenses();

            // 5. Erfolg melden und Felder leeren
            await DisplayAlert("Gespeichert", $"{neueAusgabe.Amount} € für {neueAusgabe.Category} wurde gebucht.", "Top!");

            entAmount.Text = string.Empty;
            entDescription.Text = string.Empty;
            picCategory.SelectedItem = null;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Fehler", "Da lief was schief beim Speichern: " + ex.Message, "OK");
        }
    }



    // Methode: Ausgaben neu laden, sortieren und die Gesamtsumme berechnen:
    private async Task UpdateTotalExpenses()
    {
        // 1. Alle Ausgaben laden
        var alleAusgaben = await App.Database.GetAusgabenByUserAsync(lblHelloUsername.Text);

        // 2. Die Liste (CollectionView) aktualisieren
        lstExpenses.ItemsSource = alleAusgaben.AsEnumerable().Reverse().ToList();

        // 3. Summe berechnen
        double gesamtSumme = 0;
        foreach (var ausgabe in alleAusgaben)
        {
            gesamtSumme += ausgabe.Amount;
        }

        // 4. Anzeige aktualisieren
        lblTotalExpenses.Text = gesamtSumme.ToString("F2") + " €";
    }



    // Methode: Ausgaben löschen
    private async void OnDeleteExpenseInvoked(object sender, EventArgs e)
    {        
        var swipeItem = (SwipeItem)sender;
        var ausgabe = (Haushaltsausgaben)swipeItem.CommandParameter;

        // Kurze Rückfrage, ob man wirklich löschen will
        bool answer = await DisplayAlert("Löschen?", "Möchtest du diese Ausgabe wirklich entfernen?", "Ja", "Nein");

        if (answer)
        {
            // Aus der Datenbank löschen
            await App.Database.DeleteAusgabeAsync(ausgabe);

            // Liste und Summe auf der Seite aktualisieren
            await UpdateTotalExpenses();
        }
    }

}