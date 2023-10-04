using Microsoft.AspNetCore.Mvc;

namespace AcademicBlog.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
