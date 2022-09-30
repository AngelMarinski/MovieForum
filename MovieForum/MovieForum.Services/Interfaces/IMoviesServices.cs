﻿using MovieForum.Data.Models;
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
        Task<IEnumerable<MovieDTO>> FilterByAsync(MovieQueryParameters parameters);
    }
}
