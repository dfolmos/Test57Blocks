using System;

namespace Test57Blocks.DTO
{
    public class MovieQueryDTO: MovieEditDTO
    {
        public int IdUser { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public string Genre { get; set; }

        public string Owner { get; set; }
    }
}
