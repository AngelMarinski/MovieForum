using MovieForum.Data.Models.Interfaces;
using MovieForum.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieForum.Data.Models
{
    public class Rating : IHasId
    {
        public int Id { get; set; }

        [Required]
        public virtual int UserID { get; set; }

        [Required]
        public  int MovieId { get; set; }

        public virtual Movie Movie { get; set; }

        [Required]
        public int Rate { get; set; }
    }
}
