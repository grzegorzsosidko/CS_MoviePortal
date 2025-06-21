using Firma.Data.Data;
using Firma.Data.Data.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Firma.Intranet.Controllers
{
    // Dostęp do tego kontrolera ma tylko użytkownik z rolą "Admin"
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly FirmaContext _context;

        public UserController(FirmaContext context)
        {
            _context = context;
        }

        // GET: User
        // Akcja wyświetla listę wszystkich użytkowników
        public async Task<IActionResult> Index()
        {
            // Pobieram z bazy wszystkich użytkowników i przekazuję do widoku
            return View(await _context.User.ToListAsync());
        }

        // GET: User/Edit/5
        // Akcja wyświetla formularz edycji roli dla wybranego użytkownika
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            // Przekazuję obiekt użytkownika do widoku
            return View(user);
        }

        // POST: User/Edit/5
        // Akcja zapisuje zmianę roli
        [HttpPost]
        [ValidateAntiForgeryToken]
        // ZMIANA: Przyjmuję tylko ID z adresu i nową rolę z formularza.
        // ASP.NET automatycznie dopasuje wartość z listy <select name="Role"> do parametru "role".
        public async Task<IActionResult> Edit(int id, UserRole role)
        {
            // 1. Pobieram z bazy pełny, oryginalny obiekt użytkownika na podstawie ID z adresu.
            var userToUpdate = await _context.User.FindAsync(id);
            if (userToUpdate == null)
            {
                return NotFound();
            }

            // 2. Aktualizuję w nim tylko i wyłącznie tę jedną właściwość, którą chciałem zmienić.
            userToUpdate.Role = role;

            try
            {
                // 3. Zapisuję zmiany. Nie ma tu już problemu z walidacją,
                // ponieważ operuję na pełnym obiekcie pobranym z bazy.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Obsługa ewentualnego, nieprzewidzianego błędu zapisu.
                ModelState.AddModelError("", "Nie udało się zapisać zmian. Spróbuj ponownie.");
                // Zwracam pełny obiekt, aby formularz się nie wyczyścił.
                return View(userToUpdate);
            }

            // Po sukcesie wracam do listy użytkowników.
            return RedirectToAction(nameof(Index));
        }
    }
}