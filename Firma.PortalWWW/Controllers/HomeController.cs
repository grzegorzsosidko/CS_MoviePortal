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

        // Wstrzykuj� kontekst bazy danych do kontrolera
        public HomeController(FirmaContext context)
        {
            _context = context;
        }

        // Akcja dla strony g��wnej
        public async Task<IActionResult> Index(string? searchString)
        {
            // Tworz� ViewModel, kt�ry zawsze b�dzie przekazywany do widoku
            var viewModel = new HomeViewModel
            {
                AktualneWyszukiwanie = searchString
            };

            // Przekazuj� fraz� do layoutu, aby pole wyszukiwania j� "pami�ta�o"
            ViewData["CurrentFilter"] = searchString;

            // Je�li u�ytkownik co� wpisa� w wyszukiwark�...
            if (!String.IsNullOrEmpty(searchString))
            {
                // ...wype�niam list� z wynikami wyszukiwania.
                viewModel.WynikiWyszukiwania = await _context.Film
                    .Include(f => f.Re�yser).Include(f => f.Gatunki)
                    .Where(f => f.Title.ToUpper().Contains(searchString.ToUpper()))
                    .ToListAsync();
            }
            else // Je�li wyszukiwarka jest pusta, czyli wy�wietlam standardow� stron� g��wn�...
            {
                // ...wype�niam listy dla dynamicznych sekcji.
                viewModel.NajnowszeFilmy = await _context.Film
                    .Include(f => f.Re�yser).Include(f => f.Gatunki)
                    .OrderByDescending(f => f.ReleaseYear)
                    .Take(4)
                    .ToListAsync();

                viewModel.NajlepiejOcenianeFilmy = await _context.Film
                    .Include(f => f.Re�yser).Include(f => f.Gatunki)
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