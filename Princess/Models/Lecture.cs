namespace Princess.Models;

public class Lecture
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public Teacher? Teacher { get; set; }

    public Class Class { get; set; }

    public ICollection<Student>? Students { get; set; }

    public ICollection<Presence>? Presences { get; set; }
}