using MovieForum.Data.Models;
using MovieForum.Services.DTOModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieForum.Services.Interfaces
{
    public interface IUserServices : ICRUDOperations<UserDTO>
    {
        Task<IEnumerable<Comment>> GetAllCommentsAsync(int userId);
        Task<IEnumerable<Comment>> GetAllCommentsAsync(string username);
        Task<User> GetUserAsync(int id);
        Task<User> GetUserAsync(string username);
        Task<UserDTO> GetUsernameAsync(string username);
        Task<bool> IsExistingAsync(string email);
        int UserCount();
    }
}