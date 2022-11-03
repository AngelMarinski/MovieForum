using Microsoft.AspNetCore.Mvc;

namespace MovieForum.Web.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
