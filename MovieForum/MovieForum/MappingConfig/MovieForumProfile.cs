using AutoMapper;
using MovieForum.Data.Models;
using MovieForum.Services.DTOModels;

namespace MovieForum.Web.MappingConfig
{
    public class MovieForumProfile : Profile
    {
        public MovieForumProfile()
        {
            this.CreateMap<User, UserDTO>()
                .ForMember(dest => dest.UserId, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.Role, act => act.MapFrom(src => src.Role.Name))
                .ReverseMap();

            this.CreateMap<Comment, CommentDTO>().ReverseMap();
        }
    }
}
