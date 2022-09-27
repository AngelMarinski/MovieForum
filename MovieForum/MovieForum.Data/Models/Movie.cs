using MovieForum.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieForum.Models
{
    public class Movie : IMovie
    {
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime ReleaseDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Genres Genre { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<string> Actors { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Rating { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Dislike()
        {
            throw new NotImplementedException();
        }

        public void Like()
        {
            throw new NotImplementedException();
        }
    }
}
