using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieForum.Services.Helpers;
using System.ComponentModel.DataAnnotations;

namespace MovieForum.Web.Models
{
    public class UpdateUserViewModel
    {
        [MaxLength(Constants.USER_FIRSTNAME_MAX_LENGTH), MinLength(Constants.USER_FIRSTNAME_MIN_LENGTH)]
        public string FirstName { get; set; }

        [MaxLength(Constants.USER_LASTNAME_MAX_LENGTH), MinLength(Constants.USER_LASTNAME_MIN_LENGTH)]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [MinLength(Constants.USER_PASSWORD_MIN_LENGTH)]
        public string Password { get; set; }
        public IFormFile File { get; set; }
        public string ImagePath { get; set; }
    }
}
