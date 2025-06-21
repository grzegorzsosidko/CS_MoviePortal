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
    public class GatunekController : Controller
    {
        private readonly FirmaContext _context;

        public GatunekController(FirmaContext context)
        {
            _context = context;
        }

        // GET: Gatunek
        public async Task<IActionResult> Index()
        {
            return View(await _context.Gatunek.ToListAsync());
        }

        // GET: Gatunek/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gatunek/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdGatunek,Name")] Gatunek gatunek)
        {
            // Sprawdzam, czy gatunek o takiej nazwie już istnieje (ignoruję wielkość liter)
            bool isDuplicate = await _context.Gatunek.AnyAsync(g => g.Name.ToUpper() == gatunek.Name.ToUpper());

            if (isDuplicate)
            {
                // Jeśli tak, dodaję błąd do ModelState. To zatrzyma proces zapisu.
                ModelState.AddModelError("Name", "Gatunek o tej nazwie już istnieje.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(gatunek);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gatunek);
        }

        // GET: Gatunek/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gatunek = await _context.Gatunek.FindAsync(id);
            if (gatunek == null)
            {
                return NotFound();
            }
            return View(gatunek);
        }

        // POST: Gatunek/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdGatunek,Name")] Gatunek gatunek)
        {
            if (id != gatunek.IdGatunek)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gatunek);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GatunekExists(gatunek.IdGatunek))
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
            return View(gatunek);
        }

        // GET: Gatunek/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gatunek = await _context.Gatunek
                .FirstOrDefaultAsync(m => m.IdGatunek == id);
            if (gatunek == null)
            {
                return NotFound();
            }

            return View(gatunek);
        }

        // POST: Gatunek/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gatunek = await _context.Gatunek.FindAsync(id);
            if (gatunek != null)
            {
                _context.Gatunek.Remove(gatunek);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GatunekExists(int id)
        {
            return _context.Gatunek.Any(e => e.IdGatunek == id);
        }
    }
}
