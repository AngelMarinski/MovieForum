using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieForum.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }

}
