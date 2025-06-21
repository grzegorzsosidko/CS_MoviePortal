using Firma.Data.Data;
using Firma.Data.Data.Movie;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Firma.PortalWWW.Controllers
{
    // Cały kontroler jest dostępny tylko dla zalogowanych użytkowników
    [Authorize]
    public class RecenzjaController : Controller
    {
        private readonly FirmaContext _context;

        public RecenzjaController(FirmaContext context)
        {
            _context = context;
        }

        // Akcja, która przyjmuje dane z formularza
        [HttpPost]
        public async Task<IActionResult> Add(int IdFilm, int Ocena, string Tresc)
        {
            // Pobieram ID zalogowanego użytkownika z jego "oświadczeń" (claims)
            var userIdString = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                // Jeśli z jakiegoś powodu nie ma ID, nie pozwalam na dodanie
                return Unauthorized();
            }

            var userId = int.Parse(userIdString);

            // Tworzę nowy obiekt recenzji
            var recenzja = new Recenzja
            {
                IdFilm = IdFilm,
                IdUser = userId,
                Ocena = Ocena,
                Tresc = Tresc,
                DataDodania = DateTime.Now
            };

            // Zapisuję recenzję do bazy
            _context.Recenzja.Add(recenzja);
            await _context.SaveChangesAsync();

            // Przekierowuję użytkownika z powrotem na stronę szczegółów filmu, którego dotyczyła recenzja
            return RedirectToAction("Details", "Film", new { id = IdFilm });
        }
    }
}