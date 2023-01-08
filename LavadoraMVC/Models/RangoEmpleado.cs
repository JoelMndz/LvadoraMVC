using System.ComponentModel.DataAnnotations;
namespace LavadoraMVC.Models
{
    public class RangoEmpleado
    {
        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        public DateTime Inicio { get; set; }
        [Required(ErrorMessage = "La fecha de fin es requerida")]
        public DateTime Fin { get; set; }
    }
}
