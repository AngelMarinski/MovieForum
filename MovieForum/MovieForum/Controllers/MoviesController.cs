using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Interfaces;
using MovieForum.Services.Models;
using MovieForum.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MovieForum.Web.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMoviesServices moviesService;
        private static IWebHostEnvironment webHostEnvironment;

        //update this
        private readonly IUserServices userService;

        public MoviesController(IMoviesServices moviesServices, IWebHostEnvironment _webHostEnvironment, IUserServices userService)
        {
            this.moviesService = moviesServices;
            webHostEnvironment = _webHostEnvironment;
            this.userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Movie(int id)
        {
            try
            {
               var movie = await this.moviesService.GetByIdAsync(id);

                return View(new MovieCommentWrap { MovieDTO = movie, commentViewModel = new CommentDTO(),
                rateMovieView = new RateMovieView()});
            }
            catch(InvalidOperationException ex)
            {
                return this.View("Error", new ErrorViewModel
                {
                    RequestId = ex.Message
                });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(MovieQueryParameters parameters)
        {
            var movies = await this.moviesService.FilterByAsync(parameters);

            this.ViewData["SortBy"] = parameters.SortBy;
            this.ViewData["Rating"] = parameters.MinRating;
            this.ViewData["Genre"] = parameters.Genre;
            this.ViewData["PageSize"] = parameters.PageSize;
            this.ViewData["Title"] = parameters.Title;
            this.ViewData["Tag"] = parameters.Tag;
            this.ViewData["Username"] = parameters.Username;
            this.ViewData["Year"] = parameters.Year;

            return View(movies);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            CreateMovie movie = new CreateMovie();

            return this.View(movie);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateMovie movie)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(movie);
            }

            var user = await userService.GetUserByEmailAsync(this.User.Identity.Name);

            var movieDTO = new MovieDTO
            {
                Title = movie.Title,
                Content = movie.Content,
                ReleaseDate = movie.RealeaseDate,
                Posted = DateTime.Now,
                GenreId = movie.GenreId,
                Username = user.Username,
                ImagePath = this.UploadPhoto(movie.File) ?? "Images/default.jpg"
            };

            var newMovie = new MovieDTO();
            try
            {
                newMovie = await this.moviesService.PostAsync(movieDTO);
            }
            catch (Exception ex)
            {
                return this.View("Error", new ErrorViewModel
                {
                    RequestId = ex.Message
                });
            }

            foreach (var cast in movie.Cast.Split(",").ToList())
            {
                var full_name = cast.Split(' ').ToList();

                var lastname = string.Join(" ", full_name.Skip(1).Take(full_name.Count - 1));

                await this.moviesService.AddActorAsync(newMovie.Id, full_name[0], lastname);
            }

            foreach (var tag in movie.Tags.Split(",").ToList())
            {
                await this.moviesService.AddTagAsync(newMovie.Id, tag);
            }

            return this.RedirectToAction("Index", "Movies");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(MovieCommentWrap movie)
        {
            try
            {
                await this.moviesService.DeleteAsync(movie.MovieDTO.Id);
                return this.RedirectToAction("Index", "Movies");
            }
            catch (InvalidOperationException ex)
            {
                return this.View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var movie = await this.moviesService.GetByIdAsync(id);

            UpdateMovieView post = new UpdateMovieView
            {
                MovieID = movie.Id,
                Title = movie.Title,
                Content = movie.Content,
                GenreId = movie.GenreId,
                ReleaseDate = movie.ReleaseDate
            };

            post.Cast = string.Join(",", movie.Cast.Select(x => x.ToString()));
            post.Tags = string.Join(",", movie.Tags.Select(x => x.TagName));

            return this.View(post);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(UpdateMovieView post)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(post);
            }

            try
            {
                var movie = await this.moviesService.GetByIdAsync(post.MovieID);
                var path = this.UploadPhoto(post.File);

                var movieDTO = new MovieDTO
                {
                    Id = post.MovieID,
                    Title = post.Title ?? movie.Title,
                    Content = post.Content ?? movie.Content,
                    GenreId = (int)post.GenreId,
                };

                if (post.Cast != null || post.Cast.Length != 0)
                {
                    var cast = post.Cast.Split(",").ToList();
                    foreach(var x in movie.Cast)
                    {
                        var name = string.Format($"{x.Actor.FirstName} {x.Actor.LastName}");
                        if (!cast.Any(y => y == name))
                        {
                            await this.moviesService.RemoveActorAsync(movie.Id, x.Actor.FirstName, x.Actor.LastName);
                        }
                    }

                    foreach (var x in post.Cast.Split(","))
                    {
                        var full_name = x.Split(' ').ToList();

                        var lastname = string.Join(" ", full_name.Skip(1).Take(full_name.Count - 1));
                        
                        if(!movie.Cast.Any(actor => actor.Actor.FirstName == full_name[0] 
                                            && actor.Actor.LastName == lastname))
                        {
                            await this.moviesService.AddActorAsync(movie.Id, full_name[0], lastname);
                        }
                    }
                }
                if (post.Tags != null)
                {
                    var tags = post.Tags.Split(",").ToList();


                    foreach (var item in movie.Tags)
                    {
                        if (!tags.Any(y => y == item.TagName))
                        {
                            await this.moviesService.RemoveTagAsync(movie.Id, item.TagName);
                        }
                    }

                    foreach (var tag in tags)
                    {
                        if(!movie.Tags.Any(x => x.TagName == tag))
                        {
                            await this.moviesService.AddTagAsync(post.MovieID, tag);
                        }
                    }

                }

                if(post.ReleaseDate != null)
                {
                    movieDTO.ReleaseDate = (DateTime)post.ReleaseDate;
                }

                if (path != null)
                {
                    movieDTO.ImagePath = path;
                }

                try
                {
                    await this.moviesService.UpdateAsync(post.MovieID, movieDTO);
                }
                catch(Exception ex)
                {
                    return this.View("Error", new ErrorViewModel { RequestId = ex.Message });
                }

                return this.RedirectToAction("Movie", "Movies", new { id = post.MovieID });
            }
            catch (InvalidOperationException ex)
            {
                return this.View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Rate(MovieCommentWrap rate)
        {
            try
            {
                await this.moviesService.RateMovieAsync(rate.rateMovieView.MovieId,
                                                    rate.rateMovieView.UserId,
                                                     rate.rateMovieView.Rate);

                return this.RedirectToAction("Movie", "Movies",
                    new { id = rate.rateMovieView.MovieId });
            }
            catch (InvalidOperationException ex)
            {
                return this.View("Error", new ErrorViewModel
                {
                    RequestId = ex.Message
                });
            }
            catch(UnauthorizedAccessException ex)
            {
                return this.View("Error", new ErrorViewModel
                {
                    RequestId = ex.Message
                });
            }
        }


        private List<MovieActorDTO> AddCastToMovie(List<string> cast)
        {
            var castList = new List<MovieActorDTO>();

            foreach (var x in cast)
            {
                var full_name = x.Split(' ').ToList();

                var lastname = string.Join(" ", full_name.Skip(1).Take(full_name.Count - 1));

                var actor = new ActorDTO
                {
                    FirstName = full_name[0],
                    LastName = lastname,
                };

                var movieActor = new MovieActorDTO
                {
                    Actor = actor
                };

                castList.Add(movieActor);
            }

            return castList;
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
