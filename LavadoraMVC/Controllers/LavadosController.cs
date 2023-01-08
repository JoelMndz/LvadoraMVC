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
using Microsoft.AspNetCore.Identity;

namespace LavadoraMVC.Controllers
{
    public class LavadosController : Controller
    {
        private readonly Contexto _context;

        public LavadosController(Contexto context)
        {
            _context = context;
        }

        // GET: Lavados
        [Authorize(Roles = "administrador,cliente")]
        public async Task<IActionResult> Index()
        {
            var contexto = _context.Lavados.Include(l => l.IdClienteNavigation).Include(l => l.IdEmpleadoNavigation).Include(l => l.IdTipoLavadoNavigation);
            if (User.IsInRole("cliente"))
            {
                var id = User.Claims.First(x => x.Type == "Id").Value;
                return View(await contexto.Where(x => x.IdCliente == int.Parse(id)).ToListAsync());
            }
            return View(await contexto.ToListAsync());
        }

        // GET: Lavados/Details/5
        [Authorize(Roles = "administrador,cliente")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Lavados == null)
            {
                return NotFound();
            }

            var lavados = await _context.Lavados
                .Include(l => l.IdClienteNavigation)
                .Include(l => l.IdEmpleadoNavigation)
                .Include(l => l.IdTipoLavadoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lavados == null)
            {
                return NotFound();
            }

            return View(lavados);
        }

        // GET: Lavados/Create
        [Authorize(Roles = "cliente")]
        public IActionResult Create()
        {
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "Id", "Nombre");
            ViewData["IdTipoLavado"] = new SelectList(_context.TipoLavados, "Id", "Descripcion");
            return View();
        }

        // POST: Lavados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrador,cliente")]
        public async Task<IActionResult> Create([Bind("Id,FechaCreacion,Velocidad,Calidad,Amabilidad,IdEmpleado,IdTipoLavado")] Lavados lavados)
        {
            lavados.IdCliente = int.Parse(User.Claims.First(x => x.Type == "Id").Value);
            if (ModelState.IsValid)
            {
                lavados.FechaCreacion = DateTime.Now.ToLocalTime();
                lavados.Promedio = (lavados.Velocidad+lavados.Amabilidad+lavados.Calidad) / 3;
                _context.Add(lavados);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "Id", "Nombre", lavados.IdEmpleado);
            ViewData["IdTipoLavado"] = new SelectList(_context.TipoLavados, "Id", "Descripcion", lavados.IdTipoLavado);
            return View(lavados);
        }


        // GET: Lavados/Delete/5
        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Lavados == null)
            {
                return NotFound();
            }

            var lavados = await _context.Lavados
                .Include(l => l.IdClienteNavigation)
                .Include(l => l.IdEmpleadoNavigation)
                .Include(l => l.IdTipoLavadoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lavados == null)
            {
                return NotFound();
            }

            return View(lavados);
        }

        // POST: Lavados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrador,cliente")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Lavados == null)
            {
                return Problem("Entity set 'Contexto.Lavados'  is null.");
            }
            var lavados = await _context.Lavados.FindAsync(id);
            if (lavados != null)
            {
                _context.Lavados.Remove(lavados);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
