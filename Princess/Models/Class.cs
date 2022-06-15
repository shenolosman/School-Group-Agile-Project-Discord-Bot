using System.ComponentModel.DataAnnotations.Schema;

namespace Princess.Models;

public class Class
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public ulong Id { get; set; }

    public string Name { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public ICollection<Lecture>? Lectures { get; set; }
    public ICollection<Teacher>? Teachers { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public ICollection<Student>? Students { get; set; }
}