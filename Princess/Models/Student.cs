using System.ComponentModel.DataAnnotations.Schema;

namespace Princess.Models;

public class Student
{
    //Insert the Discord ID as the student's ID
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public ulong Id { get; set; }

    public string Name { get; set; }

    public ICollection<Presence>? Presences { get; set; }

    public ICollection<Lecture>? Lectures { get; set; }

    public ICollection<Class>? Classes { get; set; }
}