using MovieForum.Data.Models;
using MovieForum.Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieForum.Web.Models
{
    public class UpdatePostViewModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime ReleaseDate { get; set; }

        public Genre Genre { get; set; }

        public ICollection<MovieActorDTO> Cast { get; set; }

        public ICollection<MovieTagsDTO> Tags { get; set; }
    }
}
