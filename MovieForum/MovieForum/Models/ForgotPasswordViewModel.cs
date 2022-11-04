using System.ComponentModel.DataAnnotations;

namespace MovieForum.Web.Models
{
    public class ForgotPasswordViewModel
    {
        [Required, EmailAddress, Display(Name = "Registered email address")]
        public string Email { get; set; }
        public bool EmailSent { get; set; }
    }
}
