using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Data.Entities;
using MoviesAPI.ViewModels;

namespace MoviesAPI.Data
{
    public class MoviesRepository : IMoviesRepository
    {
        private readonly MoviesContext _ctx;

        public MoviesRepository(MoviesContext ctx)
        {
            this._ctx = ctx;
        }

        public async Task AddOrUpdateMovieRatingAsync(RatingModel model)
        {
            var rating = await _ctx.Ratings.FirstOrDefaultAsync(a => a.UserID == model.UserID && a.MovieID == model.MovieID);
            if (rating == null)
            {
                rating = new Rating { MovieID = model.MovieID, UserID = model.UserID, RatingValue = model.RatingValue };
                _ctx.Add(rating);
            }
            else
            {
                rating.RatingValue = model.RatingValue;
            }
            await _ctx.SaveChangesAsync();
        }

        public async Task<Movie> GetMovieByIdAsync(int movieID)
        {
            return await _ctx.Movies.FirstOrDefaultAsync(a => a.Id == movieID);
        }

        public async Task<List<Movie>> GetMoviesAsync(string title, int? year, Genre? genre)
        {

            var movies = await _ctx.Movies.Include(a => a.Ratings).ToListAsync();
            if (!string.IsNullOrWhiteSpace(title))
                movies = movies.FindAll(a => a.Title.ToUpper().Contains(title.ToUpper()));

            if (year.HasValue)
                movies = movies.FindAll(a => a.YearOfRelease == year);

            if (genre.HasValue)
                movies = movies.FindAll(a => a.Genres.HasFlag(genre));

            return movies;
        }

        public async Task<List<Movie>> GetTop5RatedByUserMoviesAsync(int userID)
        {
            //var result = await _ctx.Movies.Join(_ctx.Ratings,
            //    movie => movie.Id,
            //    rating => rating.MovieID,
            //    (movie, rating) =>
            //    new { Movie = movie, Rating = rating })
            //    .Where(a => a.Rating.UserID == userID)
            //    .Select(a => a.Movie).Include(a=>a.Ratings).OrderByDescending(a => a.Ratings.Sum(b => b.RatingValue)).ToListAsync();

            //return result;

            var result = (from c in _ctx.Movies
                          join b in _ctx.Ratings on c.Id equals b.MovieID
                          where b.UserID == userID
                          group new { c, b } by new { c.Id } into g
                          select new { g.Key, SumRating = g.Sum(a => a.b.RatingValue) })
                          .OrderByDescending(a => a.SumRating).Take(5);

            var movieIds = await result.Select(a => a.Key.Id).ToListAsync();

            return await _ctx.Movies.Include(a => a.Ratings).Where(a => movieIds.Contains(a.Id)).ToListAsync();
        }

        public async Task<List<Movie>> GetTop5RatedMoviesAsync()
        {
            var result = (from c in _ctx.Movies
                          join b in _ctx.Ratings on c.Id equals b.MovieID
                          group new {c, b } by new { c.Id } into g
                          select new { g.Key, SumRating = g.Sum(a=> a.b.RatingValue) })
                          .OrderByDescending(a => a.SumRating).Take(5);

            var movieIds = await result.Select(a => a.Key.Id).ToListAsync();

            return await _ctx.Movies.Include(a => a.Ratings).Where(a => movieIds.Contains(a.Id)).ToListAsync();
        }
    }
}
