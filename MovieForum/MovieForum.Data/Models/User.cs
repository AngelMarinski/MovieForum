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
        
        [MinLength(3)]
        public string Username { get; set; }

        [MinLength(4), MaxLength(32)]
        public string FirstName { get; set; }
        
        [MinLength(4), MaxLength(32)]
        public string LastName { get; set; }

        [MinLength(8)]
        public string Password { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public virtual Role Role { get; set; }

        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone number")]
        public string PhoneNumber { get; set; }

        public bool IsDeleted { get ; set; }
        
        public DateTime? DeletedOn { get; set; }
    }
}
