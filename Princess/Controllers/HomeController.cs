using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Princess.Models;
using Princess.Services;
using System.Diagnostics;

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
        [HttpPost]
        public async Task<JsonResult> GetTeachersClass(string classId, string ddType) //ddType= dropdownType
        {
            var allClassList = await _presenceHandler.GetAllSchoolclasses();
            List<SelectListItem> result = new List<SelectListItem>();
            bool isSucceed = true;
            try
            {
                switch (ddType)
                {
                    case "getClass":
                        foreach (var firstDropdown in allClassList)
                        {
                            result.Add(new SelectListItem
                            {
                                Text = firstDropdown.Name,
                                Value = firstDropdown.Id.ToString()
                            });
                        }
                        break;
                    case "getTeacher":
                        foreach (var secondDropdown in allClassList.Where(x => x.Id == ulong.Parse(classId)))
                        {
                            foreach (var item in secondDropdown.Teachers)
                            {
                                result.Add(new SelectListItem
                                {
                                    Text = item.Name,
                                    Value = item.Id.ToString()
                                });
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                //if any error occurs
                isSucceed = false;
                result = new List<SelectListItem>();
                result.Add(new SelectListItem
                {
                    Text = "Got Issue",
                    Value = "Default"
                });

            }
            return new JsonResult(new { ok = isSucceed, text = result });
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