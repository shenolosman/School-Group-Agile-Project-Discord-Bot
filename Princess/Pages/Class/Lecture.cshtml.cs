using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Princess.Models;
using Princess.Services;
using Princess.CSV;

namespace Princess.Pages.Class
{
    public class LectureModel : PageModel
    {
        private readonly PresenceHandler _presenceHandler;

        public LectureModel(PresenceHandler presenceHandler)
        {
            _presenceHandler = presenceHandler;
        }

        [BindProperty(SupportsGet = true)] 
        public int LectureId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 10;

        public int TotalPages => (int) Math.Ceiling(decimal.Divide(Count, PageSize));

        public List<Student> Students { get; set; }


        public async Task OnGetAsync()
        {
            Students = await GetPaginatedResult(LectureId, CurrentPage, PageSize);
            Count = await GetCount(LectureId);
        }
        public async Task <IActionResult>  OnPostAsync()
        {
            var lectureIdFromButton = LectureId;
            var allStudentsFromPresenceCheck = await _presenceHandler.GetLectureAsync(lectureIdFromButton);

            var date = allStudentsFromPresenceCheck.Date;
            // TODO EXPORT SHOULD BE IN OnGetAsync instead
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
                    theClass = data.Class.Name,
                    teacher = data.Teacher.Name,
                    student = testStudent.Name,
                    registerTime = data.Date,
                    presence = presence.Attended,
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

        private async Task<List<Student>> GetPaginatedResult(int lectureId, int currentPage, int pageSize = 10)
        {
            var lecture = await _presenceHandler.GetLectureAsync(lectureId);

            var students = lecture.Students.ToList();

            return students.OrderBy(s => s.Name).Skip((currentPage - 1) * PageSize).Take(pageSize).ToList();
        }

        private async Task<int> GetCount(int lectureId)
        {
            var lecture = await _presenceHandler.GetLectureAsync(lectureId);
            var amountOfStudents = lecture.Students.Count;

            if (lecture == null)
                return 0;

            return amountOfStudents;
        }
    }
}
