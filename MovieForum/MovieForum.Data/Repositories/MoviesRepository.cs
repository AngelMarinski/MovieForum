using MovieForum.Enums;
using MovieForum.Models;
using MovieForum.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MovieForum.Repositories
{
    public class MoviesRepository : IMoviesRepository
    {
        private readonly List<IMovie> movies;

        public MoviesRepository()
        {
            this.movies = new List<IMovie>();
        }

        public IMovie Create(IMovie movie)
        {
            movies.Add(movie);

            return movie;
        }

        public IMovie Delete(int id)
        {
            var movie = movies.FirstOrDefault(m => m.Id == id);

            if(movie == null)
            {
                throw new InvalidOperationException();
            }

            movies.Remove(movie);

            return movie;
        }

        public List<IMovie> GetAll()
        {
            return movies;
        }

        public List<IMovie> FilterByGenres(Genres genre)
        {
            var moviesByGenre = movies.Where(movie => movie.Genre == genre).ToList();

            return moviesByGenre;
        }

        public Movie GetById(int id)
        {
            var movie = movies.FirstOrDefault(m => m.Id == id) as Movie;

            return movie ?? throw new InvalidOperationException();
        }

        public List<IMovie> GetByName(string name)
        {
            var regex = new Regex(@"^.*" + name + ".*$");
            var filtered = movies.Where(movie => regex.IsMatch(movie.Title)).ToList();

            return filtered;
        }

        public IMovie Update(int id, IMovie movie)
        {
            var movieToUpdate = movies.FirstOrDefault(m => m.Id == id);

            movieToUpdate.Title = movie.Title;
            movieToUpdate.Rating = movie.Rating;
            movieToUpdate.Genre = movie.Genre;
            movieToUpdate.Actors = movie.Actors;
            movieToUpdate.LikesCount = movie.LikesCount;
            movieToUpdate.DislikesCount = movie.DislikesCount;

            return movieToUpdate;
        }
    }
}
