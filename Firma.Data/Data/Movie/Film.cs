using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Firma.Data.Data.Movie
{
    public class Film
    {
        [Key]
        public int IdFilm { get; set; }

        [Required(ErrorMessage = "Tytuł jest wymagany")]
        [MaxLength(100)]
        public required string Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public int ReleaseYear { get; set; }

        public int IdReżyser { get; set; }

        [ValidateNever] // Mówie walidacji, żeby ignorowała tę właściwość podczas sprawdzania
        [ForeignKey("IdReżyser")]
        public Reżyser Reżyser { get; set; } = null!;

        // Zapisuje czas trwania w minutach
        public int DurationInMinutes { get; set; }

        // URL do plakatu filmu
        public string? PosterUrl { get; set; }

        // Właściwość nawigacyjna do gatunków - klucz do relacji M:N
        [ValidateNever]
        public ICollection<Gatunek> Gatunki { get; set; } = new List<Gatunek>();
        [ValidateNever]
        public ICollection<Aktor> Aktorzy { get; set; } = new List<Aktor>();
        [ValidateNever]
        public ICollection<Recenzja> Recenzje { get; set; } = new List<Recenzja>();
    }
}