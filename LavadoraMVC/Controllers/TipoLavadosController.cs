using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LavadoraMVC.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace LavadoraMVC.Controllers
{
    [Authorize(Roles = "administrador")]
    public class TipoLavadosController : Controller
    {
        private readonly Contexto _context;

        public TipoLavadosController(Contexto context)
        {
            _context = context;
        }

        // GET: TipoLavados
        public async Task<IActionResult> Index()
        {
              return View(await _context.TipoLavados.ToListAsync());
        }

        // GET: TipoLavados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TipoLavados == null)
            {
                return NotFound();
            }

            var tipoLavados = await _context.TipoLavados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoLavados == null)
            {
                return NotFound();
            }

            return View(tipoLavados);
        }

        // GET: TipoLavados/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoLavados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion")] TipoLavados tipoLavados)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoLavados);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoLavados);
        }

        // GET: TipoLavados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TipoLavados == null)
            {
                return NotFound();
            }

            var tipoLavados = await _context.TipoLavados.FindAsync(id);
            if (tipoLavados == null)
            {
                return NotFound();
            }
            return View(tipoLavados);
        }

        // POST: TipoLavados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion")] TipoLavados tipoLavados)
        {
            if (id != tipoLavados.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoLavados);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoLavadosExists(tipoLavados.Id))
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
            return View(tipoLavados);
        }

        // GET: TipoLavados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TipoLavados == null)
            {
                return NotFound();
            }

            var tipoLavados = await _context.TipoLavados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoLavados == null)
            {
                return NotFound();
            }

            return View(tipoLavados);
        }

        // POST: TipoLavados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TipoLavados == null)
            {
                return Problem("Entity set 'Contexto.TipoLavados'  is null.");
            }
            var tipoLavados = await _context.TipoLavados.FindAsync(id);
            if (tipoLavados != null)
            {
                _context.TipoLavados.Remove(tipoLavados);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoLavadosExists(int id)
        {
          return _context.TipoLavados.Any(e => e.Id == id);
        }
    }
}
