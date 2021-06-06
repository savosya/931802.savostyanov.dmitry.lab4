using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace lab4.ViewModel
{
    public class RegisterModel2
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Must be True")]
        public bool ToRemember { get; set; }
    }
}
