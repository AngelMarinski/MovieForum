using AutoMapper;
using MovieForum.Data.Models;
using MovieForum.Models;
using MovieForum.Services.DTOModels;
using System.Linq;

namespace MovieForum.Web.MappingConfig
{
    public class MovieForumProfile : Profile
    {
        public MovieForumProfile()
        {
            this.CreateMap<MovieActor, MovieActorDTO>().ReverseMap();

            this.CreateMap<MovieTags, MovieTagsDTO>()
                .ForMember(dest => dest.TagName, act => act.MapFrom(src => src.Tag.TagName))
                .ReverseMap();

            this.CreateMap<TagDTO, Tag>();

            this.CreateMap<User, UserDTO>()
                .ForMember(dest => dest.UserId, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.Role, act => act.MapFrom(src => src.Role.Name))
                .ReverseMap();

            this.CreateMap<User, UpdateUserDTO>()
                .ForMember(dest => dest.UserId, act => act.MapFrom(src => src.Id))
                .ReverseMap();

            this.CreateMap<UserDTO, UpdateUserDTO>()
                .ReverseMap();

            this.CreateMap<Comment, CommentDTO>()
               .ForMember(dest => dest.AuthorUsername, act => act.MapFrom(src => src.Author.Username))
               .ForMember(dest => dest.PostedOn, act => act.MapFrom(src => src.PostedOn.Value.ToString("dd/MM/yyyy HH:mm"))) 
               .ReverseMap();

            this.CreateMap<Movie, MovieDTO>()
                 .ForMember(dest => dest.Username, act => act.MapFrom(src => src.Author.Username))
                 //.ForMember(dest => dest.Rating, act => act.MapFrom(src => (double)(src.Rating.Sum(x => x.Rate)/src.Rating.Count)))
                 .ReverseMap();

            this.CreateMap<Actor, ActorDTO>()
                .ForMember(dest => dest.FirstName, act => act.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, act => act.MapFrom(src => src.LastName))
                .ReverseMap();

        }
    }
}
