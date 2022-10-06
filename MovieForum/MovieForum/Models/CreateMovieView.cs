using MovieForum.Data.Models;
using MovieForum.Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieForum.Web.Models
{
    public class CreateMovieView
    {
        [MinLength(Constants.MOVIE_TITLE_MIN_LENGHT), MaxLength(Constants.MOVIE_TITLE_MAX_LENGHT)]
        public string Title { get; set; }

        [MinLength(Constants.MOVIE_CONTENT_MIN_LENGHT), MaxLength(Constants.MOVIE_TITLE_MAX_LENGHT)]
        public string Content { get; set; }

        public int AuthorId { get; set; }

        public DateTime RealeaseDate { get; set; }

        public Genre Genre { get; set; }

        public int Rating { get; set; }
    }
}
