// Plik: Firma.Data/Data/Movie/Gatunek.cs

using System.ComponentModel.DataAnnotations;

namespace Firma.Data.Data.Movie
{
    public class Gatunek
    {
        [Key]
        public int IdGatunek { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        // Właściwość nawigacyjna do filmów - druga strona relacji M:N
        public ICollection<Film> Filmy { get; set; } = new List<Film>();
    }
}