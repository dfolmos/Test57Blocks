using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;
using Test57Blocks.Validations;

namespace Test57Blocks.DTO
{
    public class RegisterUserDTO
    {
        [Email(ErrorMessage ="Iconrrect email format")]
        [Required(ErrorMessage = "Email is required")]
        public string UserEmail { get; set; }
        
        [Required(ErrorMessage ="Password is required")]
        [StringLength(10, ErrorMessage = "Password must be at least 10 characters")]
        [PasswordComplex]
        public string Password { get; set; }
       
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

    }
}
