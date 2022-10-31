using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Interfaces;
using MovieForum.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MovieForum.Web.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentServices services;
        private readonly IUserServices userServices;

        public CommentController(ICommentServices services, IUserServices userServices)
        {
            this.services = services;
            this.userServices = userServices;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var comments = await this.services.GetAsync();
            return View(comments);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
		{
			var com = new CreateCommentViewModel();

			return this.View(com);
		}

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(MovieCommentWrap comment)
        {
            

            var user = await this.userServices.GetUserByEmailAsync(this.User.Identity.Name);

            var result = new CommentDTO
            {
                AuthorId = user.Id,
                Content = comment.commentViewModel.Content,
                MovieId = comment.commentViewModel.MovieId,
                PostedOn = System.DateTime.Now   
            };

            var check = await services.PostAsync(result);

            return this.RedirectToAction("Movie", "Movies", new { id = comment.commentViewModel.MovieId });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Like(MovieCommentWrap comment)
        {
            var user = await this.userServices.GetUserByEmailAsync(this.User.Identity.Name);

            await services.LikeCommentAsync(comment.commentViewModel.Id, user.Id);

            return this.RedirectToAction("Movie", "Movies", new { id = comment.commentViewModel.MovieId });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Dislike(MovieCommentWrap comment)
        {
            var user = await this.userServices.GetUserByEmailAsync(this.User.Identity.Name);

            await services.DislikeCommentAsync(comment.commentViewModel.Id, user.Id);

            return this.RedirectToAction("Movie", "Movies", new { id = comment.commentViewModel.MovieId });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(MovieCommentWrap comment)
        {
            await services.DeleteAsync(comment.commentViewModel.Id);

            return this.RedirectToAction("Movie", "Movies", new { id = comment.commentViewModel.MovieId });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(MovieCommentWrap comment)
        {
            await services.UpdateAsync(comment.commentViewModel.Id,comment.commentViewModel);

            return this.RedirectToAction("Movie", "Movies", new { id = comment.commentViewModel.MovieId });
        }



    }
}
