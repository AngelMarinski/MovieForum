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

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        public DateTime Posted { get; set ; }

        [Required]
        public int GenreId { get; set; }

        [Required]
        public virtual Genre Genre { get; set; }

        public virtual ICollection<MovieActor> Cast { get; set; }

        public virtual ICollection<MovieTags> Tags { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public bool IsDeleted { get ; set ; }

        public DateTime? DeletedOn { get; set; }

    }
}
