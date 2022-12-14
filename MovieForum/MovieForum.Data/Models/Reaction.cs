using MovieForum.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieForum.Data.Models
{
    public class Reaction : IHasId,IDeletable
    {
        [Key]
        public int Id { get; set; }        

        public int UserId { get; set; }

        public bool Liked { get; set; }

        public bool Disliked { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
