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
         
        public CsvProgram(int data)
        {
            int _data = data;

            WriteToCsvFile(_data);
        }
        
        public static async void WriteToCsvFile(int _data)
        {
            var attendenceList = new List<ExportToCSV>()
            {
                new ExportToCSV{student = "Anna bengtsson", presence = true, registerTime=DateTime.UtcNow, teacher="Mr.Bjorn", theClass="win21" }
            };

            var csvFileDescription = new CsvFileDescription
            {
                FirstLineHasColumnNames = true,
                SeparatorChar = '\u002C',
            };

            string rootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var fileOnServer = Path.Combine(rootDirectory, "ExportFile.csv");

            var csvContext = new CsvContext();
            
              csvContext.Write(attendenceList, fileOnServer, csvFileDescription);

            var  launchSettingURLArray = Environment.GetEnvironmentVariable("ASPNETCORE_URLS").Split(";");
            var launchSettingURLWithExportFile = launchSettingURLArray[0] ;

            string testpath = ;
               
        }

    }
}
