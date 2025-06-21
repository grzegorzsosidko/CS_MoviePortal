using Firma.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class NavbarViewComponent : ViewComponent
{
    private readonly FirmaContext _context;
    public NavbarViewComponent(FirmaContext context) { _context = context; }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        // Pobieram strony, które mają być widoczne w menu nawigacyjnym i sortuję je
        var strony = await _context.Strona.Where(s => s.PokazWNavbar).OrderBy(s => s.Pozycja).ToListAsync();
        return View(strony);
    }
}