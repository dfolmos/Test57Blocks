using System;
using System.Collections.Generic;

#nullable disable

namespace Test57Blocks.Models
{
    public partial class User
    {
        public User()
        {
            MovieIdUserNavigations = new HashSet<Movie>();
            MovieModifiedByNavigations = new HashSet<Movie>();
        }

        public int IdUser { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }

        public virtual ICollection<Movie> MovieIdUserNavigations { get; set; }
        public virtual ICollection<Movie> MovieModifiedByNavigations { get; set; }
    }
}
