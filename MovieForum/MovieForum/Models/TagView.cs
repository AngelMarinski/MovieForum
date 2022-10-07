using System.ComponentModel.DataAnnotations;

namespace MovieForum.Web.Models
{
    public class TagView
    {
        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Tag name length must be between {0} and {1} characters!")]
        public string TagName { get; set; }
    }
}
