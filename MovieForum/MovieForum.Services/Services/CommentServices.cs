using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieForum.Data;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Interfaces;
using MovieForum.Services.Mappers;
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
        private readonly IUserServices userServices;

        public CommentServices(MovieForumContext forumData, IMapper mapper, IUserServices userServices)
        {
            this.data = forumData;
            this.map = mapper;
            this.userServices = userServices;
        }

        public async Task<CommentDTO> DeleteAsync(int id)
        {
            var comment = await data.Comments.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false)
                ?? throw new InvalidOperationException(Constants.COMMENT_NOT_FOUND);

            var commentDto = map.Map<CommentDTO>(comment);

            comment.DeletedOn = DateTime.Now;
            data.Comments.Remove(comment);
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

            if (!comment.LikeDislikeMap.ContainsKey(userId))
            {
                comment.LikeDislikeMap.Add(userId, Constants.DISLIKED);
                comment.DisLikesCount++;
            }
            else if (comment.LikeDislikeMap.ContainsKey(userId) && comment.LikeDislikeMap[userId] == Constants.DISLIKED)
            {
                comment.LikeDislikeMap.Remove(userId);
                comment.DisLikesCount--;
            }
            else if (comment.LikeDislikeMap.ContainsKey(userId) && comment.LikeDislikeMap[userId] == Constants.LIKED)
            {
                comment.LikeDislikeMap[userId] = Constants.DISLIKED;
                comment.DisLikesCount++;
                comment.LikesCount--;
            }

            var commentDTO = map.Map<CommentDTO>(comment);

            return commentDTO;
        }

        public async Task<CommentDTO> LikeCommentAsync(int commentId, int userId)
        {
            var comment = await data.Comments.FirstOrDefaultAsync(x => x.Id == commentId && x.IsDeleted == false)
                ?? throw new InvalidOperationException(Constants.COMMENT_NOT_FOUND);

            var user = await data.Users.FirstOrDefaultAsync(x => x.Id == userId)
                ?? throw new InvalidOperationException(Constants.USER_NOT_FOUND);

            if (!comment.LikeDislikeMap.ContainsKey(userId))
            {
                comment.LikeDislikeMap.Add(userId, Constants.LIKED );
                comment.LikesCount++;
            }
            else if (comment.LikeDislikeMap.ContainsKey(userId) && comment.LikeDislikeMap[userId] == Constants.LIKED )
            {
                comment.LikeDislikeMap.Remove(userId);
                comment.LikesCount--;
            }
            else if (comment.LikeDislikeMap.ContainsKey(userId) && comment.LikeDislikeMap[userId] == Constants.DISLIKED)
            {
                comment.LikeDislikeMap[userId] = Constants.LIKED;
                comment.DisLikesCount--;
                comment.LikesCount++;

            }

            var commentDTO = map.Map<CommentDTO>(comment);

            return commentDTO;
        }

        public async Task<CommentDTO> PostAsync(CommentDTO obj)
        {
            var author = await data.Users.FirstOrDefaultAsync(x => x.Id == obj.AuthorId);
            if (author.IsBlocked)
            {
                throw new InvalidOperationException("This user is blocked and can not post comments!");
            }

            if (obj.AuthorId is null || obj.AuthorUsername is null || obj.Content is null || obj.MovieId is null 
                 || obj.Title is null)
            {
                throw new NullReferenceException("All of the fileds are required!");
            }

            var comment = new Comment
            {
                Author = await data.Users.FirstOrDefaultAsync(x => x.Id == obj.AuthorId),
                Title = obj.Title,
                Content = obj.Content,
                Movie = await data.Movies.FirstOrDefaultAsync(x => x.Id == obj.MovieId),
                IsDeleted = false
            };
            
            await data.Comments.AddAsync(comment);

            var movie = await data.Movies.FirstOrDefaultAsync(x => x.Id == comment.Movie.Id);
            movie.Comments.Add(comment);

            await data.SaveChangesAsync();

            return map.Map<CommentDTO>(comment);

        }

        public async Task<CommentDTO> UpdateAsync(int id, CommentDTO obj)
        {
            var commentToUpdate = await data.Comments.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false)
                ?? throw new InvalidOperationException(Constants.COMMENT_NOT_FOUND);

            if (obj.Title != null)
            {
                commentToUpdate.Title = obj.Title;
            }
            if (obj.Content != null)
            {
                commentToUpdate.Content = obj.Title;
            }

            await data.SaveChangesAsync();

            return map.Map<CommentDTO>(commentToUpdate);
        }
    }
}
