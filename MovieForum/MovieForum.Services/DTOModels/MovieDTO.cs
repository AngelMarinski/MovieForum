using MovieForum.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieForum.Services.DTOModels
{
    public class MovieDTO
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }

        public string Username { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime ReleaseDate { get; set; }

        public DateTime? Posted { get; set; }

        public int GenreId { get; set; }

        public Genre Genre { get; set; }

        public ICollection<MovieActorDTO> Cast { get; set; } = new List<MovieActorDTO>();

        public ICollection<CommentDTO> Comments { get; set; } = new List<CommentDTO>();

        public ICollection<MovieTagsDTO> Tags { get; set; } = new List<MovieTagsDTO>(); 

        public double Rating { get; set; }

        public string ImagePath { get; set; }
    }
}
