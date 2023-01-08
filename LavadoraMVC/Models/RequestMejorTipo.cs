using System.ComponentModel.DataAnnotations;

namespace LavadoraMVC.Models
{
    public class RequestMejorTipo
    {
        [Required(ErrorMessage = "La fecha es requerida")]
        public DateTime Fecha { get; set; }
    }
}
