using Princess.Data;
using Princess.Models;

namespace Princess.CSV
{
    public class CsvProgram
    {

        private readonly PresenceDbContext _ctx;

        public CsvProgram(PresenceDbContext ctx)
        {
            _ctx = ctx;
        }
        public static void WriteToCsvFile()
        {
            var attendenceList = new List<ExportToCSV>()
            {
                new ExportToCSV{student = "Anna bengtsson", presence = true, registerTime=DateTime.UtcNow, teacher="Mr.Bjorn", theClass="win21" }
            };
        }
    }
}
