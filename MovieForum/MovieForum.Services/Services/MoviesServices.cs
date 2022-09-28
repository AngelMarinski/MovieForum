using MovieForum.Enums;
using MovieForum.Models;
using MovieForum.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieForum.Services
{
    public class MoviesServices : IMoviesServices
    {
        public Movie Create(Movie movie)
        {
            throw new NotImplementedException();
        }

        public void Delete(Movie movie)
        {
            throw new NotImplementedException();
        }

        public List<Movie> FilterByGenres(Genres genre)
        {
            throw new NotImplementedException();
        }

        public List<Movie> GetAll()
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

        public Movie Update(int id, Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}
