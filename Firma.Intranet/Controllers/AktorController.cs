using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Firma.Data.Data;
using Firma.Data.Data.Movie;
using Microsoft.AspNetCore.Authorization;

namespace Firma.Intranet.Controllers
{
    [Authorize(Roles = "Admin,Moderator")]
    public class AktorController : Controller
    {
        private readonly FirmaContext _context;

        public AktorController(FirmaContext context)
        {
            _context = context;
        }

        // GET: Aktor
        public async Task<IActionResult> Index()
        {
            var hash = BCrypt.Net.BCrypt.HashPassword("Admin123!");
            Console.WriteLine("hashed password" + hash); // Wyświetli hash w konsoli wyjściowej
            return View(await _context.Aktor.ToListAsync());
        }

        // GET: Aktor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aktor = await _context.Aktor
                .FirstOrDefaultAsync(m => m.IdAktor == id);
            if (aktor == null)
            {
                return NotFound();
            }

            return View(aktor);
        }

        // GET: Aktor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Aktor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAktor,Imie,Nazwisko,DataUrodzenia")] Aktor aktor, IFormFile? zdjecie)
        {
            // Najpierw sprawdzam, czy w bazie istnieje już aktor o takim samym imieniu i nazwisku
            bool isDuplicate = await _context.Aktor.AnyAsync(a =>
                a.Imie.ToUpper() == aktor.Imie.ToUpper() &&
                a.Nazwisko.ToUpper() == aktor.Nazwisko.ToUpper()
            );

            if (isDuplicate)
            {
                // Jeśli znajdę duplikat, dodaję błąd do stanu modelu, który zostanie wyświetlony w widoku
                ModelState.AddModelError(string.Empty, "Aktor o tym imieniu i nazwisku już istnieje.");
            }

            // Sprawdzam, czy cały model jest poprawny (włącznie z moim własnym błędem powyżej)
            if (ModelState.IsValid)
            {
                // Sprawdzam, czy został przesłany plik ze zdjęciem
                if (zdjecie != null && zdjecie.Length > 0)
                {
                    // Zapisuję plik na serwerze i przypisuję ścieżkę do modelu
                    aktor.ZdjecieUrl = await SaveFile(zdjecie);
                }

                // Dodaję nowego aktora do kontekstu i zapisuję w bazie
                _context.Add(aktor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Jeśli model nie był poprawny, zwracam widok z formularzem i błędami
            return View(aktor);
        }

        // GET: Aktor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aktor = await _context.Aktor.FindAsync(id);
            if (aktor == null)
            {
                return NotFound();
            }
            return View(aktor);
        }

        // POST: Aktor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAktor,Imie,Nazwisko,DataUrodzenia,ZdjecieUrl")] Aktor aktor, IFormFile? zdjecie)
        {
            if (id != aktor.IdAktor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Pobieramoryginalny rekord aktora z bazy danych
                    var aktorToUpdate = await _context.Aktor.FindAsync(id);
                    if (aktorToUpdate == null) { return NotFound(); }

                    // Aktualizuje jego właściwości na podstawie danych z formularza
                    aktorToUpdate.Imie = aktor.Imie;
                    aktorToUpdate.Nazwisko = aktor.Nazwisko;
                    aktorToUpdate.DataUrodzenia = aktor.DataUrodzenia;

                    // ZMIANA LOGIKI PLIKU:
                    // Aktualizuje URL zdjęcia TYLKO jeśli został przesłany nowy plik
                    if (zdjecie != null && zdjecie.Length > 0)
                    {
                        aktorToUpdate.ZdjecieUrl = await SaveFile(zdjecie);
                    }
                    // Jeśli nie przesłano nowego pliku, stare pole ZdjecieUrl pozostaje nietknięte

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Aktor.Any(e => e.IdAktor == aktor.IdAktor)) { return NotFound(); } else { throw; }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(aktor);
        }

        // GET: Aktor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aktor = await _context.Aktor
                .FirstOrDefaultAsync(m => m.IdAktor == id);
            if (aktor == null)
            {
                return NotFound();
            }

            return View(aktor);
        }

        // POST: Aktor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aktor = await _context.Aktor.FindAsync(id);
            if (aktor != null)
            {
                _context.Aktor.Remove(aktor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AktorExists(int id)
        {
            return _context.Aktor.Any(e => e.IdAktor == id);
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetExtension(file.FileName);
            // Zmieniam folder docelowy na dedykowany dla aktorów
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/actors");
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            Directory.CreateDirectory(uploadsFolder);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Zwracam nową, poprawną ścieżkę
            return "/uploads/actors/" + uniqueFileName;
        }
    }
}
