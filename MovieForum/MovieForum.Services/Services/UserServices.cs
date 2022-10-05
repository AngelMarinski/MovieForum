using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieForum.Data;
using MovieForum.Data.Models;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Helpers;
using MovieForum.Services.Interfaces;
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

        public UserServices(MovieForumContext context, IMapper mapper)
        {
            this.db = context;
            this.mapper = mapper;
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

        public int UserCount()
        {
            var numOfUsers = db.Users.Count();
            return numOfUsers;
        }

        public async Task<bool> IsExistingAsync(string email)
        {
            return await db.Users.AnyAsync(x => x.Email == email && x.IsDeleted == false);
        }

        public async Task<UserDTO> GetUserByUsernameAsync(string username)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Username == username && x.IsDeleted == false);

            return mapper.Map<UserDTO>(user) ?? throw new Exception(Constants.USER_NOT_FOUND);
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
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

        public async Task<UserDTO> PostAsync(UserDTO obj)
        {
            var isEmailValid = Regex.IsMatch(obj.Email, @"[^@\t\r\n]+@[^@\t\r\n]+\.[^@\t\r\n]+");

            if (await IsExistingAsync(obj.Email))
            {
                throw new Exception("Email already taken");
            }

            if (await IsExistingAsync(obj.Username))
            {
                throw new Exception("Username already taken");
            }

            if (obj == null ||
                (obj.FirstName.Length < 4 || obj.FirstName.Length > 32) ||
                (obj.LastName.Length < 4 || obj.LastName.Length > 32) ||
                obj.Password.Length < 8 ||
                obj.Username.Length < 4 ||
                !isEmailValid)
            {
                throw new Exception("Data invalid");
            }

            var user = mapper.Map<User>(obj);
            user.RoleId = 2;
            user.Role = null;

            var passHasher = new PasswordHasher<User>();
            user.Password = passHasher.HashPassword(user, obj.Password);

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            return obj;
        }

        public async Task<UserDTO> UpdateAsync(int id, UserDTO obj)
        {
            var userToUpdate = await GetUserAsync(id);

            var isEmailValid = Regex.IsMatch(obj.Email, @"[^@\t\r\n]+@[^@\t\r\n]+\.[^@\t\r\n]+");

            if (obj == null ||
                (obj.FirstName.Length <= 4 || obj.FirstName.Length >= 32) ||
                (obj.LastName.Length <= 4 || obj.LastName.Length >= 32) ||
                obj.Password.Length <= 8 ||
                !isEmailValid)
            {
                throw new Exception("Data invalid");
            }

            if (await IsExistingAsync(obj.Email))
            {
                throw new Exception("Email already taken");
            }

            if(obj.Password != userToUpdate.Password)
            {
                var passHasher = new PasswordHasher<User>();
                userToUpdate.Password = passHasher.HashPassword(userToUpdate, obj.Password);
            }

            userToUpdate.FirstName = obj.FirstName;
            userToUpdate.LastName = obj.LastName;
            userToUpdate.Email = obj.Email;
            userToUpdate.ImagePath = obj.ImagePath;
           
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

        public async Task<UserDTO> DeleteAsync(int id)
        {
            var userToDelete = await GetUserAsync(id);

            userToDelete.DeletedOn = DateTime.Now;

            db.Users.Remove(userToDelete);
            await db.SaveChangesAsync();

            return mapper.Map<UserDTO>(userToDelete);
        }

    }
}
