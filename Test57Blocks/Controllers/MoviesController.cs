using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Test57Blocks.DTO;
using Test57Blocks.Models;
using Test57Blocks.Tools;

namespace Test57Blocks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MoviesController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly _57BlocksContext db;
        public MoviesController(IConfiguration configuration, _57BlocksContext context)
        {
            _configuration = configuration;
            db = context;
        }

        [HttpPut("Create")]
        public async Task<ActionResult<TransactionResultDTO>> Create([FromBody] MovieCreateDTO newMovie)
        {
            var idUsuario = User.Claims.FirstOrDefault(x => x.Type.Equals("idUser", StringComparison.InvariantCultureIgnoreCase));
            TransactionResultDTO result = new TransactionResultDTO();
            try
            {
                if (db.Genres.Where(x => x.IdGenre == newMovie.IdGenre).Any())
                {
                    Movie movie = new Movie();
                    movie.MovieTitle = newMovie.MovieTitle;
                    movie.Duration = newMovie.Duration;
                    movie.Rating = newMovie.Rating;
                    movie.IdGenre = newMovie.IdGenre;
                    movie.Private = newMovie.Private;
                    movie.IdUser = int.Parse(idUsuario.Value);
                    movie.CreationDate=DateTime.Now;
                    db.Movies.Add(movie);
                    await db.SaveChangesAsync();
                    result.Data = movie;
                    result.Success = true;
                    result.Message = "Movie created succesfully";
                }
                else
                {
                    result.Success = false;
                    result.Errors.Add("invalid genre");
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Errors.Add(ex.Message);
            }
            return  result;
        }
        
        [HttpPost("Edit")]
        public async Task<ActionResult<TransactionResultDTO>> Edit([FromBody] MovieEditDTO editMovie)
        {
            var idUsuario = User.Claims.FirstOrDefault(x => x.Type.Equals("idUser", StringComparison.InvariantCultureIgnoreCase));
            TransactionResultDTO result = new TransactionResultDTO();
            try
            {
                if (db.Genres.Where(x => x.IdGenre == editMovie.IdGenre).Any())
                {
                    Movie movie = db.Movies.Where(x=>x.IdMovie==editMovie.IdMovie).FirstOrDefault();
                    if (movie != null)
                    {
                        if (movie.IdUser == int.Parse(idUsuario.Value))
                        {
                            movie.MovieTitle = editMovie.MovieTitle;
                            movie.Duration = editMovie.Duration;
                            movie.Rating = editMovie.Rating;
                            movie.IdGenre = editMovie.IdGenre;
                            movie.Private = editMovie.Private;
                            movie.IdUser = int.Parse(idUsuario.Value);
                            movie.ModifiedDate = DateTime.Now;
                            await db.SaveChangesAsync();
                            result.Data = movie;
                            result.Success = true;
                            result.Message = "Movie edited succesfully";
                        }
                        else
                        {
                            result.Success = false;
                            result.Errors.Add("You dont have access to edit this movie");
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Errors.Add("invalid movie");
                    }
                }
                else
                {
                    result.Success = false;
                    result.Errors.Add("invalid genre");
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Errors.Add(ex.Message);
            }
            return result;
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult<TransactionResultDTO>> Delete([FromQuery] int idMovie)
        {
            var idUsuario = User.Claims.FirstOrDefault(x => x.Type.Equals("idUser", StringComparison.InvariantCultureIgnoreCase));
            TransactionResultDTO result = new TransactionResultDTO();
            try
            {
                Movie movie = db.Movies.Where(x => x.IdMovie == idMovie).FirstOrDefault();
                if (movie != null)
                {
                    if (movie.IdUser == int.Parse(idUsuario.Value))
                    {
                        db.Movies.Remove(movie);
                        await db.SaveChangesAsync();
                        result.Data = movie;
                        result.Success = true;
                        result.Message = "Movie deleted succesfully";
                    }
                    else
                    {
                        result.Success = false;
                        result.Errors.Add("You dont have access to delete this movie");
                    }
                }
                else
                {
                    result.Success = false;
                    result.Errors.Add("invalid movie");
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Errors.Add(ex.Message);
            }
            return result;
        }

        [HttpGet("GetMyMovies")]
        public async Task<ActionResult<List<MovieQueryDTO>>> GetMyMovies([FromQuery] PageDTO page)
        {
            var idUsuario = User.Claims.FirstOrDefault(x => x.Type.Equals("idUser", StringComparison.InvariantCultureIgnoreCase));
            var query = (from m in db.Movies
                            where m.IdUser == int.Parse(idUsuario.Value)
                            select new MovieQueryDTO()
                            {
                                IdMovie=m.IdMovie,
                                Duration=m.Duration,
                                MovieTitle=m.MovieTitle,
                                Rating=m.Rating,
                                IdGenre=m.IdGenre,
                                IdUser=m.IdUser,
                                Private=m.Private,
                                CreationDate=m.CreationDate,
                                ModifiedDate=m.ModifiedDate,
                                Genre=m.IdGenreNavigation.GenreName,
                                Owner=m.IdUserNavigation.UserName
                            }
                            ).AsQueryable();
            await HttpContext.InsertValueinHeader(query);
            var movies= await query.OrderBy(x=>x.MovieTitle).Page(page).ToListAsync();
            return movies;
        }

        [HttpGet("GetAllMovies")]
        public async Task<ActionResult<List<MovieQueryDTO>>> GetAllMovies([FromQuery] PageDTO page)
        {
            var query = (from m in db.Movies
                         where !m.Private
                         select new MovieQueryDTO()
                         {
                             IdMovie = m.IdMovie,
                             Duration = m.Duration,
                             MovieTitle = m.MovieTitle,
                             Rating = m.Rating,
                             IdGenre = m.IdGenre,
                             IdUser = m.IdUser,
                             Private = m.Private,
                             CreationDate = m.CreationDate,
                             ModifiedDate = m.ModifiedDate,
                             Genre = m.IdGenreNavigation.GenreName,
                             Owner = m.IdUserNavigation.UserName
                         }
                            ).AsQueryable();
            await HttpContext.InsertValueinHeader(query);
            var movies = await query.OrderBy(x => x.MovieTitle).Page(page).ToListAsync();
            return movies;
        }
    }
}
