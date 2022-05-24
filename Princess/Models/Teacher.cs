namespace Princess.Models
{
    public class Teacher
    {
        //Insert the Discord ID as the teacher's ID
        public ulong Id { get; set; }
        public string Name { get; set; }

        public ICollection<Class>? Classes { get; set; }
        public ICollection<Lecture>? Lectures { get; set; }
    }
}
