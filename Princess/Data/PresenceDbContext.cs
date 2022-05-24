using Microsoft.EntityFrameworkCore;

namespace Princess.Data
{
    public class PresenceDbContext : DbContext
    {
        public PresenceDbContext(DbContextOptions<PresenceDbContext> options) : base(options)
        {

        }

        //public DbSet<Class> Classes { get; set; }
        //public DbSet<Lecture> Lectures { get; set; }
        //public DbSet<Presence> Presences { get; set; }
        //public DbSet<Student> Students { get; set; }
        //public DbSet<Teacher> Teachers { get; set; }
    }
}
