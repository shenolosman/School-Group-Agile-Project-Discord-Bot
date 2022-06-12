using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Princess.Models;
using Princess.Services;

namespace Princess.Pages.Class
{
    public class LectureModel : PageModel
    {
        private readonly PresenceHandler _presenceHandler;
        private readonly IConfiguration _configuration;

        public LectureModel(PresenceHandler presenceHandler, IConfiguration configuration)
        {
            _presenceHandler = presenceHandler;
            _configuration = configuration;
        }

        [BindProperty(SupportsGet = true)]
        public int LectureId { get; set; }


        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 10;
        //public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public List<Student> Students { get; set; }

        public PaginatedList<Presence> Presences { get; set; }
        public string StudentSort { get; set; }
        public string DateSort { get; set; }
        public string PresenceCheckbox { get; set; }
        public string ReasonForAbsence { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }


        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex, int lectureId)
        {
            //sorting with pagination
            CurrentSort = sortOrder;
            CurrentFilter = currentFilter;
            StudentSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Lecture.Date" ? "date_desc" : "Lecture.Date";
            PresenceCheckbox = sortOrder == "Attended" ? "attended_desc" : "Attended";
            ReasonForAbsence = sortOrder == "ReasonAbsence" ? "reasonAbsence_desc" : "ReasonAbsence";
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            CurrentFilter = searchString;
            var lecture = await _presenceHandler.GetLectureAsync(lectureId);
            IEnumerable<Presence> PresencesIQ = lecture.Presences.ToList();
            ViewData["List"] = PresencesIQ;
            if (!string.IsNullOrEmpty(searchString))
            {
                PresencesIQ = PresencesIQ.Where(s => s.Student.Name.ToLower().Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    PresencesIQ = PresencesIQ.OrderByDescending(s => s.Student.Name);
                    break;
                case "Date":
                    PresencesIQ = PresencesIQ.OrderBy(x => x.Lecture.Date);
                    break;
                case "date_desc":
                    PresencesIQ = PresencesIQ.OrderByDescending(s => s.Lecture.Date);
                    break;
                case "Attended":
                    PresencesIQ = PresencesIQ.OrderBy(s => s.Attended);
                    break;
                case "attended_desc":
                    PresencesIQ = PresencesIQ.OrderByDescending(s => s.Attended);
                    break;
                case "ReasonAbsence":
                    PresencesIQ = PresencesIQ.OrderBy(s => s.ReasonAbsence);
                    break;
                case "reasonAbsence_desc":
                    PresencesIQ = PresencesIQ.OrderByDescending(s => s.ReasonAbsence);
                    break;
                default:
                    PresencesIQ = PresencesIQ.OrderBy(s => s.Student.Name);
                    break;
            }
            var pageSize = _configuration.GetValue("PageSize", 10);
            Presences = await PaginatedList<Presence>.CreateAsync(PresencesIQ.ToList(), pageIndex ?? 1, pageSize);
        }
        public class PaginatedList<T> : List<T>
        {
            public int PageIndex { get; private set; }
            public int TotalPages { get; private set; }
            public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
            {
                PageIndex = pageIndex;
                TotalPages = (int)Math.Ceiling(count / (double)pageSize);

                this.AddRange(items);
            }
            public bool HasPreviousPage => PageIndex > 1;
            public bool HasNextPage => PageIndex < TotalPages;
            public static Task<PaginatedList<T>> CreateAsync(
                List<T> source, int pageIndex, int pageSize)
            {
                var count = source.Count();
                var items = source.Skip(
                        (pageIndex - 1) * pageSize)
                    .Take(pageSize).ToList();
                return Task.FromResult(new PaginatedList<T>(items, count, pageIndex, pageSize));
            }
        }


        // **************Markus************************
        //public async Task OnGetAsync()
        //{
        //    Students = await GetPaginatedResult(LectureId, CurrentPage, PageSize);
        //    Count = await GetCount(LectureId);
        //}

        //private async Task<List<Student>> GetPaginatedResult(int lectureId, int currentPage, int pageSize = 10)
        //{
        //    var lecture = await _presenceHandler.GetLectureAsync(lectureId);

        //    var students = lecture.Students.ToList();

        //    return students.OrderBy(s => s.Name).Skip((currentPage - 1) * PageSize).Take(pageSize).ToList();
        //}

        //private async Task<int> GetCount(int lectureId)
        //{
        //    var lecture = await _presenceHandler.GetLectureAsync(lectureId);
        //    var amountOfStudents = lecture.Students.Count;

        //    if (lecture == null)
        //        return 0;

        //    return amountOfStudents;
        //}
    }
}
