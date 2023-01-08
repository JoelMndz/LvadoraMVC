using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LavadoraMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace LavadoraMVC.Controllers
{
    [Authorize(Roles = "administrador")]
    public class ClientesController : Controller
    {
        private readonly Contexto _context;

        public ClientesController(Contexto context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
              return View(await _context.Usuarios.Where(x => x.Rol == "cliente").ToListAsync());
        }
    }
}
