using Microsoft.AspNetCore.Mvc;

namespace Tournament.Api.Controllers.Tournament
{
    public class RoundController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
