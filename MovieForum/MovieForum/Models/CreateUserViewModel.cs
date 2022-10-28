using MovieForum.Services.Helpers;
using System.ComponentModel.DataAnnotations;

namespace MovieForum.Web.Models
{
    public class CreateUserViewModel
    {
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

        [Required]
        [MinLength(Constants.USER_PASSWORD_MIN_LENGTH)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Confirm password doesn't match! Type again !")]
        public string ConfirmPassword { get; set; }

    }
}
