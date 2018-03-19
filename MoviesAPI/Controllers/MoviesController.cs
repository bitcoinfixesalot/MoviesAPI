using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoviesAPI.Data;
using MoviesAPI.Data.Entities;
using MoviesAPI.ViewModels;

namespace MoviesAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Movies")]
    public class MoviesController : Controller
    {
        public IUsersRepository UsersRepository { get; }
        public IMoviesRepository MoviesRepository { get; }
        public ILogger<MoviesController> Logger { get; }
        public IMapper Mapper { get; }

        public MoviesController(IUsersRepository  usersRepository,IMoviesRepository moviesRepository, ILogger<MoviesController> logger, IMapper mapper)
        {
            UsersRepository = usersRepository;
            MoviesRepository = moviesRepository;
            Logger = logger;
            Mapper = mapper;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<IActionResult> Get(string title, int? year, Genre? genre)
        {
            if (!ModelState.IsValid) return BadRequest();
            if (string.IsNullOrWhiteSpace(title) && !year.HasValue && !genre.HasValue)
                return BadRequest();
            try
            {
                var movies = await MoviesRepository.GetMoviesAsync(title, year, genre);
                if (movies.Count == 0)
                    return NotFound();

                List<MovieModel> movieModels = new List<MovieModel>();
                foreach (var movie in movies)
                {
                    var model = Mapper.Map<Movie, MovieModel>(movie);
                    model.AverageRating = movie.CalculateAvarageRating();
                    movieModels.Add(model);
                }
                return Ok(movieModels);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to get movies: {ex}");
                return  BadRequest("Failed to get movies");
            }
        }

        [HttpGet("top5")]
        public async Task<IActionResult> GetTop5RatedMovies()
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {
                var movies = await MoviesRepository.GetTop5RatedMoviesAsync();
                if (movies.Count == 0)
                    return NotFound();

                List<MovieModel> movieModels = new List<MovieModel>();
                foreach (var movie in movies)
                {
                    var model = Mapper.Map<Movie, MovieModel>(movie);
                    model.AverageRating = movie.CalculateAvarageRating();
                    movieModels.Add(model);
                }
                return Ok(movieModels.OrderByDescending(a=>a.AverageRating));
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to get movies: {ex}");
                return BadRequest("Failed to get movies");
            }
        }

        [HttpGet("{userId}", Name ="Get")]
        public async Task<IActionResult> GetTop5RatedMoviesByUser(int userId)
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {
                var movies = await MoviesRepository.GetTop5RatedByUserMoviesAsync(userId);
                if (movies.Count == 0)
                    return NotFound();

                List<MovieModel> movieModels = new List<MovieModel>();
                foreach (var movie in movies)
                {
                    var model = Mapper.Map<Movie, MovieModel>(movie);
                    model.AverageRating = movie.CalculateAvarageRating();
                    movieModels.Add(model);
                }
                return Ok(movieModels.OrderByDescending(a => a.AverageRating));
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to get movies: {ex}");
                return BadRequest("Failed to get movies");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]RatingModel model)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                var user =  await UsersRepository.GetUserByIdAsync(model.UserID);
                if (user == null) return NotFound();

                var movie = await MoviesRepository.GetMovieByIdAsync(model.MovieID);
                if (movie == null) return NotFound();

                await MoviesRepository.AddOrUpdateMovieRatingAsync(model);

                return Ok();
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to update rating: {ex}");
                return BadRequest("Failed to update rating:");
            }
        }
    }
}
