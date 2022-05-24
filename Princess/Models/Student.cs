namespace Princess.Models
{
    public class Student
    {
        //Insert the Discord ID as the student's ID
        public ulong Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public ICollection<Lecture> Lectures { get; set; }

        public ICollection<Class>? Classes { get; set; }
    }
}
