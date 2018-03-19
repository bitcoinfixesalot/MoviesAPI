using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Data.Entities
{
    public class Movie
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int YearOfRelease { get; set; }

        public int RunningTime { get; set; }

        public Genre Genres { get; set; }

        public ICollection<Rating> Ratings { get; set; }
    }
}
