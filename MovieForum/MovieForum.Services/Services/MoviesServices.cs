using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieForum.Data;
using MovieForum.Data.Models;
using MovieForum.Models;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Helpers;
using MovieForum.Services.Interfaces;
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
        private readonly IMapper mapper;

        public MoviesServices(MovieForumContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<MovieDTO>> GetAsync()
        {
            var movies = await db.Movies.Where(x => x.IsDeleted == false).ToListAsync();

            return mapper.Map<IEnumerable<MovieDTO>>(movies);
        }

        public async Task<MovieDTO> GetByIdAsync(int id)
        {
            var movie = await db.Movies.FirstOrDefaultAsync(m => m.Id == id && m.IsDeleted == false)
                ?? throw new InvalidOperationException(Constants.MOVIE_NOT_FOUND);

            return mapper.Map<MovieDTO>(movie);
        }

        public async Task<MovieDTO> PostAsync(MovieDTO obj)
        {
            //TODO: if user is authorized
            if (obj.Title.Length < Constants.MOVIE_TITLE_MIN_LENGHT
                || obj.Title.Length > Constants.MOVIE_TITLE_MAX_LENGHT
                || obj.Content.Length < Constants.MOVIE_CONTENT_MIN_LENGHT
                || obj.Content.Length > Constants.MOVIE_CONTENT_MAX_LENGHT)
            {
                throw new Exception(Constants.INVALID_DATA);
            }

            var movie = mapper.Map<Movie>(obj);

            await db.Movies.AddAsync(movie);
            await db.SaveChangesAsync();

            return obj;
        }

        public async Task<MovieDTO> UpdateAsync(int id, MovieDTO obj)
        {
            var movie = await this.GetByIdAsync(id);

            //TODO: if user is authorized

            if (obj.Title.Length < Constants.MOVIE_TITLE_MIN_LENGHT
                || obj.Title.Length > Constants.MOVIE_TITLE_MAX_LENGHT
                || obj.Content.Length < Constants.MOVIE_CONTENT_MIN_LENGHT
                || obj.Content.Length > Constants.MOVIE_CONTENT_MAX_LENGHT)
            {
                throw new Exception(Constants.INVALID_DATA);
            }

            movie.AuthorId = obj.AuthorId;
            movie.Cast = obj.Cast;
            movie.Content = obj.Content;
            movie.Genre = obj.Genre;
            movie.Rating = obj.Rating;
            movie.ReleaseDate = obj.ReleaseDate;
            movie.Tags = obj.Tags;
            movie.Title = obj.Title;
            movie.Comments = obj.Comments;

            await db.SaveChangesAsync();

            return movie;
        }

        public async Task<MovieDTO> DeleteAsync(int id)
        {
            var movie = mapper.Map<Movie>(await this.GetByIdAsync(id));

            movie.DeletedOn = DateTime.Now;
            movie.IsDeleted = true;

            db.Movies.Remove(movie);

            await db.SaveChangesAsync();

            return mapper.Map<MovieDTO>(movie);
        }

        public async Task<IEnumerable<MovieDTO>> FilterByAsync(MovieQueryParameters parameters)
        {
            List<MovieDTO> result = new List<MovieDTO>(await db.Movies
                                                        .Select(x => mapper.Map<MovieDTO>(x))
                                                        .ToListAsync());

            if (!string.IsNullOrEmpty(parameters.Name))
            {
                result = result.FindAll(x => x.Title.Contains(parameters.Name)).ToList();
            }

            if (parameters.MinRating.HasValue)
            {
                result = result.FindAll(x => x.Rating >= parameters.MinRating);
            }

            /*            if (!string.IsNullOrEmpty(parameters.Username))
                        {
                            result = result.FindAll(x => x.Author.Username.Contains(parameters.Username)).ToList();
                        }*/

            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                if (parameters.SortBy.Equals("title", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderBy(x => x.Title).ToList();
                }
                else if (parameters.SortBy.Equals("releasedate", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderBy(x => x.ReleaseDate).ToList();
                }
                else if (parameters.SortBy.Equals("rating", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderByDescending(x => x.Rating).ToList();
                }
                else if (parameters.SortBy.Equals("comments", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderByDescending(x => x.Comments.Count).ToList();
                }

                if (!string.IsNullOrEmpty(parameters.SortOrder)
                    && parameters.SortOrder.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                {
                    result.Reverse();
                }
            }

            return result;
        }
    }
}
