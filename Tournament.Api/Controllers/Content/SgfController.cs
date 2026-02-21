using Microsoft.AspNetCore.Mvc;

namespace Tournament.Api.Controllers.Content
{
    public class SgfController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
