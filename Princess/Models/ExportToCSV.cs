namespace Princess.Models
{
   
    public class ExportToCSV
    {
        
        public string Class { get; set; }
        public string Teacher  { get; set; }
        public DateTime Date { get; set; }
        public string Student { get; set; }
        public bool Present { get; set; }
        public string Reason { get; set; }
    }
}
