using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoviesAPI.Data.Entities;
using MoviesAPI.ViewModels;

namespace MoviesAPI.Data
{
    public interface IMoviesRepository
    {
        Task<List<Movie>> GetMoviesAsync(string title, int? year, Genre? genre);

        Task<List<Movie>> GetTop5RatedMoviesAsync();

        Task<List<Movie>> GetTop5RatedByUserMoviesAsync(int userID);

        Task AddOrUpdateMovieRatingAsync(RatingModel ratingViewModel);
        Task<Movie> GetMovieByIdAsync(int movieID);
    }
}
