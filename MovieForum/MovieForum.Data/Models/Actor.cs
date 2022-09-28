using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieForum.Data.Models
{
    public class Actor
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }

        [Required]
        public virtual ICollection<MovieActor> Roles { get; set; }
    }
}
