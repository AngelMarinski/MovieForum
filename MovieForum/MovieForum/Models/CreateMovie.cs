using Microsoft.AspNetCore.Http;
using MovieForum.Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieForum.Web.Models
{
    public class CreateMovie
    {
        [MinLength(Constants.MOVIE_TITLE_MIN_LENGHT), MaxLength(Constants.MOVIE_TITLE_MAX_LENGHT)]
        [Required]
        public string Title { get; set; }

        [MinLength(Constants.MOVIE_CONTENT_MIN_LENGHT), MaxLength(Constants.MOVIE_CONTENT_MAX_LENGHT)]
        [Required]
        public string Content { get; set; }

        public int AuthorId { get; set; }

        public string Username { get; set; }

        public DateTime RealeaseDate { get; set; }

        public int GenreId { get; set; }

        public string Cast { get; set; }

        public string Tags { get; set; }

        public IFormFile File { get; set; }
    }
}
