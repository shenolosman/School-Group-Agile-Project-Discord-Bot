using System.ComponentModel.DataAnnotations;

namespace Princess.Models;

public class Lecture
{
    public int Id { get; set; }

    [DisplayFormat(DataFormatString = "{0:d}")]
    public DateTime Date { get; set; }

    public Teacher? Teacher { get; set; }

    public Class Class { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public ICollection<Student>? Students { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public ICollection<Presence>? Presences { get; set; }
}