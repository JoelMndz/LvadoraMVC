using LavadoraMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LavadoraMVC.Controllers
{
    [Authorize(Roles = "administrador")]
    public class ConsultaController : Controller
    {
        private readonly Contexto _context;

        public ConsultaController(Contexto context)
        {
            _context = context;
        }

        public async Task<IActionResult> CompararEmpleados()
        {
            ViewData["MostrarError"] = false;
            ViewData["MostrarResultado"] = false;
            ViewData["Empleados"] = new SelectList(_context.Empleados, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CompararEmpleados([Bind("IdEmpleado1,IdEmpleado2")] CompararEmpleado comparar)
        {
            ViewData["MostrarError"] = false;
            ViewData["MostrarResultado"] = false;
            ViewData["Empleados"] = new SelectList(_context.Empleados, "Id", "Nombre");
            if (ModelState.IsValid)
            {
                if (comparar.IdEmpleado1 == comparar.IdEmpleado2)
                {
                    ViewData["MostrarError"] = true;
                    ViewData["Error"] = "Debe escojer empleados diferentes!";
                }
                else
                {
                    var empleado1 = await _context.Empleados.Include(x => x.Lavados).Where(x => x.Id == comparar.IdEmpleado1).FirstAsync();
                    var empleado2 = await _context.Empleados.Include(x => x.Lavados).Where(x => x.Id == comparar.IdEmpleado2).FirstAsync();
                    if (empleado1.Lavados.Count == 0 && empleado2.Lavados.Count == 0)
                    {
                        ViewData["MostrarError"] = true;
                        ViewData["Error"] = "Los empleados no tienen lavados!";
                    }
                    else
                    {
                        ViewData["MostrarResultado"] = true;
                        if (empleado1.Lavados.Average(x => x.Amabilidad) > empleado2.Lavados.Average(x => x.Amabilidad))
                            ViewData["Amabilidad"] = $"{empleado1.Nombre} {empleado1.Lavados.Average(x => x.Amabilidad)} puntos";
                        else if (empleado1.Lavados.Average(x => x.Amabilidad) < empleado2.Lavados.Average(x => x.Amabilidad))
                            ViewData["Amabilidad"] = $"{empleado2.Nombre} {empleado2.Lavados.Average(x => x.Amabilidad)} puntos";
                        else
                            ViewData["Amabilidad"] = $"Ambos tienen {empleado1.Lavados.Average(x => x.Amabilidad)} puntos";

                        if (empleado1.Lavados.Average(x => x.Velocidad) > empleado2.Lavados.Average(x => x.Velocidad))
                            ViewData["Velocidad"] = $"{empleado1.Nombre} {empleado1.Lavados.Average(x => x.Velocidad)} puntos";
                        else if (empleado1.Lavados.Average(x => x.Velocidad) < empleado2.Lavados.Average(x => x.Velocidad))
                            ViewData["Velocidad"] = $"{empleado2.Nombre} {empleado2.Lavados.Average(x => x.Velocidad)} puntos";
                        else
                            ViewData["Velocidad"] = $"Ambos tienen {empleado1.Lavados.Average(x => x.Velocidad)} puntos";

                        if (empleado1.Lavados.Average(x => x.Calidad) > empleado2.Lavados.Average(x => x.Calidad))
                            ViewData["Calidad"] = $"{empleado1.Nombre} {empleado1.Lavados.Average(x => x.Calidad)} puntos";
                        else if (empleado1.Lavados.Average(x => x.Calidad) < empleado2.Lavados.Average(x => x.Calidad))
                            ViewData["Calidad"] = $"{empleado2.Nombre} {empleado2.Lavados.Average(x => x.Calidad)} puntos";
                        else
                            ViewData["Calidad"] = $"Ambos tienen {empleado1.Lavados.Average(x => x.Calidad)} puntos";

                        if (empleado1.Lavados.Average(x => x.Promedio) > empleado2.Lavados.Average(x => x.Promedio))
                            ViewData["Promedio"] = $"{empleado1.Nombre} es el mejor";
                        else if (empleado1.Lavados.Average(x => x.Promedio) < empleado2.Lavados.Average(x => x.Promedio))
                            ViewData["Promedio"] = $"{empleado2.Nombre} es el mejor";
                        else
                            ViewData["Promedio"] = $"Empate";

                        return View();
                    }
                }
            }
            return View(comparar);
        }

        public async Task<IActionResult> MejorTipoLavado()
        {
            ViewData["MostrarError"] = false;
            ViewData["MostrarResultado"] = false;
            ViewData["TipoLavados"] = new SelectList(_context.TipoLavados, "Id", "Descripcion");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MejorTipoLavado([Bind("Fecha")] RequestMejorTipo request)
        {
            ViewData["MostrarError"] = false;
            ViewData["MostrarResultado"] = false;
            ViewData["TipoLavados"] = new SelectList(_context.TipoLavados, "Id", "Descripcion");

            if (ModelState.IsValid)
            {
                var data = await _context.Lavados
                    .Include(x => x.IdTipoLavadoNavigation)
                    .Where(x => x.FechaCreacion!.Value.Year == request.Fecha.Year &&
                                x.FechaCreacion!.Value.Month == request.Fecha.Month &&
                                x.FechaCreacion!.Value.Day == request.Fecha.Day)
                    .GroupBy(x => x.IdTipoLavadoNavigation.Descripcion)
                    .Select(x => new { x.Key, Count = x.Count() })
                    .OrderByDescending(x => x.Count)
                    .FirstOrDefaultAsync();
                
                if (data == null)
                {
                    ViewData["MostrarError"] = true;
                    ViewData["Error"] = "No hay lavados registrados!";
                }
                else
                {
                    ViewData["MostrarResultado"] = true;
                    ViewData["Cantidad"] = data.Count;
                    ViewData["Tipo"] = data.Key;
                }

            }
            return View();
        }


        public IActionResult RangoEmpleados()
        {
            ViewData["MostrarError"] = false;
            ViewData["MostrarResultado"] = false;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RangoEmpleados([Bind("Inicio,Fin")] RangoEmpleado request)
        {
            ViewData["MostrarError"] = false;
            ViewData["MostrarResultado"] = false;

            if (ModelState.IsValid)
            {
                request.Fin = new(request.Fin.Year, request.Fin.Month, request.Fin.Day, 23, 59, 59);
                var data = await _context.Lavados
                    .Where(x => x.FechaCreacion >= request.Inicio && x.FechaCreacion <= request.Fin)
                    .ToListAsync();

                if (data.Count == 0)
                {
                    ViewData["MostrarError"] = true;
                    ViewData["Error"] = "No hay lavados registrados en este rango de fechas!";
                }
                else
                {
                    ViewData["MostrarResultado"] = true;
                    ViewData["Resultado"] = data.Average(x => x.Velocidad);
                }

            }
            return View();
        }

    }
}
