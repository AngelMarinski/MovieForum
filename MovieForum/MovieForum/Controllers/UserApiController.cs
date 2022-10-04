using Microsoft.AspNetCore.Mvc;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Interfaces;
using MovieForum.Web.Helpers;
using MovieForum.Web.Models;
using System;
using System.Threading.Tasks;

namespace MovieForum.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IUserServices userService;
        private readonly IAuthHelper authHelper;

        public UserApiController(IUserServices userService, IAuthHelper authHelper)
        {
            this.userService = userService;
            this.authHelper = authHelper;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await userService.GetUserByIdAsync(id);
                return this.Ok(user);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await userService.DeleteAsync(id);
                return this.Ok(user);
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Route("user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserViewModel user)
        {
            try
            {
                var userDTO = new UserDTO
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

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel userModel)
        {
            try
            {
                var user = await authHelper.TryLogin(userModel.Email, userModel.Password);
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
    }

}
