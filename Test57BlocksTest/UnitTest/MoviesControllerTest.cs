using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test57Blocks.Controllers;
using Test57Blocks.DTO;
using Test57Blocks.Models;

namespace Test57BlocksTest.UnitTest
{
    [TestClass]
    public  class MoviesControllerTest
    {
        public MoviesController BuildController(string dbName, Movie movie=null, Genre genre=null)
        {
            var context = TestDatabase.BuildDbContext(dbName);
            var config = new Dictionary<string, string>
            {
                { "JWTKey","ALSFNIGIDFBVIUBFVDNFGIURTGBMASDNDCFOIEURHG34958U0FSDNGF9384Y8723Y34354TWEFSDFSF&%$%(325EWFSDDFHFGJFGXCVSDFJSUDFVSUJCFA" }
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(config)
                .Build();
            if (genre != null)
            {
                context.Genres.Add(genre);
                context.SaveChanges();
            }
            if (movie != null)
            {
                context.Movies.Add(movie);
                context.SaveChanges();
            }
            MoviesController controller =new MoviesController(configuration, context);
            controller.ControllerContext = TestDatabase.BuildControllerContext();
            return controller;
        }

        [TestMethod]
        public async Task CreateMovieSuccess()
        {
            string dataBaseName = Guid.NewGuid().ToString();
            MovieCreateDTO movie = new MovieCreateDTO();
            movie.MovieTitle = "Terminator";
            movie.Duration = 130;
            movie.IdGenre = 1;
            movie.Private = false;
            movie.Rating = 5;
            var controller = BuildController(dataBaseName,null, new Genre() {IdGenre=1, GenreName="Action" });
            
            var answer = await controller.Create(movie);
            TransactionResultDTO  result = answer.Value;

            Assert.AreEqual("Movie created succesfully", result.Message);
        }

        [TestMethod]
        public async Task EditMovieSuccess()
        {
            string dataBaseName = Guid.NewGuid().ToString();
            MovieEditDTO movie = new MovieEditDTO();
            movie.MovieTitle = "Terminator";
            movie.Duration = 130;
            movie.IdGenre = 1;
            movie.Private = false;
            movie.Rating = 5;
            movie.IdMovie = 1;
            var controller = BuildController(dataBaseName,
                new Movie() {MovieTitle="Back to the future", Duration=5, IdGenre=1, Private=true, Rating=8, IdUser=1 , IdMovie=1}, 
                new Genre() { IdGenre = 1, GenreName = "Action" });

            var answer = await controller.Edit(movie);
            TransactionResultDTO result = answer.Value;

            Assert.AreEqual("Movie edited succesfully", result.Message);
        }

        [TestMethod]
        public async Task EditMovieFailByUnauthorize()
        {
            string dataBaseName = Guid.NewGuid().ToString();
            MovieEditDTO movie = new MovieEditDTO();
            movie.MovieTitle = "Terminator";
            movie.Duration = 130;
            movie.IdGenre = 1;
            movie.Private = false;
            movie.Rating = 5;
            movie.IdMovie = 1;
            var controller = BuildController(dataBaseName,
                new Movie() { MovieTitle = "Back to the future", Duration = 5, IdGenre = 1, Private = true, Rating = 8, IdUser = 2, IdMovie = 1 },
                new Genre() { IdGenre = 1, GenreName = "Action" });

            var answer = await controller.Edit(movie);
            TransactionResultDTO result = answer.Value;

            Assert.IsNotNull(result.Errors);
            Assert.AreEqual("You dont have access to edit this movie", result.Errors[0]);
        }

        [TestMethod]
        public async Task EditMovieFailInvalidMovie()
        {
            string dataBaseName = Guid.NewGuid().ToString();
            MovieEditDTO movie = new MovieEditDTO();
            movie.MovieTitle = "Terminator";
            movie.Duration = 130;
            movie.IdGenre = 1;
            movie.Private = false;
            movie.Rating = 5;
            movie.IdMovie = 15;
            var controller = BuildController(dataBaseName,
                new Movie() { MovieTitle = "Back to the future", Duration = 5, IdGenre = 1, Private = true, Rating = 8, IdUser = 1, IdMovie = 1 },
                new Genre() { IdGenre = 1, GenreName = "Action" });

            var answer = await controller.Edit(movie);
            TransactionResultDTO result = answer.Value;

            Assert.IsNotNull(result.Errors);
            Assert.AreEqual("invalid movie", result.Errors[0]);
        }

        [TestMethod]
        public async Task DeleteMovieSuccess()
        {
            string dataBaseName = Guid.NewGuid().ToString();
           
            var controller = BuildController(dataBaseName,
                new Movie() { MovieTitle = "Back to the future", Duration = 5, IdGenre = 1, Private = true, Rating = 8, IdUser = 1, IdMovie = 1 },
                new Genre() { IdGenre = 1, GenreName = "Action" });

            var answer = await controller.Delete(1);
            TransactionResultDTO result = answer.Value;

            Assert.AreEqual("Movie deleted succesfully", result.Message);
        }

        [TestMethod]
        public async Task DeleteMovieFailByUnauthorize()
        {
            string dataBaseName = Guid.NewGuid().ToString();
                       var controller = BuildController(dataBaseName,
                new Movie() { MovieTitle = "Back to the future", Duration = 5, IdGenre = 1, Private = true, Rating = 8, IdUser = 2, IdMovie = 1 },
                new Genre() { IdGenre = 1, GenreName = "Action" });

            var answer = await controller.Delete(1);
            TransactionResultDTO result = answer.Value;

            Assert.IsNotNull(result.Errors);
            Assert.AreEqual("You dont have access to delete this movie", result.Errors[0]);
        }

        [TestMethod]
        public async Task DeleteMovieFailInvalidMovie()
        {
            string dataBaseName = Guid.NewGuid().ToString();
            
            var controller = BuildController(dataBaseName,
                new Movie() { MovieTitle = "Back to the future", Duration = 5, IdGenre = 1, Private = true, Rating = 8, IdUser = 1, IdMovie = 1 },
                new Genre() { IdGenre = 1, GenreName = "Action" });

            var answer = await controller.Delete(54);
            TransactionResultDTO result = answer.Value;

            Assert.IsNotNull(result.Errors);
            Assert.AreEqual("invalid movie", result.Errors[0]);
        }
    }
}
