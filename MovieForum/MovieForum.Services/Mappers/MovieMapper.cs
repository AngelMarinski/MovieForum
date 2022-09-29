using MovieForum.Data.Models;
using MovieForum.Models;
using MovieForum.Services.Helpers;
using MovieForum.Services.Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieForum.Services.Mappers
{
    public static class MovieMapper
    {
        public static MovieDTO GetDTO(this Movie movie)
        {
            if(movie.Title == null)
            {
                throw new InvalidCastException(Constants.INVALID_DATA);
            }

            var movieDto = new MovieDTO
            {
                Id = movie.Id,
                AuthorId = movie.AuthorID,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                Posted = movie.Posted,
                GenreId = movie.GenreId,
                Cast = new List<MovieActor>(movie.Cast),
                Tags = new List<MovieTags>(movie.Tags),
                Rating = movie.Rating,
                LikesCount = movie.LikesCount,
                DislikesCount = movie.DislikesCount
            };

            return movieDto;
        }
    }
}
