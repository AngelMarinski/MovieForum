using Microsoft.EntityFrameworkCore;
using MovieForum.Data;
using MovieForum.Data.Models;
using MovieForum.Models;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Helpers;
using MovieForum.Services.Interfaces;
using MovieForum.Services.Mappers;
using MovieForum.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieForum.Services
{
    public class MoviesServices : IMoviesServices
    {
        private readonly MovieForumContext db;

        public MoviesServices(MovieForumContext db)
        {
            this.db = db;
        }

        public async Task<MovieDTO> DeleteAsync(int id)
        {
            var movie = await this.db.Movies
                    .Include(m => m.Author)
                        .ThenInclude(m => m.Role)
                    .Include(m => m.Comments)
                    .Include(m => m.Genre)
                    .Include(m => m.Tags)
                        .ThenInclude(t => t.Tag)
                    .Include(m => m.Cast)
                        .ThenInclude(a => a.Actor)
                     .FirstOrDefaultAsync(x => x.Id == id)
                     ?? throw new InvalidOperationException(Constants.MOVIE_NOT_FOUND);

            var movieDTO = MovieMapper.GetDTO(movie);

            movie.DeletedOn = DateTime.Now;
            movie.IsDeleted = true;
            await db.SaveChangesAsync();

            return movieDTO;
        }

        public Task<IEnumerable<MovieDTO>> FilterByAsync(MovieQueryParameters parameters)
        {
            throw new NotImplementedException();
        }

        
        public List<Movie> FilterByGenres(Genre genre)
        {
            throw new NotImplementedException();
        }

        public List<Movie> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MovieDTO>> GetAsync()
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

        public Task<MovieDTO> PostAsync(MovieDTO obj)
        {
            throw new NotImplementedException();
        }

        public Movie Update(int id, Movie movie)
        {
            throw new NotImplementedException();
        }

        public Task<MovieDTO> UpdateAsync(int id, MovieDTO obj)
        {
            throw new NotImplementedException();
        }

        Task<DTOModels.MovieDTO> ICRUDOperations<DTOModels.MovieDTO>.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<DTOModels.MovieDTO>> ICRUDOperations<DTOModels.MovieDTO>.GetAsync()
        {
            throw new NotImplementedException();
        }
    }
}
