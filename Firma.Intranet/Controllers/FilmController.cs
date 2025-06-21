using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Firma.Data.Data;
using Firma.Data.Data.Movie;
using Firma.Intranet.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Firma.Intranet.Documents;
using QuestPDF.Fluent;

namespace Firma.Intranet.Controllers
{
    [Authorize(Roles = "Admin,Moderator")]
    public class FilmController : Controller
    {
        private readonly FirmaContext _context;

        public FilmController(FirmaContext context)
        {
            _context = context;
        }

        // GET: Film
        public async Task<IActionResult> Index()
        {
            return View(await _context.Film.Include(f => f.Gatunki).Include(f => f.Reżyser).Include(f => f.Aktorzy).ToListAsync());

        }

        // GET: Film/Create
        public IActionResult Create()
        {
            // Tworzy pusty ViewModel
            var viewModel = new FilmViewModel
            {
                // Wypełniam go opcjami dla list, które ma wyświetlić formularz
                ReżyserOptions = new SelectList(_context.Reżyser.OrderBy(r => r.Nazwisko), "IdReżyser", "Nazwisko"),
                GatunkiOptions = new MultiSelectList(_context.Gatunek.OrderBy(g => g.Name), "IdGatunek", "Name"),
                AktorzyOptions = new MultiSelectList(_context.Aktor.OrderBy(a => a.Nazwisko), "IdAktor", "Nazwisko")
            };
            return View(viewModel);
        }

        // POST: Film/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FilmViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Tworzę nowy, "czysty" obiekt encji Film na podstawie danych z ViewModelu
                var film = new Film
                {
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    ReleaseYear = viewModel.ReleaseYear,
                    IdReżyser = viewModel.IdReżyser,
                    DurationInMinutes = viewModel.DurationInMinutes
                };

                // Sprawdzam, czy został przesłany plik plakatu
                if (viewModel.Plakat != null && viewModel.Plakat.Length > 0)
                {
                    // Zapisuję plik na serwerze i otrzymuję jego względną ścieżkę
                    film.PosterUrl = await SaveFile(viewModel.Plakat);
                }

                // Dodaję film do kontekstu i zapisuję go w bazie, aby uzyskać jego ID
                _context.Add(film);
                await _context.SaveChangesAsync();

                // Sprawdzam, czy zostały wybrane jakieś gatunki
                if (viewModel.WybraneGatunki != null)
                {
                    // Dla każdego wybranego ID gatunku...
                    foreach (var gatunekId in viewModel.WybraneGatunki)
                    {
                        // ...dodaję powiązanie do kolekcji w moim nowym filmie
                        film.Gatunki.Add(await _context.Gatunek.FindAsync(gatunekId));
                    }
                }

                // Sprawdzam, czy zostali wybrani jacyś aktorzy
                if (viewModel.WybraneAktorzy != null)
                {
                    // Dla każdego wybranego ID aktora...
                    foreach (var aktorId in viewModel.WybraneAktorzy)
                    {
                        // ...dodaję powiązanie do kolekcji w moim nowym filmie
                        film.Aktorzy.Add(await _context.Aktor.FindAsync(aktorId));
                    }
                }

                // Zapisuję ponownie zmiany, tym razem utrwalając relacje M:N w tabelach pośredniczących
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Jeśli formularz nie był poprawny, muszę ponownie załadować wszystkie opcje do list
            viewModel.ReżyserOptions = new SelectList(_context.Reżyser.OrderBy(r => r.Nazwisko), "IdReżyser", "Nazwisko", viewModel.IdReżyser);
            viewModel.GatunkiOptions = new MultiSelectList(_context.Gatunek.OrderBy(g => g.Name), "IdGatunek", "Name", viewModel.WybraneGatunki);
            viewModel.AktorzyOptions = new MultiSelectList(_context.Aktor.OrderBy(a => a.Nazwisko), "IdAktor", "Nazwisko", viewModel.WybraneAktorzy);

            return View(viewModel);
        }

