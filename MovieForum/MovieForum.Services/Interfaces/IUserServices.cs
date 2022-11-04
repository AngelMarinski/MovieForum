using MovieForum.Data.Models;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Interfaces;
using MovieForum.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieForum.Services.Interfaces
{
    public interface IUserServices
    {
        Task<IEnumerable<UserDTO>> GetAsync();
        Task<UserDTO> PostAsync(UpdateUserDTO obj);
        Task<UserDTO> UpdateAsync(int id, UpdateUserDTO obj);
        Task<UserDTO> DeleteAsync(int id);
        Task<IEnumerable<UserDTO>> Search(string userSearch, int type);
        Task<IEnumerable<CommentDTO>> GetAllCommentsAsync(int userId);
        Task<IEnumerable<CommentDTO>> GetAllCommentsAsync(string username);
        Task<IEnumerable<MovieDTO>> GetAllMoviesAsync(int userId);
        Task<IEnumerable<MovieDTO>> GetAllMoviesAsync(string username);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<User> GetUserAsync(int id);
        Task<User> GetUserAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
        Task<UserDTO> GetUserDTOByUsernameAsync(string username);
        Task<UserDTO> GetUserDTOByEmailAsync(string email);
        Task<UserDTO> GetUserDTOByIdAsync(int id);
        Task<bool> IsExistingAsync(string email);
        Task<bool> IsExistingUsernameAsync(string username);

        Task BlockUser(int id);
        Task UnblockUser(int id);
        Task<int> UserCount();

        Task GenerateForgotPasswordTokenAsync(User user);
        Task<bool> ResetPasswordAsync(ResetPasswordModel model);
        Task GenerateEmailConfirmationTokenAsync(User user);
        Task<bool> ConfirmEmailAsync(string uid, string token);
    }
}