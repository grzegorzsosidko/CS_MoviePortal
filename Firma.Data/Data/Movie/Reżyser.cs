// Firma.Data/Data/Movie/Reżyser.cs
using System.ComponentModel.DataAnnotations;

namespace Firma.Data.Data.Movie
{
    public class Reżyser
    {
        [Key]
        public int IdReżyser { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Imie { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Nazwisko { get; set; }

        // Właściwość nawigacyjna - jeden reżyser może mieć wiele filmów
        public ICollection<Film> Filmy { get; set; } = new List<Film>();
    }
}