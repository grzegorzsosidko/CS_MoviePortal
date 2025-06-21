using Firma.Data.Data.Movie;

namespace Firma.PortalWWW.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<Film> NajnowszeFilmy { get; set; } = new List<Film>();
        public List<Film> NajlepiejOcenianeFilmy { get; set; } = new List<Film>();
        public string? AktualneWyszukiwanie { get; set; }
        public List<Film>? WynikiWyszukiwania { get; set; }
    }
}