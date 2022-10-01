using MovieForum.Data.Models;
using MovieForum.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieForum.Services.DTOModels
{
    public class CommentDTO
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public int? AuthorId { get; set; }

        //public User Author { get; set; }
        public string AuthorUsername { get; set; }
        
        public int? MovieId { get; set; }

        public int LikesCount { get; set; }
        
        public int DisLikesCount { get; set; }

        public string PostedOn { get; set; }
    }
}

