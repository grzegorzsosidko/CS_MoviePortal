// Plik: Firma.Intranet/Models/ViewModels/FilmViewModel.cs
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Firma.Intranet.Models.ViewModels
{
    public class FilmViewModel
    {
        // Ta część przechowuje dane samego filmu
        public int IdFilm { get; set; }

        [Required(ErrorMessage = "Tytuł jest wymagany")]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public int ReleaseYear { get; set; }

        [Required(ErrorMessage = "Reżyser jest wymagany")]
        [Range(1, int.MaxValue, ErrorMessage = "Musisz wybrać reżysera z listy.")]
        public int IdReżyser { get; set; } // Tylko ID reżysera

        public int DurationInMinutes { get; set; }

        public string? PosterUrl { get; set; } // Istniejący URL, jeśli edytuje

        // Ta część odbiera dane z formularza
        public IFormFile? Plakat { get; set; } // Przesłany plik
        public int[]? WybraneGatunki { get; set; } // Zaznaczone checkboxy
        public int[]? WybraneAktorzy { get; set; }

        // Ta część dostarcza dane do wypełnienia formularza (dla list rozwijanych i checkboxów)
        public SelectList? ReżyserOptions { get; set; }
        public MultiSelectList? GatunkiOptions { get; set; }
        public MultiSelectList? AktorzyOptions { get; set; }
    }
}