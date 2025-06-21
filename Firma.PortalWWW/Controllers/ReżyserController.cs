using Firma.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Firma.PortalWWW.Controllers
{
    public class ReżyserController : Controller
    {
        private readonly FirmaContext _context;
        public ReżyserController(FirmaContext context) { _context = context; }

        // Akcja wyświetla stronę szczegółów dla reżysera o danym ID
        public async Task<IActionResult> Details(int id)
        {
            var reżyser = await _context.Reżyser
                // Dołączam do obiektu reżysera listę jego filmów
                .Include(r => r.Filmy)
                .FirstOrDefaultAsync(r => r.IdReżyser == id);

            if (reżyser == null) return NotFound();

            return View(reżyser);
        }
    }
}