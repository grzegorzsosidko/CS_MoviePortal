using Firma.Data.Data;
using Firma.PortalWWW.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Firma.PortalWWW.Controllers
{
    public class HomeController : Controller
    {
        private readonly FirmaContext _context;

        // Wstrzykujê kontekst bazy danych do kontrolera
        public HomeController(FirmaContext context)
        {
            _context = context;
        }

        // Akcja dla strony g³ównej
        public async Task<IActionResult> Index(string? searchString)
        {
            // Tworzê ViewModel, który zawsze bêdzie przekazywany do widoku
            var viewModel = new HomeViewModel
            {
                AktualneWyszukiwanie = searchString
            };

            // Przekazujê frazê do layoutu, aby pole wyszukiwania j¹ "pamiêta³o"
            ViewData["CurrentFilter"] = searchString;

            // Jeœli u¿ytkownik coœ wpisa³ w wyszukiwarkê...
            if (!String.IsNullOrEmpty(searchString))
            {
                // ...wype³niam listê z wynikami wyszukiwania.
                viewModel.WynikiWyszukiwania = await _context.Film
                    .Include(f => f.Re¿yser).Include(f => f.Gatunki)
                    .Where(f => f.Title.ToUpper().Contains(searchString.ToUpper()))
                    .ToListAsync();
            }
            else // Jeœli wyszukiwarka jest pusta, czyli wyœwietlam standardow¹ stronê g³ówn¹...
            {
                // ...wype³niam listy dla dynamicznych sekcji.
                viewModel.NajnowszeFilmy = await _context.Film
                    .Include(f => f.Re¿yser).Include(f => f.Gatunki)
                    .OrderByDescending(f => f.ReleaseYear)
                    .Take(4)
                    .ToListAsync();

                viewModel.NajlepiejOcenianeFilmy = await _context.Film
                    .Include(f => f.Re¿yser).Include(f => f.Gatunki)
                    .Where(f => f.Recenzje.Any())
                    .OrderByDescending(f => f.Recenzje.Average(r => r.Ocena))
                    .Take(4)
                    .ToListAsync();
            }

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}