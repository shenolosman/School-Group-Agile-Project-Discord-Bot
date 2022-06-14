namespace Princess.Models;

public class Presence
{
    public int Id { get; set; }

    public bool Attended { get; set; }

    public string? ReasonAbsence { get; set; }

    public Student Student { get; set; }

    public Lecture Lecture { get; set; }
}