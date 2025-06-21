using Firma.Data.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Firma.PortalWWW.Controllers
{
    [Authorize] // Dostęp tylko dla zalogowanych
    public class UserController : Controller
    {
        private readonly FirmaContext _context;
        public UserController(FirmaContext context) { _context = context; }

        // Główna strona panelu użytkownika
        public async Task<IActionResult> Index()
        {
            // Pobieram ID zalogowanego użytkownika
            var userId = int.Parse(User.FindFirstValue("UserId"));

            // Znajduję użytkownika i dołączam listę jego recenzji,
            // a do każdej recenzji dołączam dane filmu, którego dotyczy.
            var user = await _context.User
                .Include(u => u.Recenzje)
                    .ThenInclude(r => r.Film)
                .FirstOrDefaultAsync(u => u.IdUser == userId);

            if (user == null) return NotFound();

            return View(user);
        }

        public async Task<IActionResult> MojeUlubione()
        {
            // Pobieram ID użytkownika - tak jak robisz to w akcji Index.
            var idUzytkownika = int.Parse(User.FindFirstValue("UserId"));

            // Pobieram z bazy listę polubień dla danego użytkownika.
            // Używam .Include(f => f.Film), aby od razu załadować dane filmu.
            var ulubioneFilmy = await _context.UlubioneFilmy
                .Where(f => f.IdUzytkownika == idUzytkownika)
                .Include(f => f.Film)
                .OrderByDescending(f => f.DataDodania)
                .ToListAsync();

            // Przekazuję listę do widoku.
            return View(ulubioneFilmy);
        }

    }
}