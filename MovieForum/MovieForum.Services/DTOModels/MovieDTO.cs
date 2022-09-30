using MovieForum.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieForum.Services.DTOModels
{
    public class MovieDTO
    {
        public int Id { get; set; } 

        public int? AuthorId { get; set; }

        public string Title { get; set; }

        public DateTime ReleaseDate { get; set; }

        public DateTime Posted { get; set; }

        public int GenreId { get; set; }

        public virtual Genre Genre { get; set; }

        public virtual ICollection<MovieActor> Cast { get; set; }

        public virtual ICollection<MovieTags> Tags { get; set; }

        public int Rating { get; set; }

        public int LikesCount { get; set; }

        public int DislikesCount { get; set; }
    }
}
