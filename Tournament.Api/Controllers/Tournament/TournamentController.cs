using Microsoft.AspNetCore.Mvc;

namespace Tournament.Api.Controllers.Tournament
{
    public class TournamentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
