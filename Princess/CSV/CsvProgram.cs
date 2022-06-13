using LINQtoCSV;
using Microsoft.AspNetCore.Mvc;
using Princess.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Princess.CSV
{
    public class CsvProgram
    {
         
        public CsvProgram(Lecture lectureData)
        {
            Lecture _lectureData = lectureData;

            WriteToCsvFile(_lectureData);
        }
        
        public void WriteToCsvFile(Lecture _data)
        {
            var attendenceList = new List<ExportToCSV>()
            {
                new ExportToCSV{student = "Anna bengtsson", presence = true, registerTime=DateTime.UtcNow, teacher="Mr.Bjorn", theClass="win21" }
            };

            var presenceList = _data.Presences;

            var testAttendanceList = new List<ExportToCSV>() { };

            foreach (var testStudent in _data.Students)
            {
                var presence = presenceList.FirstOrDefault(p => p.Student == testStudent);
                testAttendanceList.Add(new ExportToCSV()
                {
                    theClass = _data.Class.Name,
                    teacher = _data.Teacher.Name,
                    student = testStudent.Name,
                    registerTime = _data.Date,
                    presence = presence.Attended,
                });
            }

            var csvFileDescription = new CsvFileDescription
            {
                FirstLineHasColumnNames = true,
                SeparatorChar = '\u002C',
            };

            string rootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var fileOnServer = Path.Combine(rootDirectory, "ExportFile.csv");

            var csvContext = new CsvContext();
            
              csvContext.Write(attendenceList, fileOnServer, csvFileDescription);

            //var  launchSettingURLArray = Environment.GetEnvironmentVariable("ASPNETCORE_URLS").Split(";");
            //var launchSettingURLWithExportFile = launchSettingURLArray[0] ;
            }

    }
}
