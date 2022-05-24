namespace Princess.Models
{
    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Lecture>? Lectures { get; set; }

        public ICollection<Teacher>? Teachers { get; set; }

        public ICollection<Student>? Students { get; set; }
    }
}
