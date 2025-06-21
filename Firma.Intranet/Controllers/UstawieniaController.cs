using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Firma.Data.Data;
using Firma.Data.Data.AppSettings;

namespace Firma.Intranet.Controllers
{
    public class UstawieniaController : Controller
    {
        private readonly FirmaContext _context;

        public UstawieniaController(FirmaContext context)
        {
            _context = context;
        }

        // GET: Ustawienia
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ustawienie.ToListAsync());
        }

        // GET: Ustawienia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ustawienie = await _context.Ustawienie
                .FirstOrDefaultAsync(m => m.IdUstawienia == id);
            if (ustawienie == null)
            {
                return NotFound();
            }

            return View(ustawienie);
        }

        // GET: Ustawienia/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ustawienia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUstawienia,Klucz,Wartosc")] Ustawienie ustawienie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ustawienie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ustawienie);
        }

        // GET: Ustawienia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ustawienie = await _context.Ustawienie.FindAsync(id);
            if (ustawienie == null)
            {
                return NotFound();
            }
            return View(ustawienie);
        }

        // POST: Ustawienia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUstawienia,Klucz,Wartosc")] Ustawienie ustawienie)
        {
            if (id != ustawienie.IdUstawienia)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ustawienie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UstawienieExists(ustawienie.IdUstawienia))
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
            return View(ustawienie);
        }

        // GET: Ustawienia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ustawienie = await _context.Ustawienie
                .FirstOrDefaultAsync(m => m.IdUstawienia == id);
            if (ustawienie == null)
            {
                return NotFound();
            }

            return View(ustawienie);
        }

        // POST: Ustawienia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ustawienie = await _context.Ustawienie.FindAsync(id);
            if (ustawienie != null)
            {
                _context.Ustawienie.Remove(ustawienie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UstawienieExists(int id)
        {
            return _context.Ustawienie.Any(e => e.IdUstawienia == id);
        }
    }
}
