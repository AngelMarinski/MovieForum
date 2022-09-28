using MovieForum.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieForum.Models
{
    public class Movie : IMovie
    {
        public int Id { get; set; }
        public int AuthorID { get; set; }

        public string Title { get; set; }

        public DateTime ReleaseDate { get; set; }

        public DateTime Posted { get; set ; }

        public Genres Genre { get; set; }

        [NotMapped]
        public List<string> Actors { get; set; }

        public int Rating { get; set; }

        public int LikesCount { get; set; }

        public int DislikesCount { get; set ; }
    }
}
