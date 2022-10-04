using MovieForum.Data.Models.Interfaces;
using MovieForum.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MovieForum.Data.Models
{
    public class Comment : IHasId, IDeletable
    {

        public int Id { get; set; }
                
        [Required]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Comment content length must be between {0} and {1} characters!")]
        [MinLength(2),MaxLength(2000)]
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


        public virtual ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    }
}

