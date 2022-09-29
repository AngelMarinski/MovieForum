using MovieForum.Services.Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieForum.Services.Services.Interfaces
{
    public interface ICommentServices : ICRUDOperations<CommentDTO>
    {
        Task<CommentDTO> LikeComment(int commentId);

        Task<CommentDTO> DislikeComment(int commentId);
    }
}
