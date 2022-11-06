using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieForum.Data.Models;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Interfaces;
using MovieForum.Web.Helpers;
using MovieForum.Web.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MovieForum.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IUserServices userService;
        private readonly IAuthHelper authHelper;

        public IWebHostEnvironment hostingEnvironment;

        public UserApiController(IUserServices userService, IAuthHelper authHelper, IWebHostEnvironment hostEnvironment)
        {
            this.userService = userService;
            this.authHelper = authHelper;
            this.hostingEnvironment = hostEnvironment;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await userService.GetUserDTOByIdAsync(id);
                return this.Ok(user);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("user/{id}/comments")]
        public async Task<IActionResult> GetAllCommentsByUser(int id)
        {
            try
            {
                var comments = await userService.GetAllCommentsAsync(id);
                return this.Ok(comments);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("user/{id}/movies")]
        public async Task<IActionResult> GetAllMoviesByUser(int id)
        {
            try
            {
                var comments = await userService.GetAllMoviesAsync(id);
                return this.Ok(comments);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            try
            {
                var user = await userService.DeleteAsync(email);
                return this.Ok(user);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Route("create/user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserViewModel user)
        {
            try
            {
                var userDTO = new UpdateUserDTO
                {
                    Password = user.Password,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                };
                var newUser = await userService.PostAsync(userDTO);
                return this.Ok(newUser);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update/user/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromForm] UpdateUserViewModel user)
        {
            try
            {
                var path = UploadPhoto(user.File);

                var userDTO = new UpdateUserDTO
                {
                    Password = user.Password,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                };

                if (user.PhoneNumber != null)
                {
                    userDTO.PhoneNumber = user.PhoneNumber;
                }

                if (path != null)
                {
                    userDTO.ImagePath = path;
                }

                var newUser = await userService.UpdateAsync(id, userDTO);
                return this.Ok(newUser);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }        

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel userModel)
        {
            try
            {
                var user = await authHelper.TryLogin(userModel.Credential, userModel.Password);
                return this.Ok("Logged in successfully");
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

       
        //Admin
        [HttpPut]
        [Route("block/{id}")]
        public async Task<IActionResult> Block(int id)
        {
            try
            {
                await userService.BlockUser(id);
                return this.Ok("User blocked successfully");
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("unblock/{id}")]
        public async Task<IActionResult> Unblock(int id)
        {
            try
            {
                await userService.UnblockUser(id);
                return this.Ok("User unblocked successfully");
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        private string UploadPhoto(IFormFile file)
        {
            if (file == null)
            {
                return null;
            }
            FileInfo fi = new FileInfo(file.FileName);
            var newFileName = "Image_" + DateTime.Now.TimeOfDay.Milliseconds + fi.Extension;
            if (!Directory.Exists(hostingEnvironment.WebRootPath + "\\Images\\"))
            {
                Directory.CreateDirectory(hostingEnvironment.WebRootPath + "\\Images\\");
            }
            var path = Path.Combine("", hostingEnvironment.WebRootPath + "\\Images\\" + newFileName);
            using (FileStream stream = System.IO.File.Create(path))
            {
                file.CopyTo(stream);
                stream.Flush();
            }
            return path;
        }
    }

}
