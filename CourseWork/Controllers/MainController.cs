using Microsoft.AspNetCore.Mvc;

namespace CourseWork.Controllers
{
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
