using MovieForum.Data.Models.Interfaces;
using MovieForum.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieForum.Data.Models
{
    public class MovieTags : IDeletable
    {
      
        [Required]
        public  int? MovieId { get; set; }
        public virtual Movie Movie { get; set; }
    
        [Required]
        public  int? TagId { get; set; }
        public virtual Tag Tag { get; set; }

        [Required]
        public bool IsDeleted { get ; set ; }

        public DateTime? DeletedOn { get ; set ; }
    }
}
