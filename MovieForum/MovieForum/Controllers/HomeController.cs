using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
    public class HomeController : Controller
    {
        private readonly IAuthHelper authHelper;
        private readonly IUserServices userService;

        public HomeController(IAuthHelper authHelper, IUserServices userService)
        {
            this.authHelper = authHelper;
            this.userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
