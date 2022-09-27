using MovieForum.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieForum.Data.Models
{
    public class Comment : IComment
    {
        public Comment(string title, int id, string content, int authorId, int movieId)
        {
            this.Title = title;
            this.Id = id;
            this.Content = content;
            this.AuthorID = authorId;
            this.MovieId = movieId;
        }
        public int Id { get; set; }

        [StringLength(64, MinimumLength = 5, ErrorMessage = "Comment title length must be between {0} and {1} characters!")]
        public string Title { get; set; }

        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Comment content length must be between {0} and {1} characters!")]
        public string Content { get; set; }
        public int AuthorID { get; set; }
        public int MovieId { get; set; }
        public int LikesCount { get; set; }
        public int DisLikesCount { get; set; }
    }
}
