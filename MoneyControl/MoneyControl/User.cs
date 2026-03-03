using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace MoneyControl
{
    // Das Benutzermodell: Speichert Login-Daten und stellt durch [Unique] sicher, dass jeder Name nur einmal existiert.
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique] // Sperre! Sorgt dafür, dass der Wert  in dieser Spalte nur ein einziges Mal vorkommt:
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
