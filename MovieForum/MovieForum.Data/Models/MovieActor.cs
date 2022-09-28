using MovieForum.Data.Models.Interfaces;
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
        public virtual int MovieId { get; set; }

        [Required]
        public virtual int ActorId { get; set; }
    }
}
