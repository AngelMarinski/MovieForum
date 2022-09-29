using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieForum.Data;
using MovieForum.Data.Models;
using MovieForum.Services.Helpers;
using MovieForum.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public int UserCount()
        {
            var numOfUsers = db.Users.Count();
            return numOfUsers;
        }

        public async Task<bool> IsExistingAsync(string email)
        {
            return await db.Users.AnyAsync(x => x.Email == email && x.IsDeleted == false);
        }

        public async Task<UserDTO> GetUsernameAsync(string username)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Username == username && x.IsDeleted == false);

            return mapper.Map<UserDTO>(user) ?? throw new Exception();
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync(int userId)
        {
            var comments = await db.Users.Where(x => x.Id == userId && x.IsDeleted == false).Select(x => x.Comments).ToListAsync();
            //TODO replace comments with commentDTO
            //TODO remove cast
            return (IEnumerable<Comment>)comments;
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync(string username)
        {
            var comments = await db.Users.Where(x => x.Username == username && x.IsDeleted == false).Select(x => x.Comments).ToListAsync();
            //TODO replace comments with commentDTO
            //TODO remove cast
            return (IEnumerable<Comment>)comments;
        }

        public async Task<IEnumerable<UserDTO>> GetAsync()
        {
            return await db.Users.Select(x => mapper.Map<UserDTO>(x)).ToListAsync();
        }

        public Task<UserDTO> PostAsync(UserDTO obj)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDTO> UpdateAsync(int id, UserDTO obj)
        {
            var userToUpdate = await GetUserAsync(id);

            //TODO Validations

            userToUpdate.FirstName = obj.FirstName;
            userToUpdate.LastName = obj.LastName;
            userToUpdate.Password = obj.Password;
            userToUpdate.Email = obj.Email;
            userToUpdate.ImagePath = obj.ImagePath;

            if (obj.Role == "Admin")
            {
                userToUpdate.PhoneNumber = obj.PhoneNumber;
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
