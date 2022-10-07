using MovieForum.Data.Models;
using MovieForum.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieForum.Models
{
    public class Movie : IHasId, IDeletable
    {
        public int Id { get; set; }

        [Required]
        public int? AuthorID { get; set; }

        [Required]
        public virtual User Author { get; set; }

        [Required, StringLength(64, MinimumLength = 2)]
        public string Title { get; set; }

        [Required, StringLength(64, MinimumLength = 32)]
        public string Content { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        public DateTime Posted { get; set ; }

        [Required]
        public int GenreId { get; set; }

        [Required]
        public virtual Genre Genre { get; set; }

        public virtual ICollection<MovieActor> Cast { get; set; } = new List<MovieActor>();

        public virtual ICollection<MovieTags> Tags { get; set; } = new List<MovieTags>();

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        [Required]
        public virtual ICollection<Rating> Rating { get; set; } = new List<Rating>();

        [Required]
        public bool IsDeleted { get ; set ; }

        public DateTime? DeletedOn { get; set; }

    }
}
