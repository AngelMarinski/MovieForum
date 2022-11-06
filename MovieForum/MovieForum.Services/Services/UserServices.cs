using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MovieForum.Data;
using MovieForum.Data.Models;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Helpers;
using MovieForum.Services.Interfaces;
using MovieForum.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MovieForum.Services.Services
{
    public class UserServices : IUserServices
    {
        private readonly MovieForumContext db;
        private readonly IMapper mapper;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;

        public UserServices(IEmailService emailService, IConfiguration configuration, MovieForumContext context, IMapper mapper)
        {
            this.emailService = emailService;
            this.configuration = configuration;
            this.db = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await db.Users.Where(x => x.IsDeleted == false).ToListAsync();
            return mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<User> GetUserAsync(int id)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            return user ?? throw new InvalidOperationException(Constants.USER_NOT_FOUND);
        }

        public async Task<User> GetUserAsync(string username)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Username == username && x.IsDeleted == false);
            return user ?? throw new InvalidOperationException(Constants.USER_NOT_FOUND);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email == email && x.IsDeleted == false);

            return user ?? throw new InvalidOperationException(Constants.USER_NOT_FOUND);
        }

        public async Task<IEnumerable<UserDTO>> Search(string userSearch, int type)
        {
            IEnumerable<UserDTO> userQuery = null;

            if (String.IsNullOrEmpty(userSearch))
            {
                return userQuery = await db.Users.Where(x => x.IsDeleted == false).Select(x => mapper.Map<UserDTO>(x)).ToListAsync();
            }

            switch (type)
            {
                case 1:
                    userQuery = await db.Users.Where(x => (x.Username.Contains(userSearch) && x.IsDeleted == false))
                        .Select(x => mapper.Map<UserDTO>(x)).ToListAsync();
                    break;
                case 2:
                    userQuery = await db.Users.Where(x => (x.Email.Contains(userSearch) && x.IsDeleted == false))
                        .Select(x => mapper.Map<UserDTO>(x)).ToListAsync();
                    break;
                case 3:
                    userQuery = await db.Users.Where(x => (x.FirstName.Contains(userSearch) && x.IsDeleted == false))
                        .Select(x => mapper.Map<UserDTO>(x)).ToListAsync();
                    break;
            }
            return userQuery;
        }

        public async Task<int> UserCount()
        {
            var numOfUsers = await GetAllUsersAsync();
            return numOfUsers.Count();
        }

        public async Task<bool> IsExistingAsync(string email)
        {
            return await db.Users.AnyAsync(x => x.Email == email && x.IsDeleted == false);
        }

        public async Task<bool> IsExistingUsernameAsync(string username)
        {
            return await db.Users.AnyAsync(x => x.Username == username && x.IsDeleted == false);
        }

        public async Task<UserDTO> GetUserDTOByUsernameAsync(string username)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Username == username && x.IsDeleted == false);

            return mapper.Map<UserDTO>(user) ?? throw new Exception(Constants.USER_NOT_FOUND);
        }

        public async Task<UserDTO> GetUserDTOByEmailAsync(string email)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email == email && x.IsDeleted == false);

            return mapper.Map<UserDTO>(user) ?? throw new Exception(Constants.USER_NOT_FOUND);
        }

        public async Task<UserDTO> GetUserDTOByIdAsync(int id)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);

            return mapper.Map<UserDTO>(user) ?? throw new Exception(Constants.USER_NOT_FOUND);
        }

        public async Task<IEnumerable<CommentDTO>> GetAllCommentsAsync(int userId)
        {
            var user = await GetUserAsync(userId);
            var comments = user.Comments.Select(x => mapper.Map<CommentDTO>(x)).ToList();

            return comments ?? throw new Exception(Constants.USER_NOT_FOUND);
        }

        public async Task<IEnumerable<CommentDTO>> GetAllCommentsAsync(string username)
        {
            var user = await GetUserAsync(username);
            var comments = user.Comments.Select(x => mapper.Map<CommentDTO>(x)).ToList();

            return comments ?? throw new Exception(Constants.USER_NOT_FOUND);
        }

        public async Task<IEnumerable<MovieDTO>> GetAllMoviesAsync(int userId)
        {
            var user = await GetUserAsync(userId);
            var movies = user.Movies.Select(x => mapper.Map<MovieDTO>(x)).ToList();

            return movies ?? throw new Exception(Constants.USER_NOT_FOUND);
        }

        public async Task<IEnumerable<MovieDTO>> GetAllMoviesAsync(string username)
        {
            var user = await GetUserAsync(username);
            var movies = user.Movies.Select(x => mapper.Map<MovieDTO>(x)).ToList();

            return movies ?? throw new Exception(Constants.USER_NOT_FOUND);
        }

        public async Task<IEnumerable<UserDTO>> GetAsync()
        {
            return await db.Users.Select(x => mapper.Map<UserDTO>(x)).ToListAsync();
        }

        public async Task BlockUser(int id)
        {
            var user = await GetUserAsync(id);

            if (user.IsBlocked == true)
            {
                throw new Exception("User is already blocked");
            }

            user.IsBlocked = true;

            await db.SaveChangesAsync();
        }

        public async Task UnblockUser(int id)
        {
            var user = await GetUserAsync(id);

            if (user.IsBlocked == false)
            {
                throw new Exception("User is not blocked");
            }

            user.IsBlocked = false;

            await db.SaveChangesAsync();
        }

        public async Task<UserDTO> PostAsync(UpdateUserDTO obj)
        {
            var isEmailValid = Regex.IsMatch(obj.Email, @"[^@\t\r\n]+@[^@\t\r\n]+\.[^@\t\r\n]+");

            if (await IsExistingAsync(obj.Email))
            {
                throw new Exception("Email already taken");
            }

            if (await IsExistingUsernameAsync(obj.Username))
            {
                throw new Exception("Username already taken");
            }

            if (obj == null ||
                (obj.FirstName.Length < Constants.USER_FIRSTNAME_MIN_LENGTH || obj.FirstName.Length > Constants.USER_FIRSTNAME_MAX_LENGTH) ||
                (obj.LastName.Length < Constants.USER_LASTNAME_MIN_LENGTH || obj.LastName.Length > Constants.USER_LASTNAME_MAX_LENGTH) ||
                obj.Password.Length < Constants.USER_PASSWORD_MIN_LENGTH ||
                obj.Username.Length < Constants.USER_USERNAME_MIN_LENGTH ||
                !isEmailValid)
            {
                throw new Exception(Constants.INVALID_DATA);
            }

            var user = mapper.Map<User>(obj);
            user.RoleId = 2;
            user.RoleId = 2;
            user.Role = null;

            var passHasher = new PasswordHasher<User>();
            user.Password = passHasher.HashPassword(user, obj.Password);

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            return mapper.Map<UserDTO>(obj);
        }

        public async Task<UserDTO> UpdateAsync(int id, UpdateUserDTO obj)
        {
            var userToUpdate = await GetUserAsync(id);

            var isEmailValid = Regex.IsMatch(obj.Email, @"[^@\t\r\n]+@[^@\t\r\n]+\.[^@\t\r\n]+");

            if (obj.Email != userToUpdate.Email)
            {
                if (await IsExistingAsync(obj.Email))
                {
                    throw new Exception("Email already taken");
                }
            }

            if (obj.Password != userToUpdate.Password && (obj.Password.Length >= Constants.USER_PASSWORD_MIN_LENGTH))
            {
                var passHasher = new PasswordHasher<User>();
                userToUpdate.Password = passHasher.HashPassword(userToUpdate, obj.Password);
            }


            if (obj.FirstName != null && (obj.FirstName.Length >= Constants.USER_FIRSTNAME_MIN_LENGTH && obj.FirstName.Length <= Constants.USER_FIRSTNAME_MAX_LENGTH))
            {
                userToUpdate.FirstName = obj.FirstName;
            }

            if (obj.LastName != null && (obj.LastName.Length >= Constants.USER_LASTNAME_MIN_LENGTH && obj.LastName.Length <= Constants.USER_LASTNAME_MAX_LENGTH))
            {
                userToUpdate.LastName = obj.LastName;
            }

            if (isEmailValid)
            {
                userToUpdate.Email = obj.Email;
            }

            userToUpdate.ImagePath = obj.ImagePath ?? userToUpdate.ImagePath;

            if (obj.PhoneNumber != null)
            {
                var isPhoneValid = Regex.IsMatch(obj.PhoneNumber, @"^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$");
                if (isPhoneValid)
                {
                    userToUpdate.PhoneNumber = obj.PhoneNumber;
                }
            }

            await db.SaveChangesAsync();

            return mapper.Map<UserDTO>(userToUpdate);
        }

        public async Task<UserDTO> DeleteAsync(string email)
        {
            var userToDelete = await GetUserByEmailAsync(email);

            userToDelete.DeletedOn = DateTime.Now;

            db.Users.Remove(userToDelete);
            await db.SaveChangesAsync();

            return mapper.Map<UserDTO>(userToDelete);
        }

        
        public async Task GenerateForgotPasswordTokenAsync(User user)
        {
            var token = Guid.NewGuid().ToString("d").Substring(1, 8);

            if (!string.IsNullOrEmpty(token))
            {
                await SendForgotPasswordEmail(user, token);
            }
        }

        private async Task SendForgotPasswordEmail(User user, string token)
        {
            string appDomain = configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = configuration.GetSection("Application:ForgotPassword").Value;

            UserEmailOptions options = new UserEmailOptions
            
            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", user.FirstName),
                    new KeyValuePair<string, string>("{{Link}}",
                        string.Format(appDomain + confirmationLink, user.Id, token))
                }
            };

            await emailService.SendEmailForForgotPassword(options);
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordModel model)
        {
            var user = await GetUserAsync(int.Parse(model.UserId));
            if (user != null)
            {
                var passHasher = new PasswordHasher<User>();
                user.Password = passHasher.HashPassword(user, model.NewPassword);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task GenerateEmailConfirmationTokenAsync(User user)
        {
            if(user.Id == 0)
            {
                user = await GetUserAsync(user.Username);
            }
            var token = Guid.NewGuid().ToString("d").Substring(1, 8);

            if (!string.IsNullOrEmpty(token))
            {
                await SendEmailConfirmationEmail(user, token);
            }
        }


        private async Task SendEmailConfirmationEmail(User user, string token)
        {
            string appDomain = configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = configuration.GetSection("Application:EmailConfirmation").Value;

            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", user.FirstName),
                    new KeyValuePair<string, string>("{{Link}}",
                        string.Format(appDomain + confirmationLink, user.Id, token))
                }
            };

            await emailService.SendEmailForEmailConfirmation(options);
        }


        public async Task<bool> ConfirmEmailAsync(string uid, string token)
        {            
            var user = await GetUserAsync(int.Parse(uid));
            if (user != null)
            {
                user.IsEmailConfirmed = true;
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
