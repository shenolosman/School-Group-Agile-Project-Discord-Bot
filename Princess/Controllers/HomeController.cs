using Microsoft.AspNetCore.Mvc;
using Princess.Models;
using System.Diagnostics;
using Princess.Services;

namespace Princess.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PresenceHandler _presenceHandler;
        public HomeController(ILogger<HomeController> logger, PresenceHandler presenceHandler)
        {
            _logger = logger;
            _presenceHandler = presenceHandler;
        }

        [HttpGet]
        public async Task<IActionResult> Index() 
         { 
             return View();
         }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}