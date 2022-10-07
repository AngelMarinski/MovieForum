using MovieForum.Data.Models.Interfaces;
using MovieForum.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieForum.Data.Models
{
    public class MovieActor : IDeletable
    {
        [Required]
        public int? MovieId { get; set; }
        public virtual Movie Movie {get;set;}

        [Required]
        public int? ActorId { get; set; }
        public virtual Actor Actor { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
