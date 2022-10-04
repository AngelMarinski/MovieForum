using System.ComponentModel.DataAnnotations;

namespace MovieForum.Web.Models
{
    public class CreateCommentViewModel
    {
       
        [Required,StringLength(2000, MinimumLength = 10, ErrorMessage = "Comment content length must be between {1} and {2} characters!")]
        public string Content { get; set; }
        [Required]
        public  int? AuthorId { get; set; }
        [Required]
        public int? MovieId { get; set; }
    }
}
