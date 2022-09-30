using MovieForum.Data.Models.Interfaces;
using MovieForum.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieForum.Data.Models
{
    public class Comment : IHasId, IDeletable
    {
        public int Id { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 5, ErrorMessage = "Comment title length must be between {0} and {1} characters!")]
        public string Title { get; set; }

        [Required]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Comment content length must be between {0} and {1} characters!")]
        public string Content { get; set; }

        [Required]
        public int? AuthorId { get; set; }

        [Required]
        public virtual User Author { get; set; }

        [Required]
        public int? MovieId { get; set; }

        [Required]
        public virtual Movie Movie { get; set; }

        public int LikesCount { get; set; }


        public int DisLikesCount { get; set; }

        [Required]
        public DateTime? PostedOn { get; set; }

        [Required]
        public bool IsDeleted { get; set; }


        public DateTime? DeletedOn { get; set; }


        public Dictionary<int, string> LikeDislikeMap { get; set; }
    }
}

