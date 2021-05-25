using System.ComponentModel.DataAnnotations;

namespace lab4.Models
{
    public class ConfirmPwModel
    {
        [Required(ErrorMessage = "Не указан пароль.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Не указан пароль.")]
        public string ConfirmPassword { get; set; }
    }
}
