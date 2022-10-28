using Microsoft.EntityFrameworkCore;
using MovieForum.Data;
using MovieForum.Data.Models;
using MovieForum.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieForum.Services.Services
{
    public class GenreServices : IGenreServices
    {
        private readonly MovieForumContext context;

        public GenreServices(MovieForumContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Genre>> GetAll()
        {
            return await this.context.Genres.ToListAsync();
        }
    }
}
