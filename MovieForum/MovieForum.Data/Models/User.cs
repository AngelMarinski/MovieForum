using MovieForum.Data.Models.Interfaces;
using MovieForum.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieForum.Data.Models
{
    public class User : IDeletable
    {
        public int Id { get; set; }

        [Required]
        [StringLength(int.MaxValue, MinimumLength = 4)]
        public string Username { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 4)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 4)]
        public string LastName { get; set; }

        [Required]
        [StringLength(int.MaxValue, MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        [EmailAddress()]
        public string Email { get; set; }

        [Required]
        public bool IsBlocked { get; set; }
        public bool IsEmailConfirmed { get; set; }

        public string ImagePath { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        public virtual Role Role { get; set; }

        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$")]
        public string PhoneNumber { get; set; }

        [Required]
        public bool IsDeleted { get ; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
