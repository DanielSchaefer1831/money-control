using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace MoneyControl
{
    // Der Bauplan für eine einzelne Ausgabe: Definiert alle Infos, die in der Datenbank gespeichert werden
    public class Haushaltsausgaben
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Username { get; set; } // Wer hat es ausgegeben?

        public double Amount { get; set; } // Wie viel?

        public string Description { get; set; } // Was genau?

        public string Category { get; set; } // Das Emoji + Name

        public DateTime Date { get; set; } // Wann?
    }
}
