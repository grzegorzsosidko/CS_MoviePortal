using Firma.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Firma.PortalWWW.Controllers
{
    public class AktorController : Controller
    {
        private readonly FirmaContext _context;
        public AktorController(FirmaContext context) { _context = context; }

        public async Task<IActionResult> Details(int id)
        {
            var aktor = await _context.Aktor
                .Include(a => a.Filmy)
                .FirstOrDefaultAsync(a => a.IdAktor == id);

            if (aktor == null) return NotFound();

            return View(aktor);
        }
    }
}