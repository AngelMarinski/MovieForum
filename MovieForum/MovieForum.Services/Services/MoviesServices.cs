using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieForum.Data;
using MovieForum.Data.Models;
using MovieForum.Models;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Helpers;
using MovieForum.Services.Interfaces;
using MovieForum.Services.Models;
using MovieForum.Web.Models;
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

        public async Task<IQueryable<MovieDTO>> GetQueryableAsync()
        {
            var movies = await db.Movies.Where(x => x.IsDeleted == false).ToListAsync();

            var moviesDTO = this.mapper.Map<IEnumerable<MovieDTO>>(movies);

            return moviesDTO.AsQueryable();
        }


        public async Task<MovieDTO> GetByIdAsync(int id)
        {
            var movie = await db.Movies.FirstOrDefaultAsync(m => m.Id == id && m.IsDeleted == false)
                ?? throw new InvalidOperationException(Constants.MOVIE_NOT_FOUND);

            return mapper.Map<MovieDTO>(movie);
        }

        public async Task<IEnumerable<MovieDTO>> GetTopCommentedAsync()
        {
            var movies = await db.Movies.OrderByDescending(x => x.Comments.Count).Where(x=>x.IsDeleted==false)
                                        .Take(10).ToListAsync();

            return mapper.Map<IEnumerable<MovieDTO>>(movies);
        }

        public async Task<IEnumerable<MovieDTO>> GetMostRecentPostsAsync()
        {
            var movies = await db.Movies.OrderByDescending(x => x.Posted).Where(x => x.IsDeleted == false)
                                        .Take(10).ToListAsync();

            return mapper.Map<IEnumerable<MovieDTO>>(movies);
        }

        public async Task<MovieDTO> PostAsync(MovieDTO obj)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Username == obj.Username)
                ?? throw new InvalidOperationException(Constants.USER_NOT_FOUND);

            var genre = await db.Genres.FirstOrDefaultAsync(x => x.Id == obj.GenreId)
                 ?? throw new InvalidOperationException(Constants.GENRE_NOT_FOUND);

            if (obj.Title.Length < Constants.MOVIE_TITLE_MIN_LENGHT
                || obj.Title.Length > Constants.MOVIE_TITLE_MAX_LENGHT
                || obj.Content.Length < Constants.MOVIE_CONTENT_MIN_LENGHT
                || obj.Content.Length > Constants.MOVIE_CONTENT_MAX_LENGHT)
            {
                throw new Exception(Constants.INVALID_DATA);
            }

            var movie = new Movie
            {
                AuthorID = user.Id,
                Author = user,
                Title = obj.Title,
                Content = obj.Content,
                ReleaseDate = (DateTime)obj.ReleaseDate,
                Posted = DateTime.Now,
                Genre = genre,
                GenreId = (int)genre.Id,
                IsDeleted = false,
            };

            var tags = mapper.Map<ICollection<MovieTags>>(obj.Tags);
            var cast = mapper.Map<ICollection<MovieActor>>(obj.Cast);

            if (obj.ImagePath != null)
            {
                movie.ImagePath = obj.ImagePath;
            }

            movie.Tags = tags;
            movie.Cast = cast;

            await db.Movies.AddAsync(movie);
            await db.SaveChangesAsync();
            var movieDTO = mapper.Map<MovieDTO>(movie);

            return movieDTO;
        }

        public async Task<MovieDTO> UpdateAsync(int id, MovieDTO obj)
        {
            var movie = await db.Movies.FirstOrDefaultAsync(m => m.Id == id && m.IsDeleted == false)
                        ?? throw new InvalidOperationException(Constants.MOVIE_NOT_FOUND);

/*            if (obj.Cast != null)
            {
                movie.Cast = new List<MovieActor>(mapper.Map<ICollection<MovieActor>>(obj.Cast));
            }*/
            if (obj.Title != null)
            {
                movie.Title = obj.Title;
            }
            if (obj.Content != null)
            {
                movie.Content = obj.Content;
            }
            if (obj.GenreId != movie.GenreId && obj.GenreId != default(int))
            {
                var genre = await db.Genres.FirstOrDefaultAsync(x => x.Id == obj.GenreId)
                     ?? throw new InvalidOperationException(Constants.GENRE_NOT_FOUND);
                movie.Genre = genre;
            }
            /*            if(obj.Tags != null)
                        {
                            movie.Tags = new List<MovieTags>(mapper.Map<ICollection<MovieTags>>(obj.Tags));
                        }*/

            if (obj.ReleaseDate != null)
            {
                movie.ReleaseDate = (DateTime)obj.ReleaseDate;
            }
            if (obj.ImagePath != null)
            {
                movie.ImagePath = obj.ImagePath;
            }


            await db.SaveChangesAsync();

            return mapper.Map<MovieDTO>(movie);
        }

        public async Task<MovieDTO> DeleteAsync(int id)
        {
            var movie = await db.Movies.FirstOrDefaultAsync(m => m.Id == id && m.IsDeleted == false)
                ?? throw new InvalidOperationException(Constants.MOVIE_NOT_FOUND);

            movie.DeletedOn = DateTime.Now;
            movie.IsDeleted = true;

            foreach (var comment in movie.Comments)
            {
                comment.IsDeleted = true;
                comment.DeletedOn = DateTime.Now;
            }

            db.Movies.Remove(movie);

            await db.SaveChangesAsync();

            return mapper.Map<MovieDTO>(movie);
        }

        public async Task<PaginatedList<MovieDTO>> FilterByAsync(MovieQueryParameters parameters)
        {
            var result = await this.GetQueryableAsync();

            if (!string.IsNullOrEmpty(parameters.Title))
            {

                result = result.Where(x => x.Title.Contains(parameters.Title, StringComparison.InvariantCultureIgnoreCase));
            }

            if (parameters.MinRating.HasValue && parameters.MinRating != 0)
            {
                result = result.Where(x => !Double.IsNaN(x.Rating) && x.Rating >= parameters.MinRating);
            }

            if (!string.IsNullOrEmpty(parameters.Username))
            {
                result = result.Where(x => !string.IsNullOrEmpty(x.Username)
                        && x.Username.Contains(parameters.Username));
            }

            if (!string.IsNullOrEmpty(parameters.Genre))
            {
                result = result.Where(x => x.Genre.Name == parameters.Genre);
            }

            if (!string.IsNullOrEmpty(parameters.Username))
            {
                result = result.Where(x => x.Username.Contains(parameters.Username));
            }

            if (!string.IsNullOrEmpty(parameters.Tag))
            {
                var list = result;
                result = Enumerable.Empty<MovieDTO>().AsQueryable();
                result.Concat(from item in list
                              from tags in item.Tags
                              where tags.TagName.ToLower().Contains(parameters.Tag.ToLower())
                              select item);
            }

            if (!string.IsNullOrEmpty(parameters.Year))
            {
                result = result.Where(t => t.ReleaseDate.ToString().Contains(parameters.Year));
            }

            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                if (parameters.SortBy.Equals("title", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderBy(x => x.Title);
                }
                else if (parameters.SortBy.Equals("releasedate", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderBy(x => x.ReleaseDate);
                }
                else if (parameters.SortBy.Equals("rating", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderByDescending(x => x.Rating);
                }
                else if (parameters.SortBy.Equals("comments", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderByDescending(x => x.Comments.Count);
                }

                if (!string.IsNullOrEmpty(parameters.SortOrder)
                    && parameters.SortOrder.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                {
                    result.Reverse();
                }
            }
            int totalPages = (result.Count() + 1) / parameters.PageSize;

            result = Paginate(result, parameters.PageNumber, parameters.PageSize);

            return new PaginatedList<MovieDTO>(result.ToList(), totalPages, parameters.PageNumber);
        }

        private IQueryable<MovieDTO> Paginate(IQueryable<MovieDTO> movies, int pageNumber, int pageSize)
        {
            return movies.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public async Task<MovieDTO> AddTagAsync(int movieId, string tagName)
        {
            var movie = await db.Movies.FirstOrDefaultAsync(m => m.Id == movieId && m.IsDeleted == false)
                        ?? throw new InvalidOperationException(Constants.MOVIE_NOT_FOUND);

            var tag = await db.Tags.FirstOrDefaultAsync(t => t.TagName == tagName);

            if (tag == null)
            {
                tag = new Tag
                {
                    TagName = tagName,
                    IsDeleted = false
                };
            }
            else if (tag.IsDeleted)
            {
                tag.IsDeleted = false;
            }

            var movieTag = new MovieTags
            {
                MovieId = movie.Id,
                Movie = movie,
                Tag = tag,
                TagId = tag.Id,
                IsDeleted = false
            };

            movie.Tags.Add(movieTag);
            //tag.Movies.Add(movieTag);

            await db.SaveChangesAsync();

            return mapper.Map<MovieDTO>(movie);
        }

        public async Task<MovieDTO> AddActorAsync(int movieID, string firstName, string lastName)
        {
            var movie = await db.Movies.FirstOrDefaultAsync(m => m.Id == movieID && m.IsDeleted == false)
                        ?? throw new InvalidOperationException(Constants.MOVIE_NOT_FOUND);

            var actor = await this.db.Actors.FirstOrDefaultAsync(x => x.FirstName == firstName && x.LastName == lastName);

            if (actor == null)
            {
                actor = new Actor
                {
                    FirstName = firstName,
                    LastName = lastName,
                    IsDeleted = false
                };
            }
            else if (actor.IsDeleted)
            {
                actor.IsDeleted = false;
            }

            if (movie.Cast.Any(x => x.ActorId == actor.Id))
            {
                movie.Cast.FirstOrDefault(x => x.ActorId == actor.Id).IsDeleted = false;
            }
            else
            {
                var movieActor = new MovieActor
                {
                    MovieId = movie.Id,
                    Actor = actor,
                    ActorId = actor.Id,
                    IsDeleted = false
                };


                movie.Cast.Add(movieActor);
            }

            await db.SaveChangesAsync();

            return mapper.Map<MovieDTO>(movie);
        }

        public async Task<MovieDTO> RemoveActorAsync(int movieId, string firstName, string lastName)
        {
            var movie = await db.Movies.FirstOrDefaultAsync(m => m.Id == movieId && m.IsDeleted == false)
                                   ?? throw new InvalidOperationException(Constants.MOVIE_NOT_FOUND);

            var actor = await db.Actors.FirstOrDefaultAsync(t => t.FirstName == firstName
                                                            && t.LastName == lastName)
                                    ?? throw new InvalidOperationException(Constants.ACTOR_NOT_FOUND); ;

            var movieActor = await db.MovieActors
                            .Where(m => m.MovieId == movie.Id && m.ActorId == actor.Id)
                            .ToListAsync();

            foreach (var item in movieActor)
            {
                item.IsDeleted = true;
                item.DeletedOn = DateTime.Now;
            }

            await db.SaveChangesAsync();

            return mapper.Map<MovieDTO>(movie);
        }

        public async Task<MovieDTO> RemoveTagAsync(int movieId, string tagName)
        {
            var movie = await db.Movies.FirstOrDefaultAsync(m => m.Id == movieId && m.IsDeleted == false)
                        ?? throw new InvalidOperationException(Constants.MOVIE_NOT_FOUND);

            var tag = await db.Tags.FirstOrDefaultAsync(t => t.TagName == tagName);

            var movieTags = await db.MoviesTags
                            .Where(m => m.MovieId == movie.Id && m.TagId == tag.Id)
                            .ToListAsync();

            foreach (var item in movieTags)
            {
                item.IsDeleted = true;
                item.DeletedOn = DateTime.Now;
            }

            await db.SaveChangesAsync();

            return mapper.Map<MovieDTO>(movie);
        }

        public async Task<MovieDTO> RateMovieAsync(int id, int userId, int rate)
        {
            var movie = await this.db.Movies.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false)
                                            ?? throw new InvalidOperationException(Constants.MOVIE_NOT_FOUND);

            var user = await this.db.Users.FirstOrDefaultAsync(x => x.Id == userId)
                             ?? throw new InvalidOperationException(Constants.USER_NOT_FOUND);

            if (movie.Rating.Any(x => x.UserID == user.Id))
            {
                throw new UnauthorizedAccessException(Constants.ALREADY_RATED);
            }

            movie.Rating.Add(new Rating
            {
                UserID = user.Id,
                MovieId = movie.Id,
                Rate = rate,
                IsDeleted = false
            });

            await db.SaveChangesAsync();

            return mapper.Map<MovieDTO>(movie);
        }

        public IEnumerable<CommentDTO> GetMovieComments(int movieId)
        {
            var comments = this.db.Comments.Where(x => x.MovieId == movieId && x.IsDeleted == false);

            return mapper.Map<IEnumerable<CommentDTO>>(comments);
        }

        public async Task<int> GetPostsCount()
        {
            var movies = await GetAsync();

            return movies.Count();
        }

    }
}
