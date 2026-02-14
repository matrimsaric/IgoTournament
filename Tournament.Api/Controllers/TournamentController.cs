using Microsoft.AspNetCore.Mvc;

namespace Tournament.Api.Controllers
{
    public class TournamentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
