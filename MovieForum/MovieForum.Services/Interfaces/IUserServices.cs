using MovieForum.Data.Models;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieForum.Services.Interfaces
{
    public interface IUserServices : ICRUDOperations<UserDTO>
    {
        Task BlockUser(int id);
        Task<IEnumerable<CommentDTO>> GetAllCommentsAsync(int userId);
        Task<IEnumerable<CommentDTO>> GetAllCommentsAsync(string username);
        Task<User> GetUserAsync(int id);
        Task<User> GetUserAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
        Task<UserDTO> GetUserByUsernameAsync(string username);
        Task<UserDTO> GetUserByIdAsync(int id);
        Task<bool> IsExistingAsync(string email);
        Task UnblockUser(int id);
        int UserCount();
    }
}