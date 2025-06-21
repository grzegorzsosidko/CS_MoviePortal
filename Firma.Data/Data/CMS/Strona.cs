using System.ComponentModel.DataAnnotations;

namespace Firma.Data.Data.CMS
{
    public class Strona
    {
        [Key]
        public int IdStrony { get; set; }

        [Required(ErrorMessage = "Tytuł jest wymagany")]
        [MaxLength(30)]
        public required string Tytul { get; set; }

        [Required(ErrorMessage = "Treść jest wymagana")]
        public required string Tresc { get; set; }

        [Required(ErrorMessage = "Link (klucz URL) jest wymagany")]
        [MaxLength(30)]
        public required string Link { get; set; }

        public int Pozycja { get; set; } // Pozycja do sortowania

        // NOWE POLA:
        [Display(Name = "Pokaż w menu nawigacyjnym")]
        public bool PokazWNavbar { get; set; } = false;

        [Display(Name = "Kategoria w stopce")]
        public KategoriaStopki? KategoriaStopki { get; set; } // Znak '?' oznacza, że pole jest opcjonalne (nullable)
    }
}