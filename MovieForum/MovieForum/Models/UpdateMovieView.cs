using Microsoft.AspNetCore.Http;
using MovieForum.Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MovieForum.Web.Models
{
    public class UpdateMovieView
    {
        public int MovieID { get; set; }

        [MinLength(Constants.MOVIE_TITLE_MIN_LENGHT), MaxLength(Constants.MOVIE_TITLE_MAX_LENGHT)]
        [AllowNull]
        public string Title { get; set; }

        [MinLength(Constants.MOVIE_CONTENT_MIN_LENGHT), MaxLength(Constants.MOVIE_CONTENT_MAX_LENGHT)]
        [AllowNull]
        public string Content { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public int? GenreId { get; set; }

        public string Cast { get; set; }

        public string Tags { get; set; }

        public IFormFile File { get; set; }
    }
}
