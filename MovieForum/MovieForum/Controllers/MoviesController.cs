using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MovieForum.Services.Interfaces;
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

        public MoviesController(IMoviesServices moviesServices, IWebHostEnvironment _webHostEnvironment)
        {
            this.moviesService = moviesServices;
            webHostEnvironment = _webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var movies = await this.moviesService.GetAsync();

            return View(movies);
        }
    }
}
