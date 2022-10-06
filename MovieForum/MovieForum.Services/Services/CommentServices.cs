using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieForum.Data;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieForum.Services.Helpers;

using MovieForum.Data.Models;

namespace MovieForum.Services.Services
{
    public class CommentServices : ICommentServices
    {
        private readonly MovieForumContext data;
        private readonly IMapper map;

        public CommentServices(MovieForumContext forumData, IMapper mapper)
        {
            this.data = forumData;
            this.map = mapper;
        }

        public async Task<CommentDTO> DeleteAsync(int id)
        {
            var comment = await data.Comments.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false)
                ?? throw new InvalidOperationException(Constants.COMMENT_NOT_FOUND);

            var commentDto = map.Map<CommentDTO>(comment);

            //var user = await data.Users.FirstOrDefaultAsync(x=>x.Id == comment.AuthorId);
            //user.Comments.Remove(comment);

            //var movie = await data.Movies.FirstOrDefaultAsync(x => x.Id == comment.MovieId);
            //movie.Comments.Remove(comment);

            comment.DeletedOn = DateTime.Now;
            comment.IsDeleted = true;

            await data.SaveChangesAsync();

            return commentDto;
        }


        public async Task<IEnumerable<CommentDTO>> GetAsync()
        {
            var comments = await data.Comments.Where(x=>x.IsDeleted == false).Select(x => map.Map<CommentDTO>(x)).ToListAsync();

            if (comments.Count == 0)
            {
                throw new InvalidOperationException(Constants.NO_COMMENTS_FOR_THIS_MOVIE);
            }

            return comments;
        }

        public async Task<CommentDTO> GetCommentByIdAsync(int id)
        {
            var comment = await data.Comments.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false)
                ?? throw new InvalidOperationException(Constants.COMMENT_NOT_FOUND);

            return map.Map<CommentDTO>(comment);
        }

        public async Task<CommentDTO> DislikeCommentAsync(int commentId, int userId)
        {
            var comment = await data.Comments.FirstOrDefaultAsync(x => x.Id == commentId && x.IsDeleted == false)
                ?? throw new InvalidOperationException(Constants.COMMENT_NOT_FOUND);

            var user = await data.Users.FirstOrDefaultAsync(x => x.Id == userId)
                ?? throw new InvalidOperationException(Constants.USER_NOT_FOUND);

            var reaction = comment.Reactions.FirstOrDefault(x => x.UserId == userId);

            if (reaction == null)
            {
                var disLiked = new Reaction
                {
                    UserId = userId,
                    Liked = false,
                    Disliked = true
                };
                comment.Reactions.Add(disLiked);
                comment.DisLikesCount++;
            }

            else if (reaction.UserId == userId && reaction.Disliked == true && reaction.Liked == false)
            {
                comment.Reactions.Remove(reaction);
                comment.DisLikesCount--;
            }
            else if (reaction.UserId == userId && reaction.Disliked == false && reaction.Liked == true)
            {
                reaction.Liked = false;
                reaction.Disliked = true;
                comment.DisLikesCount++;
                comment.LikesCount--;
            }

            await data.SaveChangesAsync();

            var commentDTO = map.Map<CommentDTO>(comment);


            return commentDTO;
        }

        public async Task<CommentDTO> LikeCommentAsync(int commentId, int userId)
        {
            var comment = await data.Comments.FirstOrDefaultAsync(x => x.Id == commentId && x.IsDeleted == false)
                ?? throw new InvalidOperationException(Constants.COMMENT_NOT_FOUND);

            var user = await data.Users.FirstOrDefaultAsync(x => x.Id == userId)
                ?? throw new InvalidOperationException(Constants.USER_NOT_FOUND);

            var reaction = comment.Reactions.FirstOrDefault(x => x.UserId == userId);

            if (reaction == null)
            {
                var liked = new Reaction
                {
                    UserId = userId,
                    Liked = true,
                    Disliked = false
                };
                comment.Reactions.Add(liked);
                comment.LikesCount++;
            }

            else if (reaction.UserId == userId && reaction.Disliked == false && reaction.Liked == true)
            {
                comment.Reactions.Remove(reaction);
                comment.LikesCount--;
            }
            else if (reaction.UserId == userId && reaction.Disliked == true && reaction.Liked == false)
            {
                reaction.Liked = true;
                reaction.Disliked = false;
                comment.DisLikesCount--;
                comment.LikesCount++;
            }

            await data.SaveChangesAsync();

            var commentDTO = map.Map<CommentDTO>(comment);


            return commentDTO;
        }


        public async Task<CommentDTO> PostAsync(CommentDTO obj)
        {
            var author = await data.Users.FirstOrDefaultAsync(x => x.Id == obj.AuthorId) ??
                throw new InvalidOperationException(Constants.USER_NOT_FOUND);

            var movie = await data.Movies.FirstOrDefaultAsync(x => x.Id == obj.MovieId) ??
                throw new InvalidOperationException(Constants.MOVIE_NOT_FOUND);

            if (author.IsBlocked)
            {
                throw new InvalidOperationException("This user is blocked and can not post comments!");
            }

            if (obj.AuthorId is null || obj.Content is null || obj.MovieId is null)
            {
                throw new NullReferenceException("All of the fileds are required!");
            }

            obj.AuthorUsername = author.Username;

            var comment = new Comment
            {               
                Author = author,
                Content = obj.Content,
                Movie = await data.Movies.FirstOrDefaultAsync(x => x.Id == obj.MovieId),
                IsDeleted = false,
                PostedOn = obj.PostedOn               
                
            };

            await data.Comments.AddAsync(comment);

            movie.Comments.Add(comment);

            author.Comments.Add(comment);

            await data.SaveChangesAsync();

            return map.Map<CommentDTO>(comment);

        }

        public async Task<CommentDTO> UpdateAsync(int id, CommentDTO obj)
        {
            var commentToUpdate = await data.Comments.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false)
                ?? throw new InvalidOperationException(Constants.COMMENT_NOT_FOUND);
                       
            if (obj.Content != null)
            {
                commentToUpdate.Content = obj.Content + " (Edited)";
            }

            await data.SaveChangesAsync();

            return map.Map<CommentDTO>(commentToUpdate);
        }
    }
}
