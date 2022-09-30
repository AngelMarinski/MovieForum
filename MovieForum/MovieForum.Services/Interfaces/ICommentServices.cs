using MovieForum.Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieForum.Services.Interfaces
{
    public interface ICommentServices : ICRUDOperations<CommentDTO>
    {
        Task<CommentDTO> LikeCommentAsync(int commentId, int userId);

        Task<CommentDTO> DislikeCommentAsync(int commentId, int userId);

        Task<CommentDTO> GetCommentByIdAsync(int id);
    }
}
