using AutoMapper;
using MovieForum.Data.Models;
using MovieForum.Services.DTOModels;

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
