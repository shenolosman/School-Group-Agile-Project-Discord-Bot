using LINQtoCSV;
using Princess.Data;
using Princess.Models;

namespace Princess.CSV
{
    public class CsvProgram
    {

        public CsvProgram(string data)
        {

            WriteToCsvFile();
            string _data = data;
        }
        
        public static void WriteToCsvFile()
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

            var csvContext = new CsvContext();
            csvContext.Write(attendenceList, "csvfilename.csv", csvFileDescription);
            Console.WriteLine("CSV file created");

        }
    }
}
