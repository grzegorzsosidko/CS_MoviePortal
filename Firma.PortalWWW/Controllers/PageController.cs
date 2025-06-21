using Firma.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class PageController : Controller
{
    private readonly FirmaContext _context;
    public PageController(FirmaContext context)
    {
        _context = context;
    }

    // Akcja przyjmuje "link" z adresu URL i na jego podstawie szuka strony w bazie
    public async Task<IActionResult> Index(string link)
    {
        if (link == null) return NotFound();
        var strona = await _context.Strona.FirstOrDefaultAsync(s => s.Link == link);
        if (strona == null) return NotFound();
        return View(strona);
    }
}