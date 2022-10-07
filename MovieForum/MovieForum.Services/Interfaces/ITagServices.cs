using MovieForum.Services.DTOModels;
using MovieForum.Services.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieForum.Services.Interfaces
{
    public interface ITagServices : ICRUDOperations<TagDTO>
    {
        Task<TagDTO> GetTagByIdAsync(int tagId);

        Task<TagDTO> GetTagByNameAsync(string tagName);

        //Task<string> DeleteTagFromMovieAsync(int movieId, int tagId);




    }
}
