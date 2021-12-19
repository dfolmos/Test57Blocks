using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;
using Test57Blocks.Validations;

namespace Test57Blocks.DTO
{
    public class LoginDTO
    {
        [Email(ErrorMessage = "Iconrrect {0} format")]
        [Required(ErrorMessage = "{0} is required")]
        public string UserEmail { get; set; }
        
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(10, ErrorMessage = "{0} must be at least 10 characters")]
        [PasswordComplex]
        public string Password { get; set; }
        
    }
}
