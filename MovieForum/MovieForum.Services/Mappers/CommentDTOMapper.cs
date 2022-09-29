using MovieForum.Data.Models;
using MovieForum.Services.Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieForum.Services.Mappers
{
    public static class CommentDTOMapper
    {
        public static CommentDTO GetDTO(this Comment comment)
        {
            if (comment is null || comment.Id <= 0 || comment.MovieId <= 0 || comment.Movie.Title is null
                || comment.Title is null || comment.AuthorId <= 0 || comment.Content is null)
            {
                throw new InvalidOperationException();
            }

            return new CommentDTO
            {
                Id = comment.Id,
                Title = comment.Title,
                Content = comment.Content,
                PostedOn = comment.PostedOn,
                MovieId = comment.MovieId,
                AuthorId = comment.AuthorId,
                AuthorUsername = comment.Author.Username
            };

        }
        public static Comment GetEntity(this CommentDTO comment)
        {
            if (comment is null || comment.Id <= 0 || comment.MovieId <= 0
                || comment.Title is null || comment.AuthorId <= 0 || comment.Content is null)
            {
                throw new InvalidOperationException();
            }

            return new Comment
            {
                Id = comment.Id,
                Title = comment.Title,
                Content = comment.Content,
                PostedOn = comment.PostedOn,
                MovieId = comment.MovieId,
                AuthorId = comment.AuthorId

            };
        }
    }
}
