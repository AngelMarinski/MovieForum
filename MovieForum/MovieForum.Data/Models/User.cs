using MovieForum.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieForum.Data.Models
{
    internal class User : IDeletable
    {
        public int Id { get; set; }

        [Required]
        [StringLength(int.MaxValue, MinimumLength = 4, ErrorMessage = "Username length must be greater than {1} character")]
        public string Username { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 4, ErrorMessage = "First Name must be greater between {1} and {0} character")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 4, ErrorMessage = "Last Name must be greater between {1} and {0} character")]
        public string LastName { get; set; }

        [Required]
        [StringLength(int.MaxValue, MinimumLength = 8, ErrorMessage = "Password length must be greater than {1} symbols")]
        public string Password { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        public virtual Role Role { get; set; }

        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        public bool IsDeleted { get ; set; }
        
        public DateTime? DeletedOn { get; set; }
    }
}
