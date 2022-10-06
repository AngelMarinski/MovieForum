using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieForum.Services.Interfaces;
using MovieForum.Web.Models;
using MovieForum.Services.DTOModels;
using MovieForum.Data.Models;

namespace MovieForum.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesApiController : ControllerBase
    {
        private readonly IMoviesServices moviesService;

        public MoviesApiController(IMoviesServices moviesServices)
        {
            this.moviesService = moviesServices;
        }


        [HttpGet("")]
        public async Task<IActionResult> GetMoviesAsync()
        {
            try
            {
                var movies = await this.moviesService.GetAsync();

                return this.Ok(movies);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieAsync(int id)
        {
            try
            {
                var movie = await this.moviesService.GetByIdAsync(id);

                return this.Ok(movie);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        //connect post with user
        [HttpPost("create")]
        public async Task<IActionResult> CreateMovieAsync([FromBody] CreateMovieView movie)
        {
            try
            {
                var movieDto = new MovieDTO
                {
                    AuthorId = movie.AuthorId,
                    Username = movie.Username,
                    Title = movie.Title,
                    Content = movie.Content,
                    ReleaseDate = movie.RealeaseDate,
                    Posted = DateTime.Now,
                    GenreId = movie.GenreId,
                };

                var post = await this.moviesService.PostAsync(movieDto);

                return this.Ok(post);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieAsync(int id)
        {
            try
            {
                return this.Ok(await this.moviesService.DeleteAsync(id));
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        //add authorization
/*        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovieAsync(int id, [FromBody] )*/
    }
}
