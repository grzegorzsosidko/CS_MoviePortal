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
    public class ReżyserController : Controller
    {
        private readonly FirmaContext _context;

        public ReżyserController(FirmaContext context)
        {
            _context = context;
        }

        // GET: Reżyser
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reżyser.ToListAsync());
        }

        // GET: Reżyser/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reżyser = await _context.Reżyser
                .FirstOrDefaultAsync(m => m.IdReżyser == id);
            if (reżyser == null)
            {
                return NotFound();
            }

            return View(reżyser);
        }

        // GET: Reżyser/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reżyser/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdReżyser,Imie,Nazwisko")] Reżyser reżyser)
        {
            // Sprawdzam, czy w bazie istnieje już reżyser o takim samym imieniu i nazwisku
            bool isDuplicate = await _context.Reżyser.AnyAsync(r =>
                r.Imie.ToUpper() == reżyser.Imie.ToUpper() &&
                r.Nazwisko.ToUpper() == reżyser.Nazwisko.ToUpper()
            );

            if (isDuplicate)
            {
                // Jeśli znajdę duplikat, dodaję błąd do stanu modelu
                ModelState.AddModelError(string.Empty, "Reżyser o tym imieniu i nazwisku już istnieje.");
            }

            if (ModelState.IsValid)
            {
                // Dodaję nowego reżysera i zapisuję w bazie
                _context.Add(reżyser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Zwracam widok z formularzem i błędami
            return View(reżyser);
        }

        // GET: Reżyser/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reżyser = await _context.Reżyser.FindAsync(id);
            if (reżyser == null)
            {
                return NotFound();
            }
            return View(reżyser);
        }

        // POST: Reżyser/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdReżyser,Imie,Nazwisko")] Reżyser reżyser)
        {
            if (id != reżyser.IdReżyser)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reżyser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReżyserExists(reżyser.IdReżyser))
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
            return View(reżyser);
        }

        // GET: Reżyser/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reżyser = await _context.Reżyser
                .FirstOrDefaultAsync(m => m.IdReżyser == id);
            if (reżyser == null)
            {
                return NotFound();
            }

            return View(reżyser);
        }

        // POST: Reżyser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reżyser = await _context.Reżyser.FindAsync(id);
            if (reżyser != null)
            {
                _context.Reżyser.Remove(reżyser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReżyserExists(int id)
        {
            return _context.Reżyser.Any(e => e.IdReżyser == id);
        }
    }
}
