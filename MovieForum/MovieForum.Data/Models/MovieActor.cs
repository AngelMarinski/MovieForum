using MovieForum.Data.Models.Interfaces;
using MovieForum.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieForum.Data.Models
{
    public class MovieActor : IHasId
    {
        public int Id { get; set; }

        [Required]
        public int MovieId { get; set; }

        [Required]
        public virtual Movie Movie { get; set; }

        [Required]
        public int ActorId { get; set; }

        [Required]
        public virtual Actor Actor { get; set; }
    }
}
