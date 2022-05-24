namespace Princess.Models
{
    public class Student
    {
        //Insert the Discord ID as the student's ID
        public ulong Id { get; set; }

        public string Name { get; set; }

        public string? Response { get; set; }

        public bool Presence { get; set; }

        public string? ReasonAbsence { get; set; }

        public Class? Class { get; set; }
    }
}
