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
        private async Task<AttendanceView> GetAttendanceList(int currentPage)
        {
            var maxRowsPerPage = 10;
            var attendanceModel = new AttendanceView();

            var getAttendanceList = await _presenceHandler.GetAllAttendees(DateTime.Today, "Win21", "Björn");

            attendanceModel.AttendanceList = (from student in getAttendanceList select student)
                .OrderBy(x => x.Student.Name)
                .OrderByDescending(x => x.Lecture.Date)
                .Skip((currentPage - 1) * maxRowsPerPage)
                .Take(maxRowsPerPage)
                .ToList();

            var pageCount = (double)(getAttendanceList.Count / Convert.ToDecimal(maxRowsPerPage));
            
            attendanceModel.PageCount = (int)Math.Ceiling(pageCount); 
            attendanceModel.CurrentPageIndex = currentPage; 
            
            return attendanceModel;
        }

        [HttpGet]
        public async Task<IActionResult> Index() 
         { 
             return View(await GetAttendanceList(1));
         }
        [HttpPost]
        public async Task<IActionResult> Index(int currentPageIndex)
        {
            return View(await GetAttendanceList(currentPageIndex));
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