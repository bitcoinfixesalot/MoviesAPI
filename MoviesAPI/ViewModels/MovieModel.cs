using MoviesAPI.Data.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.ViewModels
{
    public class MovieModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int YearOfRelease { get; set; }

        public int RunningTime { get; set; }

        public Genre Genres { get; set; }

        public decimal AverageRating { get; set; }
    }
}
