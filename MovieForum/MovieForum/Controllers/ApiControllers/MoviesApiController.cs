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
using MovieForum.Services.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace MovieForum.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesApiController : ControllerBase
    {
        private readonly IMoviesServices moviesService;
        private static IWebHostEnvironment webHostEnvironment;


        public MoviesApiController(IMoviesServices moviesServices, IWebHostEnvironment _webHostEnvironment)
        {
            this.moviesService = moviesServices;
            webHostEnvironment = _webHostEnvironment;
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

        [HttpGet("filtered")]
        public async Task<IActionResult> GetFilteredMoviesAsync([FromQuery] MovieQueryParameters parameters)
        {
            try
            {
                var movies = await this.moviesService.FilterByAsync(parameters);

                return this.Ok(movies);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
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

        [HttpGet("topCommented")]
        public async Task<IActionResult> GetTopCommentedAsync()
        {
            try
            {
                var movies = await this.moviesService.GetTopCommentedAsync();

                return this.Ok(movies);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpGet("mostRecent")]
        public async Task<IActionResult> GetMostRecentlyPostedAsync()
        {
            try
            {
                var movies = await this.moviesService.GetMostRecentPostsAsync();

                return this.Ok(movies);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }


        //connect post with user
        [HttpPost("create")]
        public async Task<IActionResult> CreateMovieAsync([FromForm] CreateMovieView movie)
        {
            try
            {
                var path = this.UploadPhoto(movie.File);

                var movieDto = new MovieDTO
                {
                    AuthorId = movie.AuthorId,
                    Username = movie.Username,
                    Title = movie.Title,
                    Content = movie.Content,
                    ReleaseDate = movie.RealeaseDate,
                    Posted = DateTime.Now,
                    GenreId = movie.GenreId,
                    Cast = movie.Cast,
                    Tags = movie.Tags
                };

                if (path != null)
                {
                    movieDto.ImagePath = path;
                }

                var post = await this.moviesService.PostAsync(movieDto);

                return this.Ok(post);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditPostAsync(int id, [FromForm] UpdatePostViewModel post)
        {
            try
            {
                var path = this.UploadPhoto(post.File);

                var movieDTO = new MovieDTO
                {
                    Title = post.Title,
                    Content = post.Content,
                    Cast = post.Cast == null ? null : new List<MovieActorDTO>(post.Cast),
                    Tags = post.Tags == null ? null : new List<MovieTagsDTO>(post.Tags),
                };


                if (post.GenreId != null)
                {
                    movieDTO.GenreId = (int)post.GenreId;
                }

                if (post.ReleaseDate != null)
                {
                    movieDTO.ReleaseDate = (DateTime)post.ReleaseDate;
                }

                if (path != null)
                {
                    movieDTO.ImagePath = path;
                }

                var movie = await this.moviesService.UpdateAsync(id, movieDTO);

                if (post.Cast != null)
                {
                    foreach (var item in post.Cast)
                    {
                        await this.moviesService.AddActorAsync(movie.Id, item.Actor.FirstName,
                            item.Actor.LastName);
                    }
                }
                if (post.Tags != null)
                {
                    foreach (var item in movie.Tags)
                    {
                        if (!post.Tags.Any(y => y.TagName == item.TagName))
                        {
                            await this.moviesService.RemoveTagAsync(movie.Id, item.TagName);
                        }
                    }

                    foreach (var tag in post.Tags)
                    {
                        if (!movie.Tags.Any(x => x.TagName == tag.TagName))
                        {
                            await this.moviesService.AddTagAsync(movie.Id, tag.TagName);
                        }
                    }
                }

                return this.Ok(movie);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpPut("/movie/addTag/{id}")]
        public async Task<IActionResult> AddTagAsync(int id, [FromBody] string tagName)
        {
            //add authentication
            try
            {
                var movieDTO = await moviesService.AddTagAsync(id, tagName);

                return this.Ok(movieDTO);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        //Put or Delete Query ?
        [HttpPut("/movie/removeTag/{id}")]
        public async Task<IActionResult> RemoveTagAsync(int id, [FromBody] string tagName)
        {
            //add authentication
            try
            {
                var movieDTO = await moviesService.RemoveTagAsync(id, tagName);

                return this.Ok(movieDTO);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
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


        [HttpPut("rate/movie/{id}")]
        public async Task<IActionResult> RateMovieAsync(int id, [FromBody] RateMovieViewModel rate)
        {
            try
            {
                var movie = await this.moviesService.RateMovieAsync(id, rate.UserId, rate.Rate);

                return this.Ok(movie);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        private string UploadPhoto(IFormFile file)
        {
            if (file == null)
            {
                return null;
            }
            FileInfo fi = new FileInfo(file.FileName);
            var newFileName = "Image_" + DateTime.Now.TimeOfDay.Milliseconds + fi.Extension;
            if (!Directory.Exists(webHostEnvironment.WebRootPath + "\\Images\\"))
            {
                Directory.CreateDirectory(webHostEnvironment.WebRootPath + "\\Images\\");
            }
            var path = Path.Combine("", webHostEnvironment.WebRootPath + "\\Images\\" + newFileName);
            using (FileStream stream = System.IO.File.Create(path))
            {
                file.CopyTo(stream);
                stream.Flush();
            }
            return $"Images/{newFileName}";
        }
    }
}
