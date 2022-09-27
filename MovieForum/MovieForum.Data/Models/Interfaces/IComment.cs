using System;
using System.Collections.Generic;
using System.Text;

namespace MovieForum.Data.Models.Interfaces
{
    public interface IComment : IHasId
    {
        public string Title { get; set; }

        //Content
        //AuthorId
        //MovieId
        //LikesCount
        //DislikeCount


    }
}
