using MovieForum.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieForum.Data.Models
{
    public class Actor: IHasId, IDeletable
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }

        [Required]
        public virtual ICollection<MovieActor> Roles { get; set; }

        [Required]
        public bool IsDeleted { get ; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
