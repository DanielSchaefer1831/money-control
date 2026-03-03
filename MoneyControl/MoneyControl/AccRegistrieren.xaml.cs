using MoneyControl;
namespace MoneyControl;

public partial class AccRegistrieren : ContentPage
{
	public AccRegistrieren()
	{
		InitializeComponent();
	}

    
    
    // Methode für das erste Passwort-Auge auf der Registrierungsseite:
    private void PasswordImage(object sender, EventArgs e)
    {
        entPasswordRegister.IsPassword = !entPasswordRegister.IsPassword;

        if (entPasswordRegister.IsPassword)
        {
            btnImageChangePasswordRegister.Source = "ausblenden.png";
        }
        else
        {
            btnImageChangePasswordRegister.Source = "show.png";
        }
    }

    
    // Methode für das zweite Passwort-Auge auf der Registrierungsseite:
    private void ConfirmPasswordImage(object sender, EventArgs e)
    {
        entConfirmPasswordRegister.IsPassword = !entConfirmPasswordRegister.IsPassword;
        
        if(entConfirmPasswordRegister.IsPassword)
        {
            btnImageChangeConfirmPasswordRegister.Source = "ausblenden.png";
        }
        else
        {
            btnImageChangeConfirmPasswordRegister.Source = "show.png";
        }
    }


    private async void OnConfirmRegistrationClicked(object sender, EventArgs e)
    {
        // Daten aus Eingabefeldern holen:
        string username = entUsernameRegister.Text;
        string password = entPasswordRegister.Text;
        string confirmPassword = entConfirmPasswordRegister.Text;

        // Validierung: Sind die Felder leer?
        if(string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Fehler", "Bitte füllte alle Felder aus!", "OK");
            return;
        }

        // Validierung: Sind die Passwörter gleich?
        if(password != confirmPassword)
        {
            await DisplayAlert("Fehler", "Die Passwörter stimmen nicht überein!", "OK");
            return;
        }

        try
        {
            // Ein neues User-Objekt erstellen:
            var newUser = new User
            {
                Username = username,
                Password = password
            };

            await App.Database.SaveUserAsync(newUser); // In die Datenbank speichern:
            await DisplayAlert("Erfolg", "Konto wurde erstellt!", "Zum Login"); // Erfolg melden:
            await Navigation.PopAsync(); // Zurück zur Login-Seite springen:
        }
        catch (Exception ex)
        {
            await DisplayAlert("Fehler", "Benutzername existiert bereits.", "OK"); // Falls der Name schon existiert (wegen [Unique])
        }
    }
}