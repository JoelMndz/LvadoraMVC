using System.ComponentModel.DataAnnotations;

namespace LavadoraMVC.Models
{
    public class CompararEmpleado
    {
        [Required(ErrorMessage = "El empleado 1 es requerido")]
        public int IdEmpleado1 { get; set; }
        [Required(ErrorMessage = "El empleado 2 es requerido")]
        public int IdEmpleado2 { get; set; }

    }
}
