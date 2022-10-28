using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Interfaces;
using MovieForum.Web.Helpers;
using MovieForum.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MovieForum.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthHelper authHelper;
        private readonly IUserServices userService;

        public AuthController(IAuthHelper authHelper, IUserServices userService)
        {
            this.authHelper = authHelper;
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            var register = new CreateUserViewModel();
            return this.View(register);
        }

        //[Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Register(CreateUserViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }
            if (await userService.IsExistingAsync(model.Email))
            {
                this.ModelState.AddModelError("Email", "User with this email address already exists.");
            }
            if (await userService.IsExistingUsernameAsync(model.Username))
            {
                this.ModelState.AddModelError("Username", "User with this username already exists.");
            }

            var userDTO = new UpdateUserDTO
            {
                Password = model.Password,
                Username = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email
            };

            var newUser = await userService.PostAsync(userDTO);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            var login = new LoginViewModel();
            return this.View(login);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!await userService.IsExistingAsync(model.Email))
            {
                this.ModelState.AddModelError("Email", "User with this email address doesn't exists.");
                return this.View(model);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                var user = await authHelper.TryLogin(model.Email, model.Password);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.Email),
                    new Claim("Username", user.Username),
                    new Claim("Full Name", user.FirstName +" " + user.LastName)
                };

                if(user.RoleId == 1)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, "User"));
                }

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var AuthProperties = new AuthenticationProperties
                {
                    IsPersistent = false
                };

                if(model.RememberMe == true)
                {
                    AuthProperties.IsPersistent = true;
                }

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    AuthProperties
                );
            }
            catch (Exception)
            {
                this.ModelState.AddModelError("Password", "Incorrect password.");
                return this.View(model);
            }

            return this.RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            // Clear the existing external cookie
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return this.RedirectToAction("Index", "Home");
        }
    }
}
