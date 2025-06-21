using Firma.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Firma.PortalWWW.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly FirmaContext _context;
        public FooterViewComponent(FirmaContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Pobieram strony, które mają przypisaną kategorię stopki
            var stopkaLinki = await _context.Strona
                .Where(s => s.KategoriaStopki != null)
                .OrderBy(s => s.Pozycja)
                .ToListAsync();

            // Grupuję linki po ich kategorii i przekazuję do widoku
            return View(stopkaLinki.GroupBy(s => s.KategoriaStopki));
        }
    }
}