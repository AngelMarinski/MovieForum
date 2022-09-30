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

namespace MovieForum.Services.Services
{
    public class CommentServices : ICommentServices
    {
        private readonly MovieForumContext data;

        public CommentServices(MovieForumContext forumData)
        {
            this.data = forumData;
        }
        public async Task<CommentDTO> DeleteAsync(int id)
        {
            var comment = await data.Comments.Include(x => x.Author).Include(x => x.Movie).FirstOrDefaultAsync(x => x.Id == id);

            var commentDto = comment.GetDTO();

            comment.DeletedOn = DateTime.Now;
            data.Comments.Remove(comment);
            await data.SaveChangesAsync();

            return commentDto;                
        }

        public async Task<CommentDTO> DislikeComment(int commentId)
        {
            var comment = await data.Comments.Include(x => x.Movie).Include(x => x.Author).FirstOrDefaultAsync(x => x.Id == commentId);

            comment.DisLikesCount++;

            var commentDTO = comment.GetDTO();

            return commentDTO;
        }

        public async Task<IEnumerable<CommentDTO>> GetAsync()
        {
            var comments = await data.Comments.Include(x => x.Movie).Include(x => x.Author).Select(x=>x.GetDTO()).ToListAsync();

            return comments;
        }

        public async Task<CommentDTO> LikeComment(int commentId)
        {
            var comment = await data.Comments.Include(x => x.Movie).Include(x => x.Author).FirstOrDefaultAsync(x => x.Id == commentId);

            comment.LikesCount++;

            var commentDTO = comment.GetDTO();

            return commentDTO;
        }

        public async Task<CommentDTO> PostAsync(CommentDTO obj)
        {

            throw new NotImplementedException();

        }

        public async Task<CommentDTO> UpdateAsync(int id, CommentDTO obj)
        {
            throw new NotImplementedException();
        }
    }
}
