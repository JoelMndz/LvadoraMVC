using System.ComponentModel.DataAnnotations;

namespace LavadoraMVC.Models
{
    public class Login
    {
        [Required(ErrorMessage = "El email es requerido")]
        [StringLength(50, ErrorMessage = "El valor {0} no puede superar los {1} caracteres. ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El password es requerido")]
        [StringLength(50, ErrorMessage = "El valor {0} no puede superar los {1} caracteres. ")]
        public string Password { get; set; }
    }
}
