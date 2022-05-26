using LINQtoCSV;

namespace Princess.Models
{
    [Serializable]
    public class ExportToCSV
    {
        [CsvColumn(Name="Server/class")]
        public string  theClass { get; set; }
        [CsvColumn(Name = "Teacher")]
        public string teacher  { get; set; }
        [CsvColumn(Name = "Time of registration", OutputFormat = "HH:mm dd-mm-yyy")]
        public DateTime registerTime { get; set; }
        [CsvColumn(Name = "Student Name")]
        public String student { get; set; }
        [CsvColumn(Name = "presence")]
        public Boolean presence { get; set; }
    }
}
