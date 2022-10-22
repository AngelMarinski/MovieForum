using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Interfaces;
using MovieForum.Web.Models;
using System;
using System.Threading.Tasks;

namespace MovieForum.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {

        private readonly IUserServices userService;

        public UserController( IUserServices userService)
        {
            this.userService = userService;
        }
        [HttpGet]
        public  async Task<IActionResult> Update()
        {
            var user = await userService.GetUserByEmailAsync(this.User.Identity.Name);

            var update= new UpdateUserViewModel
            {
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            return this.View(update);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(UpdateUserViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                var user = await userService.GetUserByEmailAsync(this.User.Identity.Name);

                if (await userService.IsExistingAsync(model.Email) && model.Email != user.Email)
                {
                    this.ModelState.AddModelError("Email", "User with this email address already exists.");
                }

                var userDTO = new UpdateUserDTO();

                userDTO.Password = model.Password ?? user.Password;
                userDTO.FirstName = model.FirstName ?? user.FirstName;
                userDTO.LastName = model.LastName ?? user.LastName;
                userDTO.Email = model.Email ?? user.Email;

                if (model.File != null)
                {
                    userDTO.ImagePath = model.File.FileName;
                }
                if (model.PhoneNumber != null)
                {
                    userDTO.PhoneNumber = model.PhoneNumber;
                }

                await userService.UpdateAsync(user.Id, userDTO);

            }
            catch (Exception)
            {
              return this.View(model);
            }


            return this.RedirectToAction("Index", "Home");
        }       
    }
}
