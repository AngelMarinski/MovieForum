using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieForum.Services.Interfaces;
using System.Threading.Tasks;

namespace MovieForum.Web.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentsApiController : ControllerBase
    {
        private readonly ICommentServices comServ;

        public CommentsApiController(ICommentServices comServ)
        {
            this.comServ = comServ;
        }

        [HttpGet]
        public async Task<IActionResult> GetCommentsAsync()
        {
            var comments = await comServ.GetAsync();

            return StatusCode(200, comments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentByIdAsync(int id)
        {
            var comment = comServ.GetCommentByIdAsync(id);

            return StatusCode(200, comment);
        }
    }
}
