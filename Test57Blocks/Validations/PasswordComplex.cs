using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Test57Blocks.Validations
{
    public class PasswordComplex: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
           if(value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }
            else
            {
                string password=value.ToString();
                string[] allowedCharacters = { "!", "@", "#", "?", "]" };
                if (password.Any(char.IsLower) && password.Any(char.IsUpper) && allowedCharacters.Any(password.Contains))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Password must have at least one uppercase letter, one lowercase letter, and one special character");
                }
                
            }
        }
    }
}
