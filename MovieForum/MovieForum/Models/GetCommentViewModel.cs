using System;

namespace MovieForum.Web.Models
{
    public class GetCommentViewModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public int LikesCount { get; set; }

        public int DisLikesCount { get; set; }

        public DateTime? PostedOn { get; set; }
    }
}
