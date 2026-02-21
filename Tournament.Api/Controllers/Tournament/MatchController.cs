using Microsoft.AspNetCore.Mvc;

namespace Tournament.Api.Controllers.Tournament
{
    public class MatchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
