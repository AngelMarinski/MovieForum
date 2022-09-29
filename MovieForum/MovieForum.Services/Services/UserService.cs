using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieForum.Data;
using MovieForum.Data.Models;
using MovieForum.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieForum.Services.Services
{
    class UserService : ICRUDOperations<UserDTO>
    {
        private readonly MovieForumContext db;
        private readonly IMapper mapper;

        public UserService(MovieForumContext context, IMapper mapper)
        {
            this.db = context;
            this.mapper = mapper;
        }

        public async Task<User> GetUserAsync(int id)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            return user ?? throw new Exception();
        }

        public async Task<User> GetUserAsync(string username)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Username == username);
            return user ?? throw new Exception();
        }

        public int UserCount()
        {
            var numOfUsers = db.Users.Count();
            return numOfUsers;
        }

        public async Task<bool> IsExistingAsync(string email)
        {
            return await db.Users.AnyAsync(x => x.Email == email);
        }


        public async Task<UserDTO> GetUsernameAsync(string username)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Username == username);

            return mapper.Map<UserDTO>(user) ?? throw new Exception();
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync(int userId)
        {
            var comments = await db.Users.Where(x => x.Id == userId).Select(x => x.Comments).ToListAsync();
            //TODO replace comments with commentDTO
            //TODO remove cast
            return (IEnumerable<Comment>)comments;
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync(string username)
        {
            var comments = await db.Users.Where(x => x.Username == username).Select(x => x.Comments).ToListAsync();
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

        public Task<UserDTO> UpdateAsync(int id, UserDTO obj)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDTO> DeleteAsync(int id)
        {
            var userToDelete = await GetUserAsync(id);
            userToDelete.IsDeleted = true;
            await db.SaveChangesAsync();
            return mapper.Map<UserDTO>(userToDelete) ?? throw new Exception();
        }
    }
}
