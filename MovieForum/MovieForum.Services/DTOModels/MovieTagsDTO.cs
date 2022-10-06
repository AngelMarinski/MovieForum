using MovieForum.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieForum.Services.DTOModels
{
    public class MovieTagsDTO
    {
        public string TagName { get; set; }
        public int? MovieId { get; set; }
        public MovieDTO Movie { get; set; }

        public int? TagId { get; set; }

        //public TagDTO Tag { get; set; }
    }
}
