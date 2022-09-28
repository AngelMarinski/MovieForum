using MovieForum.Data.Models;
using MovieForum.Data.Models.Interfaces;
using MovieForum.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieForum.Models
{
    public class Movie : IHasId
    {
        public int Id { get; set; }

        [Required]
        public int AuthorID { get; set; }

        [Required]
        public virtual User Author { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        public DateTime Posted { get; set ; }

        [Required]
        public Genres Genre { get; set; }

        [Required]
        public virtual ICollection<MovieActor> Cast { get; set; }

        [Required]
        public virtual ICollection<MovieTags> Tags { get; set; }

        [Required]
        public int Rating { get; set; }

        public int LikesCount { get; set; }

        public int DislikesCount { get; set ; }

        [Required]
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
