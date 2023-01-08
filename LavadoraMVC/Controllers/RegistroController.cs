using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LavadoraMVC.Models;

namespace LavadoraMVC.Controllers
{
    public class RegistroController : Controller
    {
        private readonly Contexto _context;

        public RegistroController(Contexto context)
        {
            _context = context;
        }

        // GET: Registro
        public IActionResult Index()
        {
            ViewBag.MostrarError = false;
            return View();
        }

        // POST: Registro/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Id,Nombre,Celular,Email,Password,Rol")] Usuarios usuarios)
        {
            ViewBag.MostrarError = false;
            if (ModelState.IsValid)
            {
                var existeEmail = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(usuarios.Email.ToLower()));
                var existeAdmin = await _context.Usuarios.FirstOrDefaultAsync(x => x.Rol.Equals("administrador"));
                if (existeAdmin != null && usuarios.Rol == "administrador")
                {
                    ViewBag.MostrarError = true;
                    ViewBag.Error = "Ya existe un administrador registrado!";
                }else if(existeEmail != null)
                {
                    ViewBag.MostrarError = true;
                    ViewBag.Error = "El email ya está registrado!";
                }
                else
                {
                    _context.Add(usuarios);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(usuarios);
        }

    }
}
