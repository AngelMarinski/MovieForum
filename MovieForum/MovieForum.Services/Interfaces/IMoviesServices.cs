using MovieForum.Data.Models;
using MovieForum.Models;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieForum.Services.Interfaces
{
    public interface IMoviesServices : ICRUDOperations<MovieDTO>
    {
        Task<MovieDTO> GetByIdAsync(int id);
        Task<IEnumerable<MovieDTO>> FilterByAsync(MovieQueryParameters parameters);
        Task<MovieDTO> AddTagAsync(int movieId, string tagName);
        Task<MovieDTO> RemoveTagAsync(int movieId, string tagName);
        Task<IEnumerable<MovieDTO>> GetTopCommentedAsync();
        Task<IEnumerable<MovieDTO>> GetMostRecentPostsAsync();
        Task<MovieDTO> RateMovieAsync(int id, int userId, int rate);
        IEnumerable<CommentDTO> GetMovieComments(int movieId);

        Task<int> GetPostsCount();
    }
}
