using MovieForum.Enums;
using MovieForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieForum.Repositories.Interfaces
{
    public interface IMoviesRepository
    {
        public List<IMovie> GetAll();
        public List<IMovie> FilterByGenres(Genres genre);
        public List<IMovie> GetByName(string name);
        public Movie GetById(int id);
        public IMovie Create(IMovie movie);
        public IMovie Update(int id, IMovie movie);
        public IMovie Delete(int id);
    }
}
