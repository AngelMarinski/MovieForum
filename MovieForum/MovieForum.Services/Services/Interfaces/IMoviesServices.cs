using MovieForum.Data.Models;
using MovieForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieForum.Services.Interfaces
{
    public interface IMoviesServices
    {
        public List<Movie> GetAll();
        public List<Movie> FilterByGenres(Genre genre);
        public List<Movie> GetByName(string name);
        public Movie GetById(int id);
        public Movie Create(Movie movie);
        public void Delete(Movie movie);
        public Movie Update(int id, Movie movie);
    }
}
