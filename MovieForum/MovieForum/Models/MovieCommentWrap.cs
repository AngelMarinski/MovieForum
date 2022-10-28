using MovieForum.Services.DTOModels;

namespace MovieForum.Web.Models
{
    public class MovieCommentWrap
    {
        public MovieDTO MovieDTO { get;set;}

        public CreateCommentViewModel commentViewModel { get; set; }

    }
}
