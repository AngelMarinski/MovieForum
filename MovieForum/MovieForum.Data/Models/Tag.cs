using MovieForum.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieForum.Data.Models
{
    public class Tag : IHasId, IDeletable
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Tag name length must be between {0} and {1} characters!")]
        public string TagName { get; set; }

        public virtual ICollection<MovieTags> Movies { get; set; } = new List<MovieTags>();

        [Required]
        public bool IsDeleted { get; set; }
        
        public DateTime? DeletedOn { get; set; }
    }
}
