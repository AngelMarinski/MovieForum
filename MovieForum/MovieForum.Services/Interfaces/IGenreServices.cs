using MovieForum.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieForum.Services.Interfaces
{
    public interface IGenreServices
    {
        Task<IEnumerable<Genre>> GetAll();
    }
}
