// Firma.Data/Data/Movie/Aktor.cs
using System.ComponentModel.DataAnnotations;

namespace Firma.Data.Data.Movie
{
    public class Aktor
    {
        [Key]
        public int IdAktor { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Imie { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Nazwisko { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DataUrodzenia { get; set; }

        public string? ZdjecieUrl { get; set; }

        // Właściwość nawigacyjna do filmów - relacja M:N
        public ICollection<Film> Filmy { get; set; } = new List<Film>();
    }
}