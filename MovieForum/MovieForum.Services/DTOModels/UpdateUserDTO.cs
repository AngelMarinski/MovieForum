using MovieForum.Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieForum.Services.DTOModels
{
    public class UpdateUserDTO
    {
        public int UserId { get; set; }
        [Required]
        [MinLength(Constants.USER_USERNAME_MIN_LENGTH)]
        public string Username { get; set; }

        [Required]
        [MaxLength(Constants.USER_FIRSTNAME_MAX_LENGTH), MinLength(Constants.USER_FIRSTNAME_MIN_LENGTH)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(Constants.USER_LASTNAME_MAX_LENGTH), MinLength(Constants.USER_LASTNAME_MIN_LENGTH)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string ImagePath { get; set; }

        [Required]
        [MinLength(Constants.USER_PASSWORD_MIN_LENGTH)]
        public string Password { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

    }
}
