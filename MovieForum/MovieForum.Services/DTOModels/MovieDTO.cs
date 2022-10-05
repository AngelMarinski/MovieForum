using MovieForum.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieForum.Services.DTOModels
{
    public class MovieDTO
    {
        public int Id { get; set; }

        //TODO
        public int AuthorId { get; set; }

        public UserDTO Author { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime ReleaseDate { get; set; }

        public DateTime Posted { get; set; }

        public int GenreId { get; set; }

        public Genre Genre { get; set; }

        public ICollection<MovieActor> Cast { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<MovieTags> Tags { get; set; }

        public int Rating { get; set; }

        public int LikesCount { get; set; }

        public int DislikesCount { get; set; }
    }
}
