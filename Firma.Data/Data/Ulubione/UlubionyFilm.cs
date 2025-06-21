using Firma.Data.Data.CMS; 
using Firma.Data.Data.Customers; 
using Firma.Data.Data.Movie;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Firma.Data.Data.Ulubione
{
    // To jest moja encja, która będzie reprezentować polubienie filmu przez użytkownika.
    // Łączy ona moją encję User z encją Film.
    public class UlubionyFilm
    {
        [Key]
        public int Id { get; set; }

        // Klucz obcy do mojej tabeli użytkowników.
        // Używam typu 'int', ponieważ IdUser w klasie User jest typu 'int'.
        public int IdUzytkownika { get; set; }

        // Tworzę właściwość nawigacyjną do mojej własnej klasy User.
        [ForeignKey("IdUzytkownika")]
        public virtual User Uzytkownik { get; set; }

        public int IdFilmu { get; set; }

        [ForeignKey("IdFilmu")]
        public virtual Film Film { get; set; }

        public DateTime DataDodania { get; set; }
    }
}