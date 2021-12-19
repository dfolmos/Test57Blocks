using System;
using System.Collections.Generic;

#nullable disable

namespace Test57Blocks.Models
{
    public partial class Movie
    {
        public int IdMovie { get; set; }
        public string MovieTitle { get; set; }
        public int Duration { get; set; }
        public int Rating { get; set; }
        public int IdGenre { get; set; }
        public int IdUser { get; set; }
        public bool Private { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }

        public virtual Genre IdGenreNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; }
        public virtual User ModifiedByNavigation { get; set; }
    }
}
