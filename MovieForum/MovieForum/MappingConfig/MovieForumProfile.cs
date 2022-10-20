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
               .ForMember(dest => dest.PostedOn, act => act.MapFrom(src => src.PostedOn.Value.ToString())) 
               .ReverseMap();

            this.CreateMap<Movie, MovieDTO>()
                 .ForMember(dest => dest.Username, act => act.MapFrom(src => src.Author.Username))
                 .ForMember(dest => dest.Rating, act => act.MapFrom(src =>  src.Rating.Sum(x=>(double)x.Rate)/src.Rating.Count))
                 .ForMember(dest => dest.Tags, act => act.MapFrom(src => src.Tags.Where(x => x.IsDeleted == false).ToList()))
                 .ReverseMap();

            this.CreateMap<Actor, ActorDTO>()
                .ForMember(dest => dest.FirstName, act => act.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, act => act.MapFrom(src => src.LastName))
                .ReverseMap();

            this.CreateMap<Tag, TagDTO>().ReverseMap();

        }
    }
}
