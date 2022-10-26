using Microsoft.AspNetCore.Mvc;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace MovieForum.Web.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentServices services;

        public CommentController(ICommentServices services)
        {
            this.services = services;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var comments = await this.services.GetAsync();
            return View(comments);
        }

        [HttpGet]
		public async Task<IActionResult> CreateAsync()
		{
			var com = new CommentDTO();

			return this.View(com);
		}

		

	}
}
