using MovieForum.Services.DTOModels;
using MovieForum.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieForum.Web.Models
{
    public class MoviesSearchModelWrap
    {
        public IEnumerable<MovieDTO> Movies { get; set; }
        public MovieQueryParameters Parameters { get; set; }
    }
}
