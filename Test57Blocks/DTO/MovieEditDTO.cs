using System.ComponentModel.DataAnnotations;

namespace Test57Blocks.DTO
{
    public class MovieEditDTO: MovieCreateDTO
    {
        [Required(ErrorMessage = "{0} is required")]
        public int IdMovie { get; set; }
    }
}
