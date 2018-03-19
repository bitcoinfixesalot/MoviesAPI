using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using MoviesAPI.Controllers;
using MoviesAPI.Data;
using MoviesAPI.Data.Entities;
using MoviesAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MoviesAPITests
{
    public class UnitTest1
    {
        public UnitTest1()
        {

            MoviesContext = new MoviesContext(DbOptionsFactory.DbContextOptions);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Movie, MovieModel>().ForMember(a => a.AverageRating, opt => opt.Ignore());
            });

            MapperInstance = config.CreateMapper();
        }
        
        public IMapper MapperInstance { get; }
        public MoviesContext MoviesContext { get; }

        [Fact]
        public void TestMoviesControlerGetByYear()
        {
            var mock = new Mock<ILogger<MoviesController>>();
            ILogger<MoviesController> logger = mock.Object;

            MoviesController moviesController = new MoviesController(new UsersRepository(MoviesContext), new MoviesRepository(MoviesContext), logger, MapperInstance);

            var actionResult = moviesController.Get(null, 1994, null);
            actionResult.Wait();
            Assert.NotNull(actionResult);

            var okResult = actionResult.Result.Should().BeOfType<OkObjectResult>().Subject;

            var movies = okResult.Value.Should().BeAssignableTo<IEnumerable<MovieModel>>().Subject;

            movies.Count().Should().Be(2);
        }

        [Fact]
        public void TestMoviesControlerGetByGenre()
        {
            var mock = new Mock<ILogger<MoviesController>>();
            ILogger<MoviesController> logger = mock.Object;

            MoviesController moviesController = new MoviesController(new UsersRepository(MoviesContext), new MoviesRepository(MoviesContext), logger, MapperInstance);

            var actionResult = moviesController.Get(null, null, Genre.Drama);
            actionResult.Wait();
            Assert.NotNull(actionResult);

            var okResult = actionResult.Result.Should().BeOfType<OkObjectResult>().Subject;

            var movies = okResult.Value.Should().BeAssignableTo<IEnumerable<MovieModel>>().Subject;

            movies.Count().Should().Be(5);
        }

        [Fact]
        public void TestMoviesControlerGetTopFive()
        {
            var mock = new Mock<ILogger<MoviesController>>();
            ILogger<MoviesController> logger = mock.Object;

            MoviesController moviesController = new MoviesController(new UsersRepository(MoviesContext), new MoviesRepository(MoviesContext), logger, MapperInstance);

            var actionResult = moviesController.GetTop5RatedMovies();
            actionResult.Wait();
            Assert.NotNull(actionResult);

            var okResult = actionResult.Result.Should().BeOfType<OkObjectResult>().Subject;

            var movies = okResult.Value.Should().BeAssignableTo<IEnumerable<MovieModel>>().Subject;

            movies.Count().Should().Be(5);
        }

        [Fact]
        public void TestMoviesControlerGetTopFiveByUser()
        {
            var mock = new Mock<ILogger<MoviesController>>();
            ILogger<MoviesController> logger = mock.Object;

            MoviesController moviesController = new MoviesController(new UsersRepository(MoviesContext), new MoviesRepository(MoviesContext), logger, MapperInstance);

            var actionResult = moviesController.GetTop5RatedMoviesByUser(1);
            actionResult.Wait();
            Assert.NotNull(actionResult);

            var okResult = actionResult.Result.Should().BeOfType<OkObjectResult>().Subject;

            var movies = okResult.Value.Should().BeAssignableTo<IEnumerable<MovieModel>>().Subject;

            movies.Count().Should().Be(5);
        }
    }

    public static class DbOptionsFactory
    {
        static DbOptionsFactory()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .Build();
            var connectionString = config.GetConnectionString("MoviesConnectionString");

            DbContextOptions = new DbContextOptionsBuilder<MoviesContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        public static DbContextOptions<MoviesContext> DbContextOptions { get; }

    }
}