        // POST: Film/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FilmViewModel viewModel)
        {
            if (id != viewModel.IdFilm)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 1. Pobieram z bazy film, który edytuję, razem z jego aktualnymi powiązaniami
                    var filmToUpdate = await _context.Film
                        .Include(f => f.Gatunki)
                        .Include(f => f.Aktorzy)
                        .FirstOrDefaultAsync(f => f.IdFilm == id);

                    if (filmToUpdate == null)
                    {
                        return NotFound();
                    }

                    // 2. Aktualizuję proste właściwości filmu na podstawie danych z ViewModelu
                    filmToUpdate.Title = viewModel.Title;
                    filmToUpdate.Description = viewModel.Description;
                    filmToUpdate.ReleaseYear = viewModel.ReleaseYear;
                    filmToUpdate.IdReżyser = viewModel.IdReżyser;
                    filmToUpdate.DurationInMinutes = viewModel.DurationInMinutes;

                    // ZMIANA LOGIKI PLIKU:
                    // Najpierw przypisuje stary URL z ukrytego pola
                    filmToUpdate.PosterUrl = viewModel.PosterUrl;
                    // A potem, TYLKO jeśli użytkownik wybrał nowy plik, podmieniam go.
                    if (viewModel.Plakat != null && viewModel.Plakat.Length > 0)
                    {
                        filmToUpdate.PosterUrl = await SaveFile(viewModel.Plakat);
                    }

                    // 4. Aktualizuję kolekcje (Gatunki i Aktorzy)
                    // Najpierw czyszczę stare powiązania
                    filmToUpdate.Gatunki.Clear();
                    filmToUpdate.Aktorzy.Clear();

                    // Następnie dodaję na nowo te, które przyszły z formularza
                    if (viewModel.WybraneGatunki != null)
                    {
                        foreach (var gatunekId in viewModel.WybraneGatunki)
                        {
                            filmToUpdate.Gatunki.Add(await _context.Gatunek.FindAsync(gatunekId));
                        }
                    }

                    if (viewModel.WybraneAktorzy != null)
                    {
                        foreach (var aktorId in viewModel.WybraneAktorzy)
                        {
                            filmToUpdate.Aktorzy.Add(await _context.Aktor.FindAsync(aktorId));
                        }
                    }

                    // 5. Zapisuję wszystkie zmiany do bazy danych
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Film.Any(e => e.IdFilm == viewModel.IdFilm))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Jeśli formularz nie był poprawny, muszę ponownie załadować wszystkie opcje do list
            viewModel.ReżyserOptions = new SelectList(_context.Reżyser.OrderBy(r => r.Nazwisko), "IdReżyser", "Nazwisko", viewModel.IdReżyser);
            viewModel.GatunkiOptions = new MultiSelectList(_context.Gatunek.OrderBy(g => g.Name), "IdGatunek", "Name", viewModel.WybraneGatunki);
            viewModel.AktorzyOptions = new MultiSelectList(_context.Aktor.OrderBy(a => a.Nazwisko), "IdAktor", "Nazwisko", viewModel.WybraneAktorzy);

            return View(viewModel);
        }

        // GET: Film/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Film
                .Include(f => f.Gatunki)
                .Include(f => f.Aktorzy) // Dołączam aktorów
                .FirstOrDefaultAsync(f => f.IdFilm == id);

            if (film == null)
            {
                return NotFound();
            }

            // Tworzy ViewModel na podstawie danych z bazy
            var viewModel = new FilmViewModel
            {
                IdFilm = film.IdFilm,
                Title = film.Title,
                Description = film.Description,
                ReleaseYear = film.ReleaseYear,
                IdReżyser = film.IdReżyser,
                DurationInMinutes = film.DurationInMinutes,
                PosterUrl = film.PosterUrl,
                // Przygotowuje opcje dla list, zaznaczając aktualne wartości
                ReżyserOptions = new SelectList(_context.Reżyser.OrderBy(r => r.Nazwisko), "IdReżyser", "Nazwisko", film.IdReżyser),
                GatunkiOptions = new MultiSelectList(_context.Gatunek.OrderBy(g => g.Name), "IdGatunek", "Name", film.Gatunki.Select(g => g.IdGatunek).ToList()),
                AktorzyOptions = new MultiSelectList(_context.Aktor.OrderBy(a => a.Nazwisko), "IdAktor", "Nazwisko", film.Aktorzy.Select(a => a.IdAktor).ToList())
            };

            return View(viewModel);
        }

        // GET: Film/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Film
                .FirstOrDefaultAsync(m => m.IdFilm == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // POST: Film/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var film = await _context.Film.FindAsync(id);
            if (film != null)
            {
                _context.Film.Remove(film);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
            return _context.Film.Any(e => e.IdFilm == id);
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            // Tworzę unikalną nazwę pliku, aby uniknąć konfliktów
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetExtension(file.FileName);
            // Określam ścieżkę do folderu, w którym zapisuję pliki (wwwroot/uploads/posters)
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/posters");
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Upewniam się, że folder docelowy istnieje
            Directory.CreateDirectory(uploadsFolder);

            // Zapisuję plik na serwerze
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Zwracam względną ścieżkę do pliku, którą zapiszę w bazie danych
            return "/uploads/posters/" + uniqueFileName;
        }



        // Nowa akcja do eksportu
        public async Task<IActionResult> ExportToPdf()
        {
            // 1. Pobieram z bazy wszystkie filmy, które chcę wyeksportować.
            //    Dołączam reżysera, bo będę go potrzebował w PDFie.
            

            var filmy = await _context.Film
                            .Include(f => f.Reżyser)
                            .Include(f => f.Recenzje) 
                            .Include(f => f.Gatunki)  
                            .OrderBy(f => f.IdFilm)
                            .ToListAsync();

            // 2. Tworzę instancję mojego "przepisu" na dokument, przekazując mu listę filmów.
            var dokument = new FilmyKatalogDokument(filmy);

            // 3. Generuję plik PDF w pamięci jako tablicę bajtów.
            byte[] pdfBytes = dokument.GeneratePdf();

            // 4. Zwracam plik do przeglądarki.
            //    Ustawiam typ MIME na "application/pdf" i podaję domyślną nazwę pliku do pobrania.
            return File(pdfBytes, "application/pdf", "katalog_filmow.pdf");
        }
    }
}
