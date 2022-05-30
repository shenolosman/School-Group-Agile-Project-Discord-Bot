namespace Princess.Models
{
    public class AttendanceView
    {
        public int CurrentPageIndex { get; set; }
        public int PageCount { get; set; }
        public List<Presence> AttendanceList { get; set; }
    }
}
