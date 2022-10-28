using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Interfaces;
using MovieForum.Web.Models;
using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> Movie(int id)
        {
            var movie = await this.moviesService.GetByIdAsync(id);

            return View(new MovieCommentWrap { MovieDTO = movie , commentViewModel = new CreateCommentViewModel() });
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var movies = await this.moviesService.GetAsync();

            return View(movies);
        }

        [HttpGet]
        public IActionResult Create()
        {
            CreateMovie movie = new CreateMovie();

            return this.View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMovie movie)
        {
            var user = await userService.GetUserByEmailAsync(this.User.Identity.Name);

            var cast = movie.Cast.Split(',').ToList();

            var castList = new List<MovieActorDTO>();

            foreach(var x in cast)
            {
                var full_name = x.Split(' ').ToList();

                var actor = new ActorDTO
                {
                    FirstName = full_name[0],
                    LastName = full_name[1]
                };

                var movieActor = new MovieActorDTO
                {
                    Actor = actor
                };

                castList.Add(movieActor);
            }

            var movieDTO = new MovieDTO
            {
                Title = movie.Title,
                Content = movie.Content,
                ReleaseDate = movie.RealeaseDate,
                GenreId = movie.GenreId,
                Username = user.Username,
                Cast = castList
            };

            await this.moviesService.PostAsync(movieDTO);

            return this.RedirectToAction("Index", "Movies");
        }
    }
}
