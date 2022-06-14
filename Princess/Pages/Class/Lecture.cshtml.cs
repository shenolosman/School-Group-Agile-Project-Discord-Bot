using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Princess.Models;
using Princess.Services;

namespace Princess.Pages.Class;

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
        var lecture = await _presenceHandler.GetLecture(lectureId);
        if (lecture == null)
        {
            return;
        }
        IEnumerable<Presence> presencesList = lecture.Presences.ToList();
        if (!string.IsNullOrEmpty(searchString))
        {
            presencesList = presencesList.Where(s => s.Student.Name.ToLower().Contains(searchString));
        }
        switch (sortOrder)
        {
            case "name_desc":
                presencesList = presencesList.OrderByDescending(s => s.Student.Name);
                break;
            case "Date":
                presencesList = presencesList.OrderBy(x => x.Lecture.Date);
                break;
            case "date_desc":
                presencesList = presencesList.OrderByDescending(s => s.Lecture.Date);
                break;
            case "Attended":
                presencesList = presencesList.OrderBy(s => s.Attended);
                break;
            case "attended_desc":
                presencesList = presencesList.OrderByDescending(s => s.Attended);
                break;
            case "ReasonAbsence":
                presencesList = presencesList.OrderBy(s => s.ReasonAbsence);
                break;
            case "reasonAbsence_desc":
                presencesList = presencesList.OrderByDescending(s => s.ReasonAbsence);
                break;
            default:
                presencesList = presencesList.OrderBy(s => s.Student.Name);
                break;
        }
        var pageSize = _configuration.GetValue("PageSize", 10);
        Presences = await PaginatedList<Presence>.CreateAsync(presencesList.ToList(), pageIndex ?? 1, pageSize);
    }
    public async Task<IActionResult> OnPostAsync()
    {
        var lectureIdFromButton = LectureId;
        var allStudentsFromPresenceCheck = await _presenceHandler.GetLecture(lectureIdFromButton);

        var date = allStudentsFromPresenceCheck.Date;
        // TODO EXPORT SHOULD BE IN OnGetAsync instead, maybe in a new razor page that redirects back here when export is done?
        return File(WriteCsvToMemory(allStudentsFromPresenceCheck), "text/csv", $"lecture-{date}.csv");
    }

    private byte[] WriteCsvToMemory(Lecture data)
    {
        var presenceList = data.Presences;

        var testAttendanceList = new List<ExportToCSV>() { };

        foreach (var testStudent in data.Students)
        {
            var presence = presenceList.FirstOrDefault(p => p.Student == testStudent);
            testAttendanceList.Add(new ExportToCSV()
            {
                Class = data.Class.Name,
                Teacher = data.Teacher.Name,
                Student = testStudent.Name,
                Date = data.Date,
                Present = presence.Attended,
                Reason = presence.ReasonAbsence ?? "",
            });
        }

        using (var memoryStream = new MemoryStream())
        using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
        using (var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture)
               {
                   Delimiter = ";",
               }))
        {
            csvWriter.WriteRecords(testAttendanceList);
            streamWriter.Flush();
            return memoryStream.ToArray();
        }
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
}