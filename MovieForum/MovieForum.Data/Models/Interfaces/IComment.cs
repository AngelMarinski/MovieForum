using System;
using System.Collections.Generic;
using System.Text;

namespace MovieForum.Data.Models.Interfaces
{
    public interface IComment : IHasId
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public int AuthorID { get; set; }
    
        public int MovieId { get; set; } 

        public int LikesCount { get; set; }

        public int DisLikesCount { get; set; }



    }
}
