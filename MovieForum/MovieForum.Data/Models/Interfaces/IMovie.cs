using MovieForum.Data.Models.Interfaces;
using MovieForum.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieForum.Models
{
    public interface IMovie : IHasId
    {
        public string Title { get; set; }
        public int AuthorID { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime Posted { get; set; }
        public Genres Genre { get; set; }
        public List<string> Actors { get; set; }
        public int Rating { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }
    }
}
