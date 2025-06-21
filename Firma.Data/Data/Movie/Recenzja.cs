// Firma.Data/Data/Movie/Recenzja.cs
using Firma.Data.Data.Customers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Firma.Data.Data.Movie
{
    public class Recenzja
    {
        [Key]
        public int IdRecenzja { get; set; }

        [Required]
        [Range(1, 10)]
        public int Ocena { get; set; } // Ocena w skali 1-10

        [MaxLength(2000)]
        public string? Tresc { get; set; }

        public DateTime DataDodania { get; set; } = DateTime.Now;

        // Klucz obcy i właściwość nawigacyjna do Filmu
        public int IdFilm { get; set; }
        [ForeignKey("IdFilm")]
        public Film Film { get; set; } = null!;

        // Klucz obcy i właściwość nawigacyjna do Użytkownika
        public int IdUser { get; set; }
        [ForeignKey("IdUser")]
        public User User { get; set; } = null!;
    }
}