using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace MoneyControl
{
    public class DatabaseService
    {
        // Die Verbindung zur Datenbank:
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            string dbPatch = Path.Combine(FileSystem.AppDataDirectory, "MoneyControl.db3"); // Wo die Datenbank-Datei auf dem Handy gespeichert wird:

            _database = new SQLiteAsyncConnection(dbPatch); // Verbindung herstellen:

            _database.CreateTableAsync<User>().Wait(); // Tabelle 'User' erstellen, falls sie noch nicht existiert. + Es passiert im Hintergrund (Async):
            _database.CreateTableAsync<Haushaltsausgaben>().Wait(); // Tabelle 'Haushaltsausgaben' erstellen, falss sie noch nicht existiert. + Es passiert im Hintergrund (Async):
        }



        // Methode: Einen neuen User in die Datenbank schreiben
        public Task<int> SaveUserAsync(User user) 
        {
            return _database.InsertAsync(user);
        }



        // Methode: Einen User suchen
        public Task<User> GetUserAsync(string username)
        {
            return _database.Table<User>()
                            .Where(u => u.Username == username)
                            .FirstOrDefaultAsync();
        }



        // Methode: Benutzeraccount löschen:
        public Task<int> DeleteUserAsync(string username)
        {          
            return _database.Table<User>().Where(u => u.Username == username).DeleteAsync();
        }



        // Diese Methode holt ALLE User aus der Tabelle
        //public Task<List<User>> GetUsersAsync()
        //{
        //return _database.Table<User>().ToListAsync();
        //}


        // Methode: Schreibt eine neue Ausgabe dauerhaft in die SQLite-Datenbank
        public Task<int> SaveAusgabeAsync(Haushaltsausgaben ausgabe)
        {
            return _database.InsertAsync(ausgabe);
        }



        // Methode: Holt alle Ausgaben aus der Datenbank, die zu einem bestimmten Benutzer gehören
        public Task<List<Haushaltsausgaben>> GetAusgabenByUserAsync(string username)
        {
            return _database.Table<Haushaltsausgaben>()
                            .Where(a => a.Username == username)
                            .ToListAsync();
        }



        // Entfernt eine bestimmte Ausgabe unwiderruflich aus der Datenbank
        public Task<int> DeleteAusgabeAsync(Haushaltsausgaben ausgabe)
        {
            return _database.DeleteAsync(ausgabe);
        }
    }
}
