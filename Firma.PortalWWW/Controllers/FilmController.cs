using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Firma.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Firma.PortalWWW.Controllers
{
    public class FilmController : Controller
    {
        private readonly FirmaContext _context;

        public FilmController(FirmaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Details(int id)
        {
            var film = await _context.Film
                .Include(f => f.Reżyser)
                .Include(f => f.Gatunki)
                .Include(f => f.Aktorzy)
                .Include(f => f.Recenzje)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(f => f.IdFilm == id);

            if (film == null)
            {
                return NotFound();
            }

            // Sprawdzam, czy zalogowany użytkownik polubił już ten film
            bool isFavorite = false;
            if (User.Identity.IsAuthenticated)
            {
                var userIdClaim = User.FindFirstValue("UserId");
                if (userIdClaim != null)
                {
                    var idUzytkownika = int.Parse(userIdClaim);
                    isFavorite = await _context.UlubioneFilmy
                                               .AnyAsync(f => f.IdFilmu == id && f.IdUzytkownika == idUzytkownika);
                }
            }

            // Przekazuję informację do widoku, aby mógł poprawnie wyświetlić przycisk
            ViewBag.IsFavorite = isFavorite;

            return View(film);
        }

        // Akcja Index pozostaje bez zmian - Twój kod jest świetny!
        public async Task<IActionResult> Index(int? genreId, string sortOrder)
        {
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "date" ? "date_desc" : "date";

            var filmy = _context.Film
                .Include(f => f.Reżyser)
                .Include(f => f.Gatunki)
                .AsQueryable();

            if (genreId != null)
            {
                var gatunek = await _context.Gatunek.FindAsync(genreId);
                if (gatunek != null)
                {
                    ViewData["Title"] = "Gatunek: " + gatunek.Name;
                    filmy = filmy.Where(f => f.Gatunki.Any(g => g.IdGatunek == genreId));
                }
            }
            else
            {
                ViewData["Title"] = "Wszystkie filmy";
            }

            switch (sortOrder)
            {
                case "title_desc":
                    filmy = filmy.OrderByDescending(f => f.Title);
                    break;
                case "date":
                    filmy = filmy.OrderBy(f => f.ReleaseYear);
                    break;
                case "date_desc":
                    filmy = filmy.OrderByDescending(f => f.ReleaseYear);
                    break;
                default:
                    filmy = filmy.OrderBy(f => f.Title);
                    break;
            }

            return View(await filmy.ToListAsync());
        }

        // Dodaję nową akcję do obsługi polubień
        [HttpPost]
        [Authorize] // Dostępna tylko dla zalogowanych użytkowników
        public async Task<IActionResult> PrzelaczUlubione(int idFilmu)
        {
            var idUzytkownika = int.Parse(User.FindFirstValue("UserId"));

            var istniejacePolubienie = await _context.UlubioneFilmy
                .FirstOrDefaultAsync(f => f.IdFilmu == idFilmu && f.IdUzytkownika == idUzytkownika);

            if (istniejacePolubienie != null)
            {
                // Jeśli polubienie istnieje, usuwam je
                _context.UlubioneFilmy.Remove(istniejacePolubienie);
                await _context.SaveChangesAsync();
                return Json(new { success = true, isFavorite = false });
            }
            else
            {
                // Jeśli polubienie nie istnieje, dodaję je
                var nowePolubienie = new Data.Data.Ulubione.UlubionyFilm
                {
                    IdFilmu = idFilmu,
                    IdUzytkownika = idUzytkownika,
                    DataDodania = DateTime.Now
                };
                _context.UlubioneFilmy.Add(nowePolubienie);
                await _context.SaveChangesAsync();
                return Json(new { success = true, isFavorite = true });
            }
        }
        
    }
}