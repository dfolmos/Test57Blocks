using System.ComponentModel.DataAnnotations;

namespace Test57Blocks.DTO
{
    public class MovieCreateDTO
    {
        [Required(ErrorMessage = "{0} is required")]
        public string MovieTitle { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Range(0, int.MaxValue, ErrorMessage = "{0} must be greater than zero")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Range(0, 100, ErrorMessage = "{0} must be greater than zero and less than 100")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public int IdGenre { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public bool Private { get; set; }
    }
}
