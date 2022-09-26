﻿using MovieForum.Enums;
using MovieForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieForum.Repositories.Interfaces
{
    public interface IMoviesRepository
    {
        public List<Movie> GetAll();
        public List<Movie> FilterByGenres(Genres genre);
        public List<Movie> GetByName(string name);
        public Movie GetById(int id);
        public Movie Create(IMovie movie);
        public Movie Update(int id, IMovie movie);
        public Movie Delete(IMovie movie);
    }
}
