using Microsoft.EntityFrameworkCore;
using MoviesAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Data
{
    public class DataSeeder
    {
        private readonly MoviesContext _ctx;

        public DataSeeder(MoviesContext moviesContext)
        {
            this._ctx = moviesContext;
        }

        public async Task Seed()
        {
            _ctx.Database.EnsureCreated();

            if (_ctx.Users.Any()) return;

            var users = new User[]
                {
                    new User{  Email = "john@hotmail.com", UserName ="John"},
                    new User { Email = "colin@gmail.com", UserName = "Colin"},
                    new User { Email = "aaron@gmail.com", UserName = "Aaron"},
                    new User { Email = "anna@outlook.com", UserName = "Anna"},
                    new User { Email = "tom@live.com", UserName = "Tom"},
                    new User { Email = "jamesmc@outlook.com", UserName = "James"}
                };

            await _ctx.AddRangeAsync(users);
            await _ctx.SaveChangesAsync();

            var movies = new Movie[]
                {
                    new Movie{ Title = "Pulp Fiction", YearOfRelease = 1994, RunningTime = 120, Genres = Genre.Action | Genre.Comedy},
                    new Movie { Title = "Fight Club", YearOfRelease = 1999, RunningTime = 139, Genres = Genre.Drama},
                    new Movie { Title = "The Matrix ", YearOfRelease = 1999, RunningTime = 136, Genres = Genre.Action | Genre.SciFi },
                    new Movie { Title = "The Shawshank Redemption", YearOfRelease = 1994, RunningTime = 142, Genres = Genre.Crime | Genre.Drama},
                    new Movie { Title = "The Big Lebowski", YearOfRelease = 1998, RunningTime = 115, Genres = Genre.Comedy},
                    new Movie { Title = "Watchmen", YearOfRelease = 2009, RunningTime = 160, Genres = Genre.SciFi},
                    new Movie { Title = "Thank You for Smoking", YearOfRelease = 2005, RunningTime = 99, Genres = Genre.Drama},
                    new Movie { Title = "The Hitman's Bodyguard", YearOfRelease = 2017, RunningTime = 120, Genres = Genre.Action},
                    new Movie { Title = "V for Vendetta", YearOfRelease = 2005, RunningTime = 130, Genres = Genre.Drama | Genre.Action},
                    new Movie { Title = "Fifty Shades of Grey", YearOfRelease = 2015, RunningTime = 250, Genres = Genre.Drama}
                };

            await _ctx.AddRangeAsync(movies);
            await _ctx.SaveChangesAsync();

            var someUsers = _ctx.Users.Take(3);
            var somemovies = _ctx.Movies.Where(a => !a.Title.Contains("Fifty"));
            var worstMovie = _ctx.Movies.FirstOrDefault(a => a.Title.Contains("Fifty"));

            var random = new Random(1);
            foreach (var user in someUsers)
            {
                foreach (var movie in somemovies)
                {
                    var rate = random.Next(1, 6);
                    var rating = new Rating { MovieID = movie.Id, UserID = user.Id, RatingValue = rate };

                    _ctx.Add(rating);
                }

                if (worstMovie != null)
                {
                    var rating = new Rating { MovieID = worstMovie.Id, UserID = user.Id, RatingValue = 1 };

                    _ctx.Add(rating);
                }
            }

            await _ctx.SaveChangesAsync();
        }
    }
}
