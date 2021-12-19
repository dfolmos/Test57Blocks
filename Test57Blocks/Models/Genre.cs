using System;
using System.Collections.Generic;

#nullable disable

namespace Test57Blocks.Models
{
    public partial class Genre
    {
        public Genre()
        {
            Movies = new HashSet<Movie>();
        }

        public int IdGenre { get; set; }
        public string GenreName { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}
