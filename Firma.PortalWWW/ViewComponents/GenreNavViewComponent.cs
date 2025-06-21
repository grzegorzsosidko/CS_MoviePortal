using Firma.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Firma.PortalWWW.ViewComponents
{
    public class GenreNavViewComponent : ViewComponent
    {
        private readonly FirmaContext _context;
        public GenreNavViewComponent(FirmaContext context) { _context = context; }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Pobieram wszystkie gatunki i przekazuję do widoku
            var gatunki = await _context.Gatunek.OrderBy(g => g.Name).ToListAsync();
            return View(gatunki);
        }
    }
}
