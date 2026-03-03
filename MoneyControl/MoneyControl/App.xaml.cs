namespace MoneyControl
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent(); // Initialisiert die Benutzeroberfläche und verknüpft XAML mit C#

            MainPage = new AppShell(); // Legt die Startseite der App fest (in diesem Fall die AppShell für Navigation)
        }



        // Speicherplatz für die Datenbankverbindung:
        private static DatabaseService _database; 
        
        // Das Tor zur Datenbank:
        public static DatabaseService Database 
        {
            get
            {
                if (_database == null) // Verbindung wird erst erstellt, wenn sie zum ersten Mal gebraucht wird.
                {
                    _database = new DatabaseService();
                }
                return _database; // Gibt die (bestehende oder neu erstellte) Verbindung zurück.
            }
        }      
    }
}
