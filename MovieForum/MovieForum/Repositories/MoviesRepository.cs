using MovieForum.Enums;
using MovieForum.Models;
using MovieForum.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieForum.Repositories
{
    public class MoviesRepository : IMoviesRepository
    {
        private readonly List<Movie> movies;

        public Movie Create(IMovie movie)
        {
            throw new NotImplementedException();
        }

        public Movie Delete(IMovie movie)
        {
            throw new NotImplementedException();
        }

        public List<Movie> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<Movie> FilterByGenres(Genres genre)
        {
            throw new NotImplementedException();
        }

        public Movie GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Movie> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Movie Update(int id, IMovie movie)
        {
            throw new NotImplementedException();
        }
    }
}
