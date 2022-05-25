using Microsoft.AspNetCore.Mvc;
using Princess.Models;
using Princess.Services;
using System.Diagnostics;

namespace Princess.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PresenceHandler _handler;

        public HomeController(ILogger<HomeController> logger, PresenceHandler handler)
        {
            _logger = logger;
            _handler = handler;
        }

        public async Task<IActionResult> Index()
        {
            //var model = await _handler.GetAllAbsenceAttendees();
            //var model = await _handler.GetAllAttendees();
            //var model = await _handler.GetAllPresenceAttendees();
            //var model = await _handler.GetAbsenceAttendee("Ronni");
            var model = await _handler.GetPresenceAttendee("Ronni");
            return View(model);
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