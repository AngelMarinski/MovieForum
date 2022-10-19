using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Interfaces;
using MovieForum.Web.Models;
using System;
using System.Threading.Tasks;

namespace MovieForum.Web.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentsApiController : ControllerBase
    {
        private readonly ICommentServices comServ;
        private readonly IMapper map;
        public CommentsApiController(ICommentServices comServ, IMapper map)
        {
            this.comServ = comServ;
            this.map = map;
        }

        [HttpGet]
        public async Task<IActionResult> GetCommentsAsync()
        {
            try
            {
                var comments = await comServ.GetAsync();

                //return StatusCode(200, comments);
                return this.Ok(comments);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);

            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentByIdAsync(int id)
        {
            try
            {
                var comment = await comServ.GetCommentByIdAsync(id);

                return Ok(comment);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostCommentAsync([FromBody] CreateCommentViewModel comment)
        {
            var commentDTO = new CommentDTO
            {
                AuthorId = comment.AuthorId,
                MovieId = comment.MovieId,
                Content = comment.Content,
                PostedOn = DateTime.Now

            };
            try
            {
                var result = await comServ.PostAsync(commentDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/dislike")]
        public async Task<IActionResult> DisLikeComment(int id, int userId)
        {
            try
            {
                var commentDTo = await comServ.DislikeCommentAsync(id, userId);

                return Ok(commentDTo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/like")]
        public async Task<IActionResult> LikeComment(int id, int userId)
        {
            try
            {
                var commentDTo = await comServ.LikeCommentAsync(id, userId);

                return Ok(commentDTo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] UpdateCommentView obj)
        {
            var commentDTO = new CommentDTO
            {
                Content = obj.Content
            };

            try
            {
                var result = await comServ.UpdateAsync(id, commentDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);

            }


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment (int id)
        {
            try
            {
                var result = await comServ.DeleteAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
