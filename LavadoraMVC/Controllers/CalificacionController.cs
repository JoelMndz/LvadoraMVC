using LavadoraMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LavadoraMVC.Controllers
{
    [Authorize(Roles = "administrador")]
    public class CalificacionController : Controller
    {
        private readonly Contexto _context;
        public CalificacionController(Contexto contexto)
        {
            _context= contexto;
        }
        public async Task<IActionResult> Index()
        {
            var lista = await _context.Empleados.Include(x => x.Lavados).ToListAsync();
            return View(lista);
        }
    }
}
