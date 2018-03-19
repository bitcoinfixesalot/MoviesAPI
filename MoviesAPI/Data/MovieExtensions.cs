using MoviesAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Data
{
    public static class MovieExtensions
    {
        public static decimal CalculateAvarageRating(this Movie movie)
        {
            if (movie?.Ratings.Count == 0) return 0;

            decimal rating =  (decimal)movie.Ratings.Sum(a => a.RatingValue) / (decimal)movie.Ratings.Count;

            var rounded = Math.Round((rating * 2), MidpointRounding.AwayFromZero);

            return rounded / 2;
        }
    }
}
