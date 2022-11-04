using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MovieForum.Data.Models;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Interfaces;
using MovieForum.Services.Models;
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
            try
            {
                var userDTO = new UpdateUserDTO
                {
                    Password = model.Password,
                    Username = model.Username,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    ImagePath = "defaultphoto.jpg"
                };

                var newUser = await userService.PostAsync(userDTO);
                var user = new User
                {
                    Username = newUser.Username,
                };
                await userService.GenerateEmailConfirmationTokenAsync(user);
            }catch (Exception)
            {
                this.ModelState.AddModelError("Password", "Incorrect password.");
                return this.View(model);
            }
            return View("ConfirmEmail", new EmailConfirmModel());
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
                this.ModelState.AddModelError("Email", "Incorrect combination of email and password.");
                return this.View(model);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                var user = await authHelper.TryLogin(model.Email, model.Password);

                if(user == null)
                {
                    this.ModelState.AddModelError("IsEmailConfirmed", "You have to confirm your email.");
                    return this.View(model);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.Email),
                    new Claim("Username", user.Username),
                    new Claim("Full Name", user.FirstName +" " + user.LastName),
                    new Claim("Image", user.ImagePath)
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
                    IsPersistent = false,
                    ExpiresUtc = DateTime.UtcNow.AddDays(14)
                };


                if (model.RememberMe == true)
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
                this.ModelState.AddModelError("Password", "Incorrect combination of email and password.");
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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // code here
                try
                {
                    var user = await userService.GetUserByEmailAsync(model.Email);
                    await userService.GenerateForgotPasswordTokenAsync(user);

                    ModelState.Clear();
                    model.EmailSent = true;
                }
                catch (Exception )
                {
                    model.EmailSent=false;
                    return View();
                }
            }
            return View(model);
        }


        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token)
        {
            EmailConfirmModel model = new EmailConfirmModel();
            

            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace(' ', '+');
                if (await userService.ConfirmEmailAsync(uid, token))
                {
                    model.EmailVerified = true;
                }
            }

            return View(model);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmModel model)
        {
            var user = await userService.GetUserByEmailAsync(model.Email);

            if (user != null)
            {
                user.IsEmailConfirmed = model.IsConfirmed;

                if (user.IsEmailConfirmed)
                {
                    model.EmailVerified = true;
                    return View(model);
                }

                await userService.GenerateEmailConfirmationTokenAsync(user);
                model.EmailSent = true;
                ModelState.Clear();
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong.");
            }
            return View(model);
        }


        [HttpGet("reset-password")]
        [AllowAnonymous]
        public IActionResult ResetPassword(string uid, string token)
        {
            ResetPasswordModel resetPasswordModel = new ResetPasswordModel
            {
                Token = token,
                UserId = uid
            };
            return View(resetPasswordModel);
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                model.Token = model.Token.Replace(' ', '+');
                var result = await userService.ResetPasswordAsync(model);
                if (result)
                {
                    ModelState.Clear();
                    model.IsSuccess = true;
                    return View(model);
                }
                    ModelState.AddModelError("","Can't channge password");
                
            }
            return View(model);
        }
    }
}
