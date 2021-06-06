using System;
using System.ComponentModel.DataAnnotations;

namespace lab4.Models
{
    public class UserModel
    {
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage ="Must be True")]
        public bool ToRemember { get; set; }
    }
}
