using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Helpers;
using MovieForum.Services.Interfaces;
using MovieForum.Services.Services;
using MovieForum.Web.Models;
using System;
using System.Threading.Tasks;

namespace MovieForum.Web.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class TagApiController : ControllerBase
    {
        private readonly ITagServices tagService;

        public TagApiController(ITagServices services)
        {
            this.tagService = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            try
            {
                var tags = await tagService.GetAsync();
                return Ok(tags);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTagById(int id)
        {
            try
            {
                var tag = await tagService.GetTagByIdAsync(id);
                return Ok(tag);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);                
            }
        }

        [HttpGet("/tagView")]
        public async Task<IActionResult> GetTagByName(string name)
        {
            try
            {
                var tag = await tagService.GetTagByNameAsync(name);
                return Ok(tag);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTag(int tagId, TagView tagView)
        {
            var tagDTO = new TagDTO
            {
                TagName = tagView.TagName
            };

            try
            {
                var tagToUpdate = await tagService.UpdateAsync(tagId, tagDTO);
                return Ok(tagToUpdate);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewTag(TagView tagView)
        {
            var tagDTO = new TagDTO
            {
                TagName = tagView.TagName
            };

            try
            {
                var tagToUpdate = await tagService.PostAsync(tagDTO);
                return Ok(tagToUpdate);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]

        public async Task<IActionResult>DeleteTag(int id)
        {
            try
            {
                var tagToDel = await tagService.DeleteAsync(id);
                return Ok(Constants.DELETED_TAG);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
