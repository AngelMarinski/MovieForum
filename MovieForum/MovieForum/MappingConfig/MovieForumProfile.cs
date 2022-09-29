using AutoMapper;
using MovieForum.Data.Models;
using MovieForum.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieForum.Web.MappingConfig
{
    public class MovieForumProfile : Profile
    {
        public MovieForumProfile()
        {
            this.CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
