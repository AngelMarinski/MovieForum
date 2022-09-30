using Microsoft.AspNetCore.Mvc;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Interfaces;
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

        public UserApiController(IUserServices userService)
        {
            this.userService = userService;
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
    }

}
