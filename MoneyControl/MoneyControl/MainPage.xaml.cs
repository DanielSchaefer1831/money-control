namespace MoneyControl
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Entfernt die standardmäßige Android-Unterlinie bei allen Entry-Feldern:
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
                #if ANDROID
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
                #endif
            });
        }



        // Methode: Schaltet die Sichtbarkeit des Passworts zwischen Text und Punkten um:
        private void PasswordImage(object sender, EventArgs e)
        {
            entPassword.IsPassword = !entPassword.IsPassword;

            if(entPassword.IsPassword)
            {
                btnImageChangePassword.Source = "ausblenden.png";
            }
            else
            {
                btnImageChangePassword.Source = "show.png";
            }
        }



        // Navigiert den Benutzer zur Registrierungsseite:
        private async void RegisterButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AccRegistrieren());
        }



        // Methode: Überprüft die Login-Daten und leitet den Benutzer zum Dashboard weiter
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = entUsername.Text;
            string password = entPassword.Text;

            var user = await App.Database.GetUserAsync(username); // In der Datenbank nach dem User suchen:

            // Prüfung: Existiert der User und stimmt das Passwort?
            if (user != null && user.Password == password)
            {
                // Das hier macht das Dashboard zur neuen Hauptseite -> Pfeil verschwindet
                App.Current.MainPage = new NavigationPage(new DashboardPage(user.Username));
            }
            else
            {
                await DisplayAlert("Fehler", "Benutzername oder Passwort falsch.", "OK");
            }



            // Alle User werden angezeigt:
            //var allUsers = await App.Database.GetUsersAsync();
            //foreach (var u in allUsers)
            //{
            //Console.WriteLine($"GEFUNDEN IN DATENBANK: Name={u.Username}, Passwort={u.Password}");
            //}
        }
    }
}
