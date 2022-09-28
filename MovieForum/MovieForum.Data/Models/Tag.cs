using MovieForum.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieForum.Data.Models
{
    public class Tag : ITag
    {
        public Tag(int id, string tagName)
        {
            this.TagName = tagName;
            this.Id = id;
        }
        public int Id { get; set; }

        [StringLength(20, MinimumLength = 2, ErrorMessage = "Comment title length must be between {0} and {1} characters!")]
        public string TagName { get; set; }

        [Required]
        public virtual ICollection<MovieTags> Movies { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
