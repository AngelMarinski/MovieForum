using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieForum.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public IActionResult Get()
        {
         
            return Ok("Hello Maggie, How are ya?");
        }
    }
}
