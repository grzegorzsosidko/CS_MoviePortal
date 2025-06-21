using System.ComponentModel.DataAnnotations;

namespace Firma.Data.Data.AppSettings
{
    public class Ustawienie
    {
        [Key]
        public int IdUstawienia { get; set; }

        // Klucz jest unikalnym identyfikatorem ustawienia, np. "WelcomeHeadline"
        [Required]
        [MaxLength(50)]
        public required string Klucz { get; set; }

        // Wartość to tekst, który chce wyświetlić
        [Required]
        [MaxLength(1000)]
        public required string Wartosc { get; set; }
    }
}