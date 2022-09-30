using System;
using System.Collections.Generic;
using System.Text;

namespace MovieForum.Services.DTOModels
{
    public class CommentDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int? AuthorId { get; set; }

        public string AuthorUsername { get; set; }

        public int? MovieId { get; set; }

        public DateTime? PostedOn { get; set; }
    }
}

