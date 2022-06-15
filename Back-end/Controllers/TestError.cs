using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    public class TestError : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
